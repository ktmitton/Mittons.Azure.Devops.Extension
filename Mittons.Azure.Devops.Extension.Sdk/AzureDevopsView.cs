using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Mittons.Azure.Devops.Extension.Sdk;

public partial class AzureDevopsView : IComponent, IHandleAfterRender
{
    private RenderHandle _renderHandle;

    private bool _isInitialized;

    // private ILogger<AzureDevopsViews> _logger;

    // [Inject]
    // private ISdk Sdk { get; set; }

    // [Inject]
    // private ILoggerFactory LoggerFactory { get; set; }

    [Parameter]
    [EditorRequired]
    public RenderFragment Initialized { get; set; }

    [Parameter]
    [EditorRequired]
    public RenderFragment Uninitialized { get; set; }

    public void Attach(RenderHandle renderHandle)
    {
        System.Console.WriteLine("Starting Attach");
        //_logger = LoggerFactory.CreateLogger<AzureDevopsViews>();
        _renderHandle = renderHandle;
        System.Console.WriteLine("Ending Attach");
    }

    public async Task OnAfterRenderAsync()
    {
        System.Console.WriteLine("Starting OnAfterRenderAsync");
        if (!_isInitialized)
        {
            // await Sdk.InitializeAsync();

            _isInitialized = true;

            Refresh();
        }
        System.Console.WriteLine("Ending OnAfterRenderAsync");
    }

    private void Refresh()
    {
        System.Console.WriteLine("Starting Refresh");
        if (_isInitialized)
        {
            //Log.DisplayingInitialized(_logger);
            _renderHandle.Render(Initialized);
        }
        else
        {
            //Log.DisplayingUninitialized(_logger);
            _renderHandle.Render(Uninitialized);
        }
        System.Console.WriteLine("Ending Refresh");
    }

    public Task SetParametersAsync(ParameterView parameters)
    {
        System.Console.WriteLine("Starting SetParametersAsync");
        parameters.SetParameterProperties(this);

        if (Initialized == null)
        {
            throw new InvalidOperationException($"The {nameof(AzureDevopsView)} component requires a value for the parameter {nameof(Initialized)}.");
        }

        if (Uninitialized == null)
        {
            throw new InvalidOperationException($"The {nameof(AzureDevopsView)} component requires a value for the parameter {nameof(Uninitialized)}.");
        }

        Refresh();
        System.Console.WriteLine("Ending SetParametersAsync");

        return Task.CompletedTask;
    }

    private static partial class Log
    {
        [LoggerMessage(1, LogLevel.Debug, $"Displaying {nameof(Initialized)}", EventName = "DisplayingInitialized")]
        internal static partial void DisplayingInitialized(ILogger logger);

        [LoggerMessage(2, LogLevel.Debug, $"Displaying {nameof(Uninitialized)}", EventName = "DisplayingUninitialized")]
        internal static partial void DisplayingUninitialized(ILogger logger);
    }
}