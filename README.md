# RazorPageChart
Implementation of a Razor page displays a chart with SignalR

[Stack Overflow complete answer on Updating values of a chart in a razor page](https://stackoverflow.com/a/77428238/8017690)

<b>Implementation</b>

1. Via NuGet, install Microsoft.AspNetCore.SignalR.Common 1.1.0

2. Right-click the project > Manage Client-side Libraries, it will generate a *libman.json* file. In the file, you install the @microsoft/signalr library as below:

```JSON
{
  "version": "1.0",
  "defaultProvider": "unpkg",
  "libraries": [
    {
      "library": "@microsoft/signalr@latest",
      "destination": "wwwroot/js/signalr",
      "files": [
        "dist/browser/signalr.js",
        "dist/browser/signalr.min.js"
      ]
    }
  ]
}
```

3. Create a [SignalR hub][2].

```csharp
namespace Razor_WebAPI_Application.Hubs
{
    public class ChartHub : Hub
    {
        public async Task UpdateChart(ChartData chartData)
        {
            await Clients.All.SendAsync("UpdateChart", chartData);
        }
    }
}
```

4. [Configure SignalR in Startup.cs][3]

```csharp
public class Startup 
{
    public void ConfigureServices(IServiceCollection services)
    {
        ...
        services.AddSignalR();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        ...

        app.UseSignalR(routes =>
            {
                routes.MapHub<ChartHub>("/chartHub");
            });
    }
}
```

5. Get the `ChartHub` from the DI. And emit the data to all clients.

```csharp
using Microsoft.AspNetCore.SignalR;
using Razor_WebAPI_Application.Hubs;

[Route("api/[controller]")]
[ApiController]
public class AjaxAPIController : ControllerBase
{

    private readonly IHubContext<ChartHub> _hubContext;

    public AjaxAPIController(IHubContext<ChartHub> hubContext)
    {
        _hubContext = hubContext;
    }
}
```

```csharp
[HttpPost]
[Route("UpdateChartData")]
public IActionResult UpdateChartData([FromBody] ChartData data)
{
    // Read received data
    var newDataX = data.X;
    var newDataY = data.Y;
    var chartData = new { x = new List<double>[] { newDataX }, y = new List<double>[] { newDataY } };

    _hubContext.Clients.All.SendAsync("UpdateChart", chartData);

    return Ok(chartData);
}
```

6. [Add SignalR client code][4]. With this, you wait for the SignalRHub to emit the "UpdateChart" event and update the chart but not send the POST request to the "UpdateChartData" API that requires the request body. 

```js
<script src="~/js/signalr/dist/browser/signalr.js"></script>

<script type="text/javascript">

  ...
  var connection = new signalR.HubConnectionBuilder().withUrl("/chartHub").build();

  connection.on("UpdateChart", function (data) {
      updateScatterChart(data);
  });

  connection.start().then(function () {

  }).catch(function (err) {
      return console.error(err.toString());
  });
</script>
```

  [1]: https://learn.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-7.0&viewFallbackFrom=aspnetcore-2.2&WT.mc_id=dotnet-35129-website&tabs=visual-studio
  [2]: https://learn.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-7.0&WT.mc_id=dotnet-35129-website&tabs=visual-studio#create-a-signalr-hub
  [3]: https://learn.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-7.0&tabs=visual-studio#configure-signalr
  [4]: https://learn.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-7.0&WT.mc_id=dotnet-35129-website&tabs=visual-studio#add-signalr-client-code
