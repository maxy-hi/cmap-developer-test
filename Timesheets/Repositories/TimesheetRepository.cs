using Microsoft.EntityFrameworkCore.Infrastructure;
using Timesheets.Infrastructure;
using Timesheets.Models;

namespace Timesheets.Repositories
{
    public interface ITimesheetRepository
    {
        void AddTimesheet(Timesheet timesheet);
        IList<Timesheet> GetAllTimesheets();
        IList<ProjectInfo> GetProjectInfos();
    }

    public class TimesheetRepository : ITimesheetRepository
    {
        private DataContext _context;

        public TimesheetRepository(DataContext context)
        {
            _context = context;
        }
        public void AddTimesheet(Timesheet timesheet)
        {
            //Added this as without it the reference for the timesheet entry is lost
            _context.TimesheetEntrys.Add(timesheet.TimesheetEntry);
            _context.Timesheets.Add(timesheet);
            _context.SaveChanges();
        }

        public IList<Timesheet> GetAllTimesheets()
        {
            var timesheets = _context.Timesheets.ToList();
            return timesheets;
        }

        public IList<ProjectInfo> GetProjectInfos()
        {
            var projectInfos = _context.Timesheets
                                .Select(e => e.TimesheetEntry)
                                .GroupBy(e => e.Project)
                                .Select(e =>
                                    new ProjectInfo
                                    {
                                        Project = e.Key,
                                        TotalHours = e.Sum(e => Convert.ToDouble(e.Hours)),
                                        Workers = e.Select(i =>
                                            new Workers
                                            {
                                                Name = $"{i.FirstName} {i.LastName}",
                                                HoursWorked = Convert.ToDouble(i.Hours),
                                                Date = i.Date,
                                            }).ToList(),
                                    })
                                .AsEnumerable()
                                .OrderByDescending(e => e.TotalHours)
                                .ToList();

            return projectInfos;
        }
    }
}
