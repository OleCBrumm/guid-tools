using System;

namespace SitecoreIdConverter;

public static class GuidExtensions
{
    public static string Short(this Guid guid) => guid.ToString("N").Substring(0, 8);
    public static string Medium(this Guid guid) => guid.ToString("N");
    public static string Large(this Guid guid) => guid.ToString();
    public static string Full(this Guid guid) => $"{{{guid}}}";
}
