namespace Timesheets.Models
{
    public class ProjectInfo
    {
        public string Project { get; set; }
        public double TotalHours { get; set; }
        public IEnumerable<Workers> Workers { get; set; }
    }
}
