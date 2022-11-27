using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Mittons.Azure.Devops.Extension.Ui.Drawing;

namespace Mittons.Azure.Devops.Extension.Ui;

public partial class Coin
{
    [Parameter]
    public EventCallback<MouseEventArgs> OnClickCallback { get; set; }

    [Parameter]
    public string? AriaLabel { get; set; }

    [Parameter]
    public bool IsTabStop { get; set; }

    [Parameter]
    public string? DisplayName { get; set; }

    [Parameter]
    public string? ImageUrl { get; set; }

    [Parameter]
    public string? ImageAltText { get; set; }

    [Parameter]
    public RenderFragment? TooltipContent { get; set; }

    private string? ClickClass => OnClickCallback.HasDelegate ? "cursor-pointer" : default;

    private string? ImageClass => _imageLoaded ? default : "pending-load-image";

    private string? Role => OnClickCallback.HasDelegate ? "button" : default;

    private int? TabIndex => IsTabStop ? 0 : default;

    private bool _showTooltip = false;

    private string BackgroundColorStyle
    {
        get
        {
            var color = ColorFactory.FromName(DisplayName);

            return $"rgb({color.R}, {color.G}, {color.B})";
        }
    }

    private Task OnKeyDownCallback(KeyboardEventArgs eventArgs)
    {
        var validButtons = new[] { "Enter", "NumpadEnter", "Space" };

        return validButtons.Contains(eventArgs.Code) ? OnClickCallback.InvokeAsync() : Task.CompletedTask;
    }

    private Task OnMouseOverCallback(MouseEventArgs eventArgs)
    {
        _showTooltip = true;

        return Task.CompletedTask;
    }

    private Task OnMouseOutCallback(MouseEventArgs eventArgs)
    {
        _showTooltip = false;

        return Task.CompletedTask;
    }

    private bool _imageErrored = false;

    private bool _imageLoaded = false;

    private string GetInitials()
    {
        var segments = DisplayName?.Split(" ", 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? Array.Empty<string>();

        switch (segments.Length)
        {
            case 1:
                return segments[0][0].ToString();
            case 2:
                return $"{segments[0][0]}{segments[1][0]}";
            default:
                return string.Empty;
        }
    }
}
