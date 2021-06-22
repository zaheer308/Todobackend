using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoBackend.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public int Verification { get; set; }
        public int IsVerified { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }

    public class Params
    {
        public string paramstring { get; set; }
    }

    public class LoginData
    {
        public string username { get; set; }
        public string password { get; set; }

    }

    public class VerifyModel
    {
        public string Username { get; set; }
        public int Verification { get; set; }

    }
}