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
        public Guid TransactionId { get; private set; }
        public Guid UserId { get; set; }
        public TransactionSuccess Success { get; set; }
        public TransactionType Type { get; set; }
        public DateTime TransactionTime { get; private set; }
        public string Description { get; set; }

        public Transaction()
        {
            this.TransactionId = Guid.NewGuid();
            this.TransactionTime = DateTime.UtcNow;
        }
    }

}
