using Mittons.Azure.Devops.Extension.Sdk.Attributes;
using Mittons.Azure.Devops.Extension.Sdk.Service.Attributes;

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
    [ProxyFunction("addBanner")]
    Task AddBannerAsync(Banner banner);

    /// <summary>
    /// Adds a new dialog to the displayed dialogs. CornerDialog or CustomDialog can be added
    /// @param dialog - The dialog to display
    /// </summary>
    [ProxyFunction("addDialog")]
    Task AddDialogAsync(Dialog dialog);

    /// <summary>
    /// Displays or queues a Toast to display at the bottom of the page
    /// @param toast - The toast to display
    /// </summary>
    [ProxyFunction("addToast")]
    Task AddToastAsync(Toast toast);

    /// <summary>
    /// Closes the currently active global message banner
    /// </summary>
    [ProxyFunction("closeBanner")]
    Task CloseBannerAsync();

    /// <summary>
    /// Closes the currently active global dialog
    /// </summary>
    [ProxyFunction("closeDialog")]
    Task CloseDialogAsync();
}
