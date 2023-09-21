using Castle.Core.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using Timesheets.Infrastructure;
using Timesheets.Models;
using Timesheets.Repositories;
using Timesheets.Services;

namespace Timesheets.Test
{
    public class TimesheetTests
    {
        [Fact]
        public void GivenAValidTimesheet_ThenAddTimesheetToInMemoryDatabase()
        {
            //Arrange
            var timesheet = new Timesheet();
            var timesheetEntry = new TimesheetEntry()
            {
                Id = 1,
                Date = "01/09/2023",
                Project = "Test Project",
                FirstName = "Test",
                LastName = "Test",
                Hours = "7.5"
            };
            timesheet.Id = 1;
            timesheet.TimesheetEntry = timesheetEntry;
            timesheet.TotalHours = timesheetEntry.Hours;

            var mockRepository = new Mock<ITimesheetRepository>();
            var timesheetService = new TimesheetService(mockRepository.Object);

            // Act
            timesheetService.Add(timesheet);

            // Assert
            mockRepository.Verify(repo => repo.AddTimesheet(It.IsAny<Timesheet>()), Times.Once);
        }


        #region get ordered items
        [Fact]
        public void GivenAValidTimesheets_Get3OrderedItems()
        {

            var mockRepository = new Mock<ITimesheetRepository>();
            mockRepository
                .Setup(x => x.GetProjectInfos())
                .Returns(new List<ProjectInfo> {
                    new ProjectInfo
                    {
                        Project = "1",
                        TotalHours = 6.0,
                        Workers =
                        new List<Workers>
                        {
                            new Workers
                            {
                                Name = "TEST 1",
                                HoursWorked = 6,
                                Date = "12/12/12",
                            }
                        }
                    },
                    new ProjectInfo
                    {
                        Project = "2",
                        TotalHours = 1.0,
                        Workers =
                        new List<Workers>
                        {
                            new Workers
                            {
                                Name = "TEST 2",
                                HoursWorked = 1,
                                Date = "11/12/12",
                            }
                        }
                    },
                    new ProjectInfo
                    {
                        Project = "3",
                        TotalHours = 2.0,
                        Workers =
                        new List<Workers>
                        {
                            new Workers
                            {
                                Name = "TEST 3",
                                HoursWorked = 2,
                                Date = "11/12/12",
                            }
                        }
                    },
                });


            var timesheetService = new TimesheetService(mockRepository.Object);

            var sorted = timesheetService.GetProjectInfos();

            Assert.NotNull(sorted);
            Assert.NotEmpty(sorted);
            Assert.Equal(6.0, sorted.First().TotalHours);
        }

        [Fact]
        public void GivenAValidTimesheets_Get1OrderedItems()
        {

            var mockRepository = new Mock<ITimesheetRepository>();
            mockRepository
                .Setup(x => x.GetProjectInfos())
                .Returns(new List<ProjectInfo> {
                    new ProjectInfo
                    {
                        Project = "1",
                        TotalHours = 6.0,
                        Workers =
                        new List<Workers>
                        {
                            new Workers
                            {
                                Name = "TEST 1",
                                HoursWorked = 6,
                                Date = "12/12/12",
                            }
                        }
                    },
                });


            var timesheetService = new TimesheetService(mockRepository.Object);

            var sorted = timesheetService.GetProjectInfos();

            Assert.NotNull(sorted);
            Assert.NotEmpty(sorted);
            Assert.Single(sorted);
            Assert.Equal(6.0, sorted.First().TotalHours);
        }

        [Fact]
        public void GivenAValidTimesheets_Get0OrderedItems()
        {

            var mockRepository = new Mock<ITimesheetRepository>();
            mockRepository
                .Setup(x => x.GetProjectInfos())
                .Returns(new List<ProjectInfo>
                {
                });


            var timesheetService = new TimesheetService(mockRepository.Object);

            var sorted = timesheetService.GetProjectInfos();

            Assert.NotNull(sorted);
            Assert.Empty(sorted);
        }
        #endregion
    }
}
