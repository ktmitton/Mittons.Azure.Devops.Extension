using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Mittons.Azure.Devops.Extension.UI.Tooltip;

public partial class Tooltip
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public EventCallback<FocusEventArgs> OnFocusInCallback { get; set; }

    [Parameter]
    public EventCallback<FocusEventArgs> OnFocusOutCallback { get; set; }
}