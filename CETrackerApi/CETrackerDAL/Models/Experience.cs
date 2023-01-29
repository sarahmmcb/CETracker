using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETrackerDAL.Models;

public class Experience
{
    public int ExperienceId { get; set; }
    public int UserId { get; set; }
    public int LocationId { get; set; }
    public bool CarryForward { get; set; }
    public string ProgramTitle { get; set; }
    public string EventName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public string Notes { get; set; }
    public bool IsDeleted { get; set; }
}
