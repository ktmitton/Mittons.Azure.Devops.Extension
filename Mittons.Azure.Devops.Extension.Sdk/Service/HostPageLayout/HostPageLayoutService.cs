using Mittons.Azure.Devops.Extension.Sdk.Attributes;

namespace Mittons.Azure.Devops.Extension.Sdk.Service.HostPageLayout;

public interface IDialogOptions<T> { }

public interface IMessageDialogOptions { }

public interface IPaneOptions<T> { }

/// <summary>
/// Service for interacting with the layout of the page: managing full-screen mode,
/// opening dialogs and panels
/// </summary>
[GenerateService("ms.vss-features.host-page-layout-service")]
public interface IHostPageLayoutService
{
    /// <summary>
    /// Gets whether the page is currently in full screen mode
    /// </summary>
    [RemoteProxyFunction("getFullScreenMode")]
    public Task<bool> GetFullScreenModeAsync();

    /// <summary>
    /// Open a dialog in the host frame, showing custom external content
    /// @param contentContributionId - Id of the dialog content contribution that specifies the content to display in the dialog.
    /// @param options - Dialog options
    /// </summary>
    [RemoteProxyFunction("openCustomDialog")]
    public Task OpenCustomDialogAsync<T>(string contentContributionId, IDialogOptions<T> options);

    /// <summary>
    /// Open a dialog in the host frame, showing the specified text message, an OK and optional Cancel button
    /// @param message - Dialog message text
    /// @param options - Dialog options
    /// </summary>
    [RemoteProxyFunction("openMessageDialog")]
    public Task OpenMessageDialogAsync(string message, IMessageDialogOptions? options);

    /// <summary>
    /// Open a panel in the host frame, showing custom external content
    /// @param contentContributionId - Id of the panel content contribution that specifies the content to display in the panel.
    /// @param options - Panel display options
    /// </summary>
    [RemoteProxyFunction("openPanel")]
    public Task OpenPanelAsync<T>(string contentContributionId, IPaneOptions<T> options);

    /// <summary>
    /// Enter or exit full screen mode
    /// @param fullScreenMode True to enter full-screen mode, false to exit.
    /// </summary>
    [RemoteProxyFunction("setFullScreenMode")]
    public Task SetFullScreenModeAsync(bool fullScreenMode);
}
