using System.Collections.Generic;
using SR.ResourceManagement.Domain;
using System;
using Xunit;
using System.Linq;

namespace Test.Scheduling.ScheduleManager.GivenSimpleRecurringSchedule
{
    public class WhenExpandingSchedule
    {
        private Schedule _MySchedule = new Schedule
        {
            RecurringSchedules = new List<RecurringSchedule>{
                new RecurringSchedule
                {
                    MinStartDateTime = new DateTime(2019,5,1),
                    MaxEndDateTime = new DateTime(2019,5,30),
                    CronPattern = "0 15 * * 1",
                    Duration = TimeSpan.FromHours(1)
                }
            }    
        };

        [Fact]
        public void ShouldExpandWithinTheLimitedDates()
        {
            var sut = new SR.ResourceManagement.Scheduling.ScheduleManager();
            var results = sut.ExpandSchedule(_MySchedule,new DateTime(2019,1,1), new DateTime(2019,12,31));
            
            //assert
            Assert.Equal(4,results.Count());
            Assert.Equal(15,results.First().StartDateTime.Hour);
        }

        [Fact]
        public void ShouldExpandWithinTheRequestedDates()
        {
            var sut = new SR.ResourceManagement.Scheduling.ScheduleManager();
            var results = sut.ExpandSchedule(_MySchedule,new DateTime(2019,5,1), new DateTime(2019,5,15));
            
            //assert
            Assert.Equal(2,results.Count());
            Assert.Equal(15,results.First().StartDateTime.Hour);
        }
    }
}