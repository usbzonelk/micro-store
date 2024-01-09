using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Http.Features;
using MessageBus.RabittMQ;
using MessageBus.Models;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

namespace MessageBus.Controllers.v1
{
    [Route("api/v1/email")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly ILogger<MailController> _logger;
        private IRabitMQProducer _rabbitMQProducer;
        public MailController(ILogger<MailController> logger, IRabitMQProducer rabbitMQProducer)
        {
            _logger = logger;
            _rabbitMQProducer = rabbitMQProducer;

        }

        [HttpPost("sendnow", Name = "sendMail")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<object> SendMail([FromBody] Email email)
        {
            if (email is null)
            {
                return BadRequest("Invalid email format!");
            }

            _rabbitMQProducer.SendEmail(email);
            return Ok("Mail has been sent!");
        }
    }
}