using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Razor_WebAPI_Application.Models;

namespace Razor_WebAPI_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        public ChartData chartData = new ChartData();

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
            // Read received data
            var newDataX = data.X; 
            var newDataY = data.Y;
            var chartData = new { x = new List<double>[] { newDataX }, y = new List<double>[] { newDataY } };
            return Ok(chartData);
        }
    }
}
