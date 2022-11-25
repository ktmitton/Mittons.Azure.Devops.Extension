using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Mittons.Azure.Devops.Extension.Sdk;

public partial class AzureDevopsView : IComponent, IHandleAfterRender
{
    private RenderHandle _renderHandle;

    private bool _isInitialized;

    private ILogger<AzureDevopsView>? _logger;

    [Inject]
    private ISdk? Sdk { get; set; }

    [Inject]
    private ILoggerFactory? LoggerFactory { get; set; }

    [Parameter]
    [EditorRequired]
    public RenderFragment? Initialized { get; set; }

    [Parameter]
    [EditorRequired]
    public RenderFragment? Uninitialized { get; set; }

    public void Attach(RenderHandle renderHandle)
    {
        _logger = LoggerFactory?.CreateLogger<AzureDevopsView>();
        _renderHandle = renderHandle;
    }

    public async Task OnAfterRenderAsync()
    {
        if (!_isInitialized && Sdk is not null)
        {
            await Sdk.InitializeAsync();

            _isInitialized = true;

            Refresh();
        }
    }

    private void Refresh()
    {
        if (_isInitialized)
        {
            if (_logger is not null)
            {
                Log.DisplayingInitialized(_logger);
            }

            if (Initialized is not null)
            {
                _renderHandle.Render(Initialized);
            }
        }
        else
        {
            if (_logger is not null)
            {
                Log.DisplayingUninitialized(_logger);
            }

            if (Uninitialized is not null)
            {
                _renderHandle.Render(Uninitialized);
            }
        }
    }

    public Task SetParametersAsync(ParameterView parameters)
    {
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
