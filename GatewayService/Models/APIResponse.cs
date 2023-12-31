using System;
using System.Net;

namespace GatewayService.Models.DTO
{
    public class APIResponse <T>
    {
        public APIResponse()
        {
            Errors = new List<string>();
        }
        public HttpStatusCode Status { get; set; }
        public bool Successful { get; set; } = true;
        public List<string> Errors { get; set; }
        public T Result { get; set; }
    }
}
