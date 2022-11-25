using Mittons.Azure.Devops.Extension.Sdk.Attributes;

namespace Mittons.Azure.Devops.Extension.Sdk.Service.HostNavigation;

public interface INavigationElement { }

public interface IPageRoute { }

/// <summary>
/// Service for interacting with the host window's navigation (URLs, new windows, etc.)
/// </summary>
[GenerateService("ms.vss-features.host-navigation-service")]
public interface IHostNavigationService
{
    /// <summary>
    /// Gets the current hash.
    /// </summary>
    [RemoteProxyFunction("getHash")]
    public Task<string> GetHashAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the set of navigation elements (like hubs and hub groups) selected on the current page.
    /// </summary>
    [RemoteProxyFunction("getPageNavigationElements")]
    public Task<INavigationElement> GetPageNavigationElementsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about the route that was matched for the current page
    /// </summary>
    [RemoteProxyFunction("getPageRoute")]
    public Task<IPageRoute> GetPageRouteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current set of query parameters in the host page's URL.
    /// </summary>
    [RemoteProxyFunction("getQueryParams")]
    public Task<Dictionary<string, string>> GetQueryParamsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Navigate the parent page to the specified url
    /// @param url Url to navigate to
    /// </summary>
    [RemoteProxyFunction("navigate")]
    public Task NavigateAsync(string url, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add a callback to be invoked each time the hash navigation has changed
    /// @param callback Method invoked on each navigation hash change
    /// </summary>
    [RemoteProxyFunction("onHashChanged")]
    public Task OnHashChangedAsync(Action<string> callback, CancellationToken cancellationToken = default);

    /// <summary>
    /// Open a new window to the specified url
    /// @param url Url of the new window
    /// @param features Comma-separated list of features/specs sent as the 3rd parameter to window.open. For example: "height=400,width=400".
    /// </summary>
    [RemoteProxyFunction("openNewWindow")]
    public Task OpenNewWindowAsync(string url, string features, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reloads the parent frame
    /// </summary>
    [RemoteProxyFunction("reload")]
    public Task ReloadAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Replace existing hash with the provided hash from the hosted content.
    /// </summary>
    [RemoteProxyFunction("replaceHash")]
    public Task ReplaceHashAsync(string hash, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update the host document's title (appears as the browser tab title).
    /// @param title The new title of the window
    /// </summary>
    [RemoteProxyFunction("setDocumentTitle")]
    public Task SetDocumentTitleAsync(string title, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the provided hash from the hosted content.
    /// </summary>
    [RemoteProxyFunction("setHash")]
    public Task SetHashAsync(string hash, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets one or more query parameters on the host page
    /// @param parameters Dictionary of query string parameters to add, update, or remove (pass an empty value to remove)
    /// </summary>
    [RemoteProxyFunction("setQueryParams")]
    public Task SetQueryParametersAsync(Dictionary<string, string> parameters, CancellationToken cancellationToken = default);
}
