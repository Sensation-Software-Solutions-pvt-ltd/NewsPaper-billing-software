using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingManagement.Models
{
    public class InvoiceDetailModel
    {
        public int NewsPaperId { get; set; }
        public string NewsPaperName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfCopies { get; set; }
        public decimal Cost { get; set; }
    }
}
