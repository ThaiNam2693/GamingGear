using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class OrderObject
    {
        public string OrderId { get; set; } = null!;

        public int? StaffId { get; set; }

        public string Username { get; set; } = null!;

        public decimal? TotalPrice { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Description { get; set; }

        public int? Status { get; set; }

        public virtual ManagerObject? Staff { get; set; }

        public virtual CustomerObject UsernameNavigation { get; set; } = null!;
    }
}
