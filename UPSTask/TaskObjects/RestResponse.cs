using System;
using System.Collections.Generic;
using System.Text;

namespace UPSTask.TaskObjects
{
    public class RestResponse
    {
        public int code { get; set; }
        public Meta Meta { get; set; }
        public List<User> Data { get; set; }
    }
}
