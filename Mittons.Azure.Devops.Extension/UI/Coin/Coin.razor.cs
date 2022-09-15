using System.Drawing;

namespace Mittons.Azure.Devops.Extension.UI.Coin;

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