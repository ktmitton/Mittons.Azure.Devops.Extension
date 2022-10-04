using System.Drawing;
using Microsoft.AspNetCore.Components.Web;

namespace Mittons.Azure.Devops.Extension.UI.Tooltip;

public record TooltipProperties
{
    public bool AddAriaDescribedBy { get; }
    public Point AnchorOffset { get; }
    public Point AnchorOrigin { get; }
    public TimeSpan Delay { get; }
    public bool IsDisabled { get; }
    public string Id { get; }
    public bool ShowOnFocus { get; }
    public string Text { get; }
    public Point TooltipOrigin { get; }

    public TooltipProperties(
        bool AddAriaDescribedBy,
        Point AnchorOffset,
        Point AnchorOrigin,
        string? ClassName,
        TimeSpan Delay,
        bool IsDisabled,
        string Id,
        bool ShowOnFocus,
        string Text,
        Point TooltipOrigin
    )
    {
        this.AddAriaDescribedBy = AddAriaDescribedBy;
        this.AnchorOffset = AnchorOffset;
        this.AnchorOrigin = AnchorOrigin;
        this.Delay = Delay;
        this.IsDisabled = IsDisabled;
        this.Id = Id;
        this.ShowOnFocus = ShowOnFocus;
        this.Text = Text;
        this.TooltipOrigin = TooltipOrigin;
    }
}