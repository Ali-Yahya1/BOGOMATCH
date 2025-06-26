using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOGOMATCH_DOMAIN.MODELS.Product
{
    [Table("BOGO_PRODUCTS")]
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal RealPrice { get; set; }
        public decimal SplitPrice { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
        public string SpecialOffer { get; set; }
        public ICollection<RatingReview> Reviews { get; set; }
    }
}
