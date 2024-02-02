using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;
using NotificationService.Models.Entities;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly ProducerService _producerService;

        public ProducerController(IConfiguration configuration)
        {
            _producerService = new ProducerService(configuration);
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromForm] Email model)
        {
            await _producerService.SendMessageAsync(model);
            return Ok();
        }
    }
}
