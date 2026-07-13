using System.Text.RegularExpressions;

namespace eb17953u202421866.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;

/// <summary>
///     String helpers for building kebab-case URL segments.
/// </summary>
/// <remarks>
///     Author: __YOUR_NAME__
/// </remarks>
public static partial class StringExtensions
{
    /// <summary>Converts PascalCase/camelCase text to kebab-case.</summary>
    public static string ToKebabCase(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;
        return KebabCaseRegex().Replace(text, "-$1")
            .Trim()
            .ToLower();
    }

    [GeneratedRegex("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", RegexOptions.Compiled)]
    private static partial Regex KebabCaseRegex();
}
