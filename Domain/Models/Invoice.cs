using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Invoice : BaseModel
    {
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime IssueDate { get; set; }

        public string InvoiceBody { get; set; }
    }
}