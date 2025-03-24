using System.ComponentModel.DataAnnotations;

namespace CoE.Assessment.Orders.Application.Models
{
    public class UpdateOrder
    {
        [Required]
        public int CustomerId { get; init; }
    }
}
