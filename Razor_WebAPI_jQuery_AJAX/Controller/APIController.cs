using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Razor_WebAPI_jQuery_AJAX.Models;
using Microsoft.AspNetCore.SignalR;
using Razor_WebAPI_Application.Hubs;

namespace Razor_WebAPI_jQuery_AJAX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AjaxAPIController : ControllerBase
    {
        public ChartData chartData = new ChartData();

        private readonly IHubContext<ChartHub> _hubContext;
        public AjaxAPIController(IHubContext<ChartHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        [Route("ChartDataJson")]
        public IActionResult ChartDataJson()
        {
            // First example
            var x = new List<double> { 1.0,};
            var y = new List<double> { 10.0,};
            var chartData = new { x, y };
            return Ok(chartData);
        }

        [HttpGet]
        [Route("ChartDataJson2")]
        public IActionResult ChartDataJson2()
        {
            // Second Example
            var x = new List<double> {1.0, 20.0, 30.0 };
            var y = new List<double> {2.0, 30.0,  45.0};
            var chartData = new { x, y };
            return Ok(chartData);
        }

        [HttpPost]
        [Route("UpdateChartData")]
        public IActionResult UpdateChartData([FromBody] ChartData data)
        {
            var chartData = new
            {
                x = data.X,
                y = data.Y
            };
            _hubContext.Clients.All.SendAsync("UpdateChart", chartData);
            return Ok(chartData);
        }
    }
}
