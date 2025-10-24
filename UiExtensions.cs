#nullable enable
using System.Windows.Controls;

namespace IdConverter;

public static class UiExtensions
{
    public static void Clear(this TextBlock? textBlock)
    {
        if (textBlock != null) textBlock.Text = "";
    }
}
