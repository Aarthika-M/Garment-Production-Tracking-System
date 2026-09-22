using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{

    [Table("tb_gp_users", Schema = "aa")]
    public class User
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Username")]
        public string Username { get; set; } = null!;//it must have value

        [Column("PasswordHash")]
        public string PasswordHash { get; set; } = null!;

        [Column("Role")]
        public Role Role { get; set; }
    }
}