using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("tb_gp_orderass", Schema = "aa")]
    public class OrderAssignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        public GarmentOrder? Order { get; set; }  // Navigation

        [Required]
        public int ManagerId { get; set; }
        public User? Manager { get; set; }        // Navigation

        [Required]
        public int WorkerId { get; set; }
        public User? Worker { get; set; }         // Navigation

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Assigned";   // Manager instructions


        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "timestamp with time zone")]
        public DateTime? CompletedDate { get; set; }
    }
}