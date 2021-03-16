using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DeskHelpAPI.Data
{
    public class User
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }

        [Required]
        public byte[] UHash { get; set; }

        [Required]
        public byte[] USalt { get; set; }


        public void GeneratePassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                USalt = hmac.Key;
                UHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool ValidatePassword(string password)
        {
            using (var hmac = new HMACSHA512(USalt))
            {
                var ch = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < ch.Length; i++)
                {
                    if (ch[i] != UHash[i])
                        return false;
                }
            }

            return true;
        }
    }
}
