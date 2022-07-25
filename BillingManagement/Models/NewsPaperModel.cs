using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingManagement.Models
{
    public class NewsPaperModel
    {
        public int Id { get; set; }
        public string PaperName { get; set; }
        public decimal SundayCost { get; set; }
        public decimal MondayCost { get; set; }
        public decimal TuesdayCost { get; set; }
        public decimal WednesdayCost { get; set; }
        public decimal ThursdayCost { get; set; }
        public decimal FridayCost { get; set; }
        public decimal SaturdayCost { get; set; }
        public DateTime Applydate { get; set; }
        public int NewsPaperCostId { get; set; }
    }
}