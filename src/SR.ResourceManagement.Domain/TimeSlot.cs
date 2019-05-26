using System;
namespace SR.ResourceManagement.Domain
{
    public class TimeSlot
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public ScheduleStatus Status { get; set; }

    }
}