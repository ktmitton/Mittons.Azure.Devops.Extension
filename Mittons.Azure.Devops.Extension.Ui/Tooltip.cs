using Microsoft.AspNetCore.Components;

namespace Mittons.Azure.Devops.Extension.Ui;

public partial class Tooltip
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool Show { get; set; }

    private string _fadeClass => Show ? "bolt-tooltip-fade-in" : "bolt-tooltip-fade-out";
}
