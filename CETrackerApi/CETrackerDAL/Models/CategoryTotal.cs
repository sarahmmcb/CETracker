using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETrackerDAL.Models;
public class CategoryTotal
{
    public int CategoryId { get; set; }
    public string DisplayName { get; set; }
    public decimal Total { get; set; }
    public string UnitLongNamePlural { get; set; }
}
