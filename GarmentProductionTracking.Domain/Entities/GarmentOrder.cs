
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("tb_gp_orders", Schema = "aa")]
    public class GarmentOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//Auto-increment primary key
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string GarmentType { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public DateTime OrderDate { get; set; } 

        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public DateTime DeliveryDate { get; set; } 

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        [Column(TypeName = "bytea")] 
        public byte[]? ImageContent { get; set; }

        // Foreign key to User
        [Required]
        public int CustomerId { get; set; }
        public User? Customer { get; set; }   // optional navigation

        //  Add this
        [Column(TypeName = "text")]
        public string? CustomerInstructions { get; set; }
    }
}