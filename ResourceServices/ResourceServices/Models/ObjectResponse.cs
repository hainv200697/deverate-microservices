using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenServices.Models
{
    public class ObjectResponse
    {
        public Status status = new Status(200, "Success");
        public Dictionary<String, dynamic> data { get; set; }
    }

    public class Status
    {
        public int code { get; set; }
        public string message { get; set; }
        public Status(int code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public Status()
        {
        }
    }
}
