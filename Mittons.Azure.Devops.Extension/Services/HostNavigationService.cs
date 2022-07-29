namespace Mittons.Azure.Devops.Extension.Service;

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
    public Task<string> GetHash();

    /// <summary>
    /// Gets the set of navigation elements (like hubs and hub groups) selected on the current page.
    /// </summary>
    [ProxyFunction("getPageNavigationElements")]
    public Task<INavigationElement> GetPageNavigationElements();

    /// <summary>
    /// Gets information about the route that was matched for the current page
    /// </summary>
    [ProxyFunction("getPageRoute")]
    public Task<IPageRoute> GetPageRoute();

    /// <summary>
    /// Gets the current set of query parameters in the host page's URL.
    /// </summary>
    [ProxyFunction("getQueryParams")]
    public Task<Dictionary<string, string>> GetQueryParams();

    /// <summary>
    /// Navigate the parent page to the specified url
    /// @param url Url to navigate to
    /// </summary>
    [ProxyFunction("navigate")]
    public Task Navigate(string url);

    /// <summary>
    /// Add a callback to be invoked each time the hash navigation has changed
    /// @param callback Method invoked on each navigation hash change
    /// </summary>
    [ProxyFunction("onHashChanged")]
    public Task OnHashChanged(Action<string> callback);

    /// <summary>
    /// Open a new window to the specified url
    /// @param url Url of the new window
    /// @param features Comma-separated list of features/specs sent as the 3rd parameter to window.open. For example: "height=400,width=400".
    /// </summary>
    [ProxyFunction("openNewWindow")]
    public Task OpenNewWindow(string url, string features);

    /// <summary>
    /// Reloads the parent frame
    /// </summary>
    [ProxyFunction("reload")]
    public Task Reload();

    /// <summary>
    /// Replace existing hash with the provided hash from the hosted content.
    /// </summary>
    [ProxyFunction("replaceHash")]
    public Task ReplaceHash(string hash);

    /// <summary>
    /// Update the host document's title (appears as the browser tab title).
    /// @param title The new title of the window
    /// </summary>
    [ProxyFunction("setDocumentTitle")]
    public Task SetDocumentTitle(string title);

    /// <summary>
    /// Sets the provided hash from the hosted content.
    /// </summary>
    [ProxyFunction("setHash")]
    public Task SetHash(string hash);

    /// <summary>
    /// Sets one or more query parameters on the host page
    /// @param parameters Dictionary of query string parameters to add, update, or remove (pass an empty value to remove)
    /// </summary>
    [ProxyFunction("setQueryParams")]
    public Task SetQueryParameters(Dictionary<string, string> parameters);
}
