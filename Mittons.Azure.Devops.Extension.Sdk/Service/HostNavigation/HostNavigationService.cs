using Mittons.Azure.Devops.Extension.Sdk.Service.Attributes;

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
    [ProxyFunction("getHash")]
    public Task<string> GetHashAsync();

    /// <summary>
    /// Gets the set of navigation elements (like hubs and hub groups) selected on the current page.
    /// </summary>
    [ProxyFunction("getPageNavigationElements")]
    public Task<INavigationElement> GetPageNavigationElementsAsync();

    /// <summary>
    /// Gets information about the route that was matched for the current page
    /// </summary>
    [ProxyFunction("getPageRoute")]
    public Task<IPageRoute> GetPageRouteAsync();

    /// <summary>
    /// Gets the current set of query parameters in the host page's URL.
    /// </summary>
    [ProxyFunction("getQueryParams")]
    public Task<Dictionary<string, string>> GetQueryParamsAsync();

    /// <summary>
    /// Navigate the parent page to the specified url
    /// @param url Url to navigate to
    /// </summary>
    [ProxyFunction("navigate")]
    public Task NavigateAsync(string url);

    /// <summary>
    /// Add a callback to be invoked each time the hash navigation has changed
    /// @param callback Method invoked on each navigation hash change
    /// </summary>
    [ProxyFunction("onHashChanged")]
    public Task OnHashChangedAsync(Action<string> callback);

    /// <summary>
    /// Open a new window to the specified url
    /// @param url Url of the new window
    /// @param features Comma-separated list of features/specs sent as the 3rd parameter to window.open. For example: "height=400,width=400".
    /// </summary>
    [ProxyFunction("openNewWindow")]
    public Task OpenNewWindowAsync(string url, string features);

    /// <summary>
    /// Reloads the parent frame
    /// </summary>
    [ProxyFunction("reload")]
    public Task ReloadAsync();

    /// <summary>
    /// Replace existing hash with the provided hash from the hosted content.
    /// </summary>
    [ProxyFunction("replaceHash")]
    public Task ReplaceHashAsync(string hash);

    /// <summary>
    /// Update the host document's title (appears as the browser tab title).
    /// @param title The new title of the window
    /// </summary>
    [ProxyFunction("setDocumentTitle")]
    public Task SetDocumentTitleAsync(string title);

    /// <summary>
    /// Sets the provided hash from the hosted content.
    /// </summary>
    [ProxyFunction("setHash")]
    public Task SetHashAsync(string hash);

    /// <summary>
    /// Sets one or more query parameters on the host page
    /// @param parameters Dictionary of query string parameters to add, update, or remove (pass an empty value to remove)
    /// </summary>
    [ProxyFunction("setQueryParams")]
    public Task SetQueryParametersAsync(Dictionary<string, string> parameters);
}
