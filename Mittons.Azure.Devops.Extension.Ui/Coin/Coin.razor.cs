using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Mittons.Azure.Devops.Extension.Ui.Tooltip;

namespace Mittons.Azure.Devops.Extension.Ui.Coin;

public enum CoinSize
{
    ExtraSmall = 16,
    Small = 20,
    Medium = 24,
    Large = 32,
    ExtraLarge = 40,
    ExtraExtraLarge = 72
}

public partial class Coin
{
    [Parameter]
    public EventCallback<MouseEventArgs> OnClickCallback { get; set; }

    [Parameter]
    public string? AriaLabel { get; set; }

    [Parameter]
    public string? ClassName { get; set; }

    [Parameter]
    public bool DataIsFocusable { get; set; }

    [Parameter]
    public bool TabStop { get; set; }

    [Parameter]
    public CoinSize Size { get; set; }

    [Parameter]
    public string? DisplayName { get; set; }

    [Parameter]
    public string? ImageUrl { get; set; }

    [Parameter]
    public string? ImageAltText { get; set; }

    [Parameter]
    public TooltipProperties? TooltipProperties { get; set; }

    private string SizeClass => $"size{(int)Size}";

    private string? ClickClass => OnClickCallback.HasDelegate ? "cursor-pointer" : default(string);

    private string? ImageClass => _imageLoaded ? default(string) : "pending-load-image";

    private string? Role => OnClickCallback.HasDelegate ? "button" : default(string);

    private string BackgroundColorStyle
    {
        get
        {
            var color = PickColor();

            return $"background: rgb({color.R}, {color.G}, {color.B});";
        }
    }

    private Task OnKeyDownCallback(KeyboardEventArgs eventArgs)
    {
        var validButtons = new[] { "Enter", "NumpadEnter", "Space" };

        if (validButtons.Contains(eventArgs.Code))
        {
            return OnClickCallback.InvokeAsync();
        }

        return Task.CompletedTask;
    }

    private Task OnMouseOverCallback(MouseEventArgs eventArgs)
    {
        _showTooltip = true;
        System.Console.WriteLine($"Client: {eventArgs.ClientX} {eventArgs.ClientY}");
        System.Console.WriteLine($"Offset: {eventArgs.OffsetX} {eventArgs.OffsetY}");
        System.Console.WriteLine($"Page: {eventArgs.PageX} {eventArgs.PageY}");
        System.Console.WriteLine($"Screen: {eventArgs.ScreenX} {eventArgs.ScreenY}");
        System.Console.WriteLine($"");

        return Task.CompletedTask;
    }

    private Task OnMouseOutCallback(MouseEventArgs eventArgs)
    {
        System.Console.WriteLine("OnMouseOut");

        return Task.CompletedTask;
    }

    private bool _imageErrored = false;

    private bool _imageLoaded = false;

    private bool _showTooltip = false;

    private static Color DefaultColor = Color.FromArgb(79, 107, 237);

    private static Color[] ColorPalette = new[]
    {
        Color.FromArgb(117, 11, 28),
        Color.FromArgb(164, 38, 44),
        Color.FromArgb(209, 52, 56),
        Color.FromArgb(202, 80, 16),
        Color.FromArgb(73, 130, 5),
        Color.FromArgb(11, 106, 11),
        Color.FromArgb(3, 131, 135),
        Color.FromArgb(0, 91, 112),
        Color.FromArgb(0, 120, 212),
        Color.FromArgb(79, 107, 237),
        Color.FromArgb(92, 46, 145),
        Color.FromArgb(135, 100, 184),
        Color.FromArgb(136, 23, 152),
        Color.FromArgb(194, 57, 179),
        Color.FromArgb(227, 0, 140),
        Color.FromArgb(152, 111, 11),
        Color.FromArgb(142, 86, 46),
        Color.FromArgb(122, 117, 116),
        Color.FromArgb(105, 121, 126)
    };

    public Color PickColor()
    {
        if (string.IsNullOrWhiteSpace(DisplayName))
        {
            return DefaultColor;
        }

        var hashcode = 0;

        for (var i = 0; i < DisplayName.Length; ++i)
        {
            var shift = i % 8;

            hashcode ^= (DisplayName[i] << shift) + (DisplayName[i] >> (8 - shift));
        }

        return ColorPalette[hashcode % ColorPalette.Length];
    }

    private string GetInitials()
    {
        var segments = DisplayName?.Split(" ", 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ??
        Array.Empty<string>();

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
