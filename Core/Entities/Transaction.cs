using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public Guid UserId { get; set; }

        public TransactionType Type { get; set; }
        public DateTime TransactionTime { get; set; }
        public string Description { get; set; }
    }
}
