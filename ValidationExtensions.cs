using static System.Text.RegularExpressions.Regex;

namespace IdConverter;

public static class ValidationExtensions
{
    public static bool IsEmpty(this string value) => string.IsNullOrWhiteSpace(value);

    // Check if it's a valid 8-character or 32-character hex string
    public static bool IsValidGuid(this string value) =>
        IsMatch(value, @"^[0-9a-fA-F]{8}$|^[0-9a-fA-F]{32}$");
}
