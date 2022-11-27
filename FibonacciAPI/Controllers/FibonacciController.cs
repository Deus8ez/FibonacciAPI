using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FibonacciAPI.Interfaces;
using FibonacciAPI.DTOs;

namespace FibonacciAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FibonacciController : ControllerBase
    {
        IFibonacciService fibbonaciService { get; set; }
        public FibonacciController(IFibonacciService fibbonaciService)
        {
            this.fibbonaciService = fibbonaciService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int startIndex, [FromQuery]int endIndex, [FromQuery]bool fromCache, [FromQuery]int milliseconds)
        {
            if (startIndex > endIndex)
            {
                return BadRequest("start index should be less than end index");
            }

            var result = await Task.Run(() => fibbonaciService.GetSequence(startIndex, endIndex, fromCache))
                                                                .WaitAsync(TimeSpan.FromMilliseconds(milliseconds));
            
            return Ok(new GetSequenceResponse { sequence = result });
        }
    }
}
