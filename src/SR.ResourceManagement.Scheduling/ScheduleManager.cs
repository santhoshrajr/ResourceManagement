using System;
using System.Collections.Generic;
using System.Linq;
using SR.ResourceManagement.Domain;

namespace SR.ResourceManagement.Scheduling
{
    public class ScheduleManager
    {
        public IEnumerable<TimeSlot> ExpandSchedule (Schedule schedule, DateTime startTime, DateTime endTime)
        {
             var outSchedule = new List<TimeSlot>();
             outSchedule.AddRange(ExpandScheduleItems(schedule.ScheduleItems,startTime,endTime));
             return outSchedule;
        }

        private IEnumerable<TimeSlot> ExpandScheduleItems(IList<ScheduleItem> scheduleItems, DateTime startTime, DateTime endTime)
        {
            return scheduleItems
            .Where( i => i.StartDateTime < endTime && i.EndDateTime >startTime)
            .Select(i => new TimeSlot(){
                StartDateTime = i.StartDateTime,
                EndDateTime = i.EndDateTime,
                Status = i.Status
            })
            .OrderBy(t => t.StartDateTime);
        }
    }
}
