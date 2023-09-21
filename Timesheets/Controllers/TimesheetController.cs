using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using Timesheets.Models;
using Timesheets.Services;

namespace Timesheets.Controllers
{
    public class TimesheetController : Controller
    {
        private ITimesheetService _timesheetService;

        public TimesheetController(ITimesheetService timesheetService)
        {
            _timesheetService = timesheetService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(TimesheetEntry timesheetEntry)
        {
            var timesheet = new Timesheet()
            {
                TimesheetEntry = timesheetEntry,
                TotalHours = timesheetEntry.Hours
            };

            _timesheetService.Add(timesheet);

            ViewData["ProjectInfo"] = _timesheetService.GetProjectInfos();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void GetCsv()
        {
            //NB: this should be changed the
            var file = @"F:\myOutput.csv";

            using (var writer = new StreamWriter(file))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                var projectInfo = _timesheetService.GetProjectInfos();
                var mapped = new List<object>();
                foreach (var project in projectInfo)
                {
                    foreach (var worker in project.Workers)
                    {
                        //turn the projectInfo into a flat object to then pass it to a CSV
                        mapped.Add(new { project.Project, project.TotalHours, worker.Name, worker.Date, worker.HoursWorked });
                    }
                }
                csv.WriteRecords(mapped);
                csv.Flush();
            }

            //As this is in memory the info will get deleted after this as the page has to be directed to
        }

    }
}