using System;
using System.Collections.Generic;
using System.Text;

namespace UPSTask.TaskObjects
{
    public class Pagination
    {
        public int Total { get; set; }
        public int Pages { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }
}
