using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerJob.Model
{
    class Employee
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsLinkAccessed { get; set; }
        public int RemainderCount { get; set; }
    }
}
