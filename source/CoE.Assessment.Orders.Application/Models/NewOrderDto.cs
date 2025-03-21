using System.ComponentModel.DataAnnotations;

namespace CoE.Assessment.Orders.Application.Models
{
    public record NewOrderDto
    {
        [Required]
        public int CustomerId { get; init; }

        [Required]
        public int ProductId { get; init; }

        [Required]
        [Range(1, 10)]
        public int Quantity { get; init; }        
    }
}
