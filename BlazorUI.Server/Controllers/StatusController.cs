using BlazorUI.Server.Attributes;
using BlazorUI.Shared.Events;
using BlazorUI.Shared.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Totem.Timeline.Mvc;

namespace BlazorUI.Server.Controllers
{
    /// <summary>
    /// Controls interactions with the API root
    /// </summary>
    [Route("[controller]")]
    public class StatusController : Controller
    {
        private IQueryServer _queries { get; set; }
        private ICommandServer _commands { get; set; }

        public StatusController(IQueryServer queries, ICommandServer commands)
        {
            _queries = queries;
            _commands = commands;
        }

        [HttpGet("[action]")]
        [TimelineQuery(typeof(EchoQuery))]
        public Task<IActionResult> GetEcho() => _queries.Get<EchoQuery>();

        [HttpPost("[action]")]
        public Task<IActionResult> SendEcho()
        {
            var test = new LogMonitorService();
            test.Test();
            return _commands.Execute(new Echo(),
When<EchoSuccess>.ThenOk, When<EchoFailure>.ThenBadRequest);
        }
    }
}