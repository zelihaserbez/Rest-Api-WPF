using System;
using System.Collections.Generic;
using System.Text;

namespace UPSTask.TaskObjects
{
    public class UserUpdateResponse
    {
        public int code { get; set; }
        public Meta Meta { get; set; }
        public User? Data { get; set; }

    }
}
