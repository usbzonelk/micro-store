using System;
using System.Net;

namespace UserService.Models
{
    public class APIResponse
    {
        public APIResponse()
        {
            Errors = new List<string>();
        }
        public HttpStatusCode Status { get; set; }
        public bool Successful { get; set; } = true;
        public List<string> Errors { get; set; }
        public object Result { get; set; }
    }
}
