using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class Transaction
    {
        public long ID { get; set; }
        public long AccountNumber { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public long Source { get; set; }
        public long Destination { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
