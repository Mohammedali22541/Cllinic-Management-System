using ClinicApp.Data.Entites.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Data.Entites
{
    public class DoctorAvailability : BaseEntity<int>
    {
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;

        public DayOfWeek DayOfWeek { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public int SlotDurationInMinutes { get; set; } = 30;
    }
}
