using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Business.Dtos.AvailabilityDto
{
    public class CreateDoctorAvailabilityDto
    {
        public int DoctorId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int SlotDurationInMinutes { get; set; } = 30;
    }
}
