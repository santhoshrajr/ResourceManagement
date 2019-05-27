using System;
using System.Collections.Generic;
using System.Linq;
using SR.ResourceManagement.Domain;
using SR.ResourceManagement.Scheduling;
using Xunit;

namespace Test.Scheduling.GivenSimpleScheduler.GivenSimpleScheduler
{
    public class WhenExpandingSchedule
    {

        private Schedule _MySimpleSchedule = new Schedule()
        {
            ScheduleItems = new List<ScheduleItem>()
                {
                    new ScheduleItem
                    {
                        StartDateTime = new DateTime(2019,5,23,15,0,0),
                        EndDateTime = new DateTime(2019,5,23,16,0,0)
                    }
                }
        };

        private Schedule _MyMultipleItemsSchedule = new Schedule()
        {
            ScheduleItems = new List<ScheduleItem>()
                {
                    new ScheduleItem
                    {
                        StartDateTime = new DateTime(2019,5,23,15,0,0),
                        EndDateTime = new DateTime(2019,5,23,16,0,0)
                    },
                    new ScheduleItem
                    {
                        StartDateTime = new DateTime(2019,5,26,17,0,0),
                        EndDateTime = new DateTime(2019,5,26,18,0,0)
                    }
                }
        };
        [Fact]
        public void ShouldCreateTheScheduleItemsSpecified()
        {
            //act
            var sut = new SR.ResourceManagement.Scheduling.ScheduleManager();
            var results = sut.ExpandSchedule(_MySimpleSchedule,new DateTime(2019,1,1), new DateTime(2019,12,31));
            
            //assert
            Assert.Single(results);
            Assert.Equal(15,results.First().StartDateTime.Hour);
        }

        [Fact]
        public void ShouldCreateMultipleScheduleItemsSpecified()
        {
            //arrange
            //act
            var sut = new SR.ResourceManagement.Scheduling.ScheduleManager();
            var results = sut.ExpandSchedule(_MyMultipleItemsSchedule,new DateTime(2019,1,1), new DateTime(2019,12,31));
            
            //assert
            Assert.NotEmpty(results);
            Assert.Equal(2,results.Count());
            Assert.Equal(15,results.First().StartDateTime.Hour);
            Assert.Equal(17,results.Skip(1).First().StartDateTime.Hour);
        }

        [Fact]
        public void ShouldExpandWithinDatesSpecified()
        {
            //arrange
            var sut = new SR.ResourceManagement.Scheduling.ScheduleManager();
            var results = sut.ExpandSchedule(_MyMultipleItemsSchedule,new DateTime(2019,1,1), new DateTime(2019,5,24));
            
            //assert
            Assert.Single(results);
        }
    }
}