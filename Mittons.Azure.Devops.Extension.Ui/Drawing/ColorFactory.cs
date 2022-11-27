using System.Drawing;

namespace Mittons.Azure.Devops.Extension.Ui.Drawing;

public static class ColorFactory
{
    public static Color FromName(string? name)
    {
        var defaultColor = Color.FromArgb(79, 107, 237);

        var palette = new[]
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

        if (string.IsNullOrWhiteSpace(name))
        {
            return defaultColor;
        }

        var hashcode = 0;

        for (var i = 0; i < name.Length; ++i)
        {
            var shift = i % 8;

            hashcode ^= (name[i] << shift) + (name[i] >> (8 - shift));
        }

        return palette[hashcode % palette.Length];
    }
}