using Mittons.Azure.Devops.Extension.Sdk.Attributes;

namespace Mittons.Azure.Devops.Extension.Sdk.Service.GlobalMessages;

/// <summary>
/// Service for showing global message banners at the top of the page
/// </summary>
[GenerateService("ms.vss-tfs-web.tfs-global-messages-service")]
public interface IGlobalMessagesService
{
    /// <summary>
    /// Adds a new message banner to the displayed banners
    /// @param banner - The message banner to display
    /// </summary>
    [RemoteProxyFunction("addBanner")]
    Task AddBannerAsync(Banner banner);

    /// <summary>
    /// Adds a new dialog to the displayed dialogs. CornerDialog or CustomDialog can be added
    /// @param dialog - The dialog to display
    /// </summary>
    [RemoteProxyFunction("addDialog")]
    Task AddDialogAsync(Dialog dialog);

    /// <summary>
    /// Displays or queues a Toast to display at the bottom of the page
    /// @param toast - The toast to display
    /// </summary>
    [RemoteProxyFunction("addToast")]
    Task AddToastAsync(Toast toast);

    /// <summary>
    /// Closes the currently active global message banner
    /// </summary>
    [RemoteProxyFunction("closeBanner")]
    Task CloseBannerAsync();

    /// <summary>
    /// Closes the currently active global dialog
    /// </summary>
    [RemoteProxyFunction("closeDialog")]
    Task CloseDialogAsync();
}
