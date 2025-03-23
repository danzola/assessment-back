using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using CoE.Assessment.Domain.Commands;
using CoE.Assessment.Domain.Events;
using CoE.Assessment.Domain.Bus;

namespace CoE.Assessment.Infrastructure.Bus
{
    public sealed class RabbitMQBus(IServiceScopeFactory serviceScopeFactory) : IEventBus, ICommandBus, IAsyncDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly Dictionary<string, List<Type>> _eventHandlers = [];
        private readonly Dictionary<string, List<Type>> _commandHandlers = [];
        private readonly List<Type> _eventTypes = [];
        private readonly List<Type> _commandTypes = [];
        private IConnection? _connection;
        private IChannel? _channel;

        private async Task InitializeConnectionAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
        }

        public async Task PublishEvent<T>(T @event) where T : Event
        {
            if (_connection is null || _channel is null)
            {
                await InitializeConnectionAsync();
            }

            var eventName = @event.GetType().Name;

            await _channel!.QueueDeclareAsync(eventName, false, false, false, null);

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            await _channel.BasicPublishAsync(exchange: "", routingKey: eventName, body: body);
        }

        public async Task SendCommand<T>(T command) where T : Command
        {
            if (_connection is null || _channel is null)
            {
                await InitializeConnectionAsync();
            }

            var commandName = command.GetType().Name;

            await _channel!.QueueDeclareAsync(commandName, false, false, false, null);

            var message = JsonConvert.SerializeObject(command);
            var body = Encoding.UTF8.GetBytes(message);

            await _channel.BasicPublishAsync(exchange: "", routingKey: commandName, body: body);
        }

        public async Task SubscribeEvent<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            if (_connection is null || _channel is null)
            {
                await InitializeConnectionAsync();
            }

            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if (!_eventHandlers.ContainsKey(eventName))
            {
                _eventHandlers.Add(eventName, []);
            }

            if (_eventHandlers[eventName].Any(s => s.GetType() == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already is registered for '{eventName}'", nameof(handlerType));
            }

            _eventHandlers[eventName].Add(handlerType);

            await StartBasicConsumeAsync<T, TH>();
        }

        public async Task SubscribeCommand<T, TH>()
            where T : Command
            where TH : ICommandHandler<T>
        {
            if (_connection is null || _channel is null)
            {
                await InitializeConnectionAsync();
            }

            var commandName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!_commandTypes.Contains(typeof(T)))
            {
                _commandTypes.Add(typeof(T));
            }

            if (!_commandHandlers.ContainsKey(commandName))
            {
                _commandHandlers.Add(commandName, []);
            }

            if (_commandHandlers[commandName].Any(s => s.GetType() == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already is registered for '{commandName}'", nameof(handlerType));
            }

            _commandHandlers[commandName].Add(handlerType);

            await StartBasicConsumeAsync<T, TH>();
        }

        private async Task StartBasicConsumeAsync<T, TH>()
            where T : Message
            where TH : IHandler<T>
        {
            var queueName = typeof(T).Name;

            await _channel!.QueueDeclareAsync(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += Consumer_ReceivedAsync<T, TH>;

            await _channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: true,
                consumer: consumer);
        }

        private async Task Consumer_ReceivedAsync<T, TH>(object sender, BasicDeliverEventArgs e)
            where T : Message
            where TH : IHandler<T>
        {
            var messageName = e.RoutingKey;
            var body = @e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            try
            {
                if (typeof(T).IsSubclassOf(typeof(Event)))
                {
                    await ProcessMessage<T, TH>(messageName, message,_eventHandlers,_eventTypes).ConfigureAwait(false);
                }
                else if (typeof(T).IsSubclassOf(typeof(Command)))
                {
                    await ProcessMessage<T, TH>(messageName, message, _commandHandlers, _commandTypes).ConfigureAwait(false);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported message type: {typeof(T).Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error consuming message: {ex.Message}");
                throw;
            }
        }        

        private async Task ProcessMessage<T, TH>(
            string messageName, 
            string message,
            Dictionary<string, List<Type>> handlerDictionary,
            List<Type> messageTypes)
            where T : Message
            where TH : IHandler<T>
        {
            if (handlerDictionary.TryGetValue(messageName, out List<Type>? subscriptions))
            {
                using var scope = _serviceScopeFactory.CreateScope();
                foreach (var subscription in subscriptions)
                {
                    var handler = scope.ServiceProvider.GetRequiredService<TH>();
                    if (handler == null) continue;

                    var messageType = messageTypes.SingleOrDefault(t => t.Name == messageName);
                    if (messageType == null) continue;

                    var messageHandled = JsonConvert.DeserializeObject(message, messageType);
                    if (messageHandled == null) continue;

                    await handler.Handle((T)messageHandled);
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel?.IsOpen == true)
                await _channel.CloseAsync();

            if (_connection?.IsOpen == true)
                await _connection.CloseAsync();

            GC.SuppressFinalize(this);
        }
    }
}
