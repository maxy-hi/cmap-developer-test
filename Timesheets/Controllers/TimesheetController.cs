using Microsoft.AspNetCore.Mvc;
using SoftCircuits.CsvParser;
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

        public IActionResult DownloadCsv()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (var writer = new CsvWriter(memoryStream))
            {
                var projectInfo = _timesheetService.GetProjectInfos();

                writer.Write("Project", "Total Hours", "Name", "Date", "Hours Worked");
                foreach (var project in projectInfo)
                {
                    foreach (var worker in project.Workers)
                    {
                        writer.Write($"{project.Project}, {project.TotalHours}, {worker.Name}, {worker.Date}, {worker.HoursWorked}");
                    }
                }

                writer.Flush(); // This is important!

                memoryStream.Seek(0, SeekOrigin.Begin);
                FileContentResult result = new FileContentResult(memoryStream.GetBuffer(), "text/csv")
                {
                    FileDownloadName = "Filename.csv"
                };

                return result;
            }
        }
    }
}