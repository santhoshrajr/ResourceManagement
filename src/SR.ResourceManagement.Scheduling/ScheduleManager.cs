using System;
using System.Collections.Generic;
using System.Linq;
using NCrontab;
using SR.ResourceManagement.Domain;

namespace SR.ResourceManagement.Scheduling
{
    public class ScheduleManager
    {
        public IEnumerable<TimeSlot> ExpandSchedule (Schedule schedule, DateTime startTime, DateTime endTime)
        {
             var outSchedule = new List<TimeSlot>();
             if(schedule.ScheduleItems != null)
             {
                 outSchedule.AddRange(ExpandScheduleItems(schedule.ScheduleItems,startTime,endTime));
             }
             if(schedule.RecurringSchedules != null)
             {
                 outSchedule.AddRange(ExpandRecurringSchedules(schedule.RecurringSchedules,startTime,endTime));
             }
             return outSchedule;
        }

        /**
        Scenarios for schedule overlap 
        My Schedule                 /--------/
        Scenario 1:      /---------/
        Scenario 2:                           /--------/
        Scneario 3:             /--------------/
        Scneario 4:                     /--/
        Scenario 5:            /--------/
        Scneario 6:                     /-----------/
         */

        private static (DateTime begin,DateTime end)?  CalcualteIntersectionOfTimeSlots((DateTime StartDateTime,DateTime EndDateTime) ts1, (DateTime StartDateTime,DateTime EndDateTime) ts2)
        {
            //Eliminate scenario 1 and 2
            if(!(ts1.StartDateTime <= ts2.EndDateTime   && ts2.StartDateTime <= ts1.EndDateTime))
             return null;
            //scenario 3
            if(ts1.StartDateTime >= ts2.StartDateTime && ts1.EndDateTime <= ts2.EndDateTime) 
            return (ts1.StartDateTime,ts1.EndDateTime);
            //Scenario 4
            if(ts1.StartDateTime <= ts2.StartDateTime && ts1.EndDateTime >= ts2.EndDateTime) 
            return (ts2.StartDateTime,ts2.EndDateTime);
            //Scenario 5
            if(ts1.StartDateTime >= ts2.StartDateTime && ts1.EndDateTime <= ts2.EndDateTime) return (ts1.StartDateTime,ts2.EndDateTime);
            //Scenario 6
            if(ts1.StartDateTime <= ts2.StartDateTime && ts1.EndDateTime <= ts2.EndDateTime) 
            return (ts2.StartDateTime,ts1.EndDateTime);
            return null;
        }
        private IEnumerable<TimeSlot> ExpandRecurringSchedules(IList<RecurringSchedule> recurringSchedules, DateTime startTime, DateTime endTime)
        {
            var outList = new List<TimeSlot>();
            foreach(var r in recurringSchedules)
            {
                var expandRange = CalcualteIntersectionOfTimeSlots((startTime,endTime),(r.MinStartDateTime,r.MaxEndDateTime));
                if(expandRange == null) continue;
                var cts = CrontabSchedule.Parse(r.CronPattern);
                outList.AddRange(cts.GetNextOccurrences(expandRange.Value.begin,expandRange.Value.end)
                    .Select(d => new TimeSlot{
                        StartDateTime = d,
                        EndDateTime = d.Add(r.Duration),
                        Status = r.Status
                    }));
            }
            return outList;
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
