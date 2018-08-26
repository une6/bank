using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class User
    {
        public long ID { get; set; }
        [Required]
        public string LoginName { get; set; }
        public long AccountNumber { get; set; }
        [Required]
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
