using System;

namespace IdConverter;

public static class GuidExtensions
{
    public static string Short(this Guid guid, bool toLower = true) => 
        toLower 
        ? guid.ToString("N").Substring(0, 8)
        : guid.ToString("N").Substring(0, 8).ToUpper();

    public static string Medium(this Guid guid, bool toLower = true) => 
        toLower 
        ? guid.ToString("N")
        : guid.ToString("N").ToUpper();

    public static string Large(this Guid guid, bool toLower = true) => 
        toLower 
        ? guid.ToString("D")
        : guid.ToString("D").ToUpper();

    public static string Full(this Guid guid, bool toLower = true) => 
        toLower 
            ? $"{{{guid.ToString("D")}}}"
            : $"{{{guid.ToString("D").ToUpper()}}}";
}
