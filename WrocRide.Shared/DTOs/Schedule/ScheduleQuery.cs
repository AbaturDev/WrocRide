using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrocRide.Shared.DTOs.Schedule
{
    public class ScheduleQuery
    {
        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
    }
}
