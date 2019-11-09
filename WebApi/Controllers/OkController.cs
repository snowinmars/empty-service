using System.Threading.Tasks;
using EmptyService.Logger.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EmptyService.WebApi.Controllers
{
    // ReSharper disable once AllowPublicClass
    [ApiController]
    [Route("api/[controller]")]
    public sealed class OkController : ControllerBase
    {
        public OkController(ILog log)
        {
            this.log = log;
        }

        private readonly ILog log;

        [HttpGet("")]
        public async Task<string> GetAsync()
        {
            log.Information("The endpoint was called");

            return await Task.FromResult("ok");
        }
    }
}