using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities
{  
     [Table("RolePermissions", Schema = "aa")]
    public class RolePermission
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public int Id { get; set; }
         [Column("Role")]
        public string Role { get; set; }   
         [Column("Controller")]     
        public string Controller { get; set; }  
         [Column("Action")]
        public string Action { get; set; }   
         [Column("Enabled")]   
        public bool Enabled { get; set; }      
    }
}
