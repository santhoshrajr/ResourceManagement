﻿using System;
using System.Collections.Generic;

namespace SR.ResourceManagement.Domain
{
    public class Schedule
    {
        public int Id { get; set; }
        public IList<ScheduleItem> ScheduleItems { get; set; }

        public IList<RecurringSchedule> RecurringSchedules { get; set; }
        public IList<(DateTime start, DateTime end)> ScheduleExceptions { get; set; }
    }
}