using Humanizer;

namespace eb17953u202421866.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     String helpers used by <see cref="ModelBuilderExtensions" /> to build database object names.
/// </summary>
/// <remarks>
///     Author: __YOUR_NAME__
/// </remarks>
public static class StringExtensions
{
    /// <summary>Converts PascalCase/camelCase text to snake_case.</summary>
    public static string ToSnakeCase(this string text)
    {
        return new string(Convert(text.GetEnumerator()).ToArray());

        static IEnumerable<char> Convert(CharEnumerator e)
        {
            if (!e.MoveNext()) yield break;

            yield return char.ToLower(e.Current);

            while (e.MoveNext())
                if (char.IsUpper(e.Current))
                {
                    yield return '_';
                    yield return char.ToLower(e.Current);
                }
                else
                {
                    yield return e.Current;
                }
        }
    }

    /// <summary>Pluralizes a word (e.g. "space" -&gt; "spaces"), used for table names.</summary>
    public static string ToPlural(this string text)
    {
        return text.Pluralize(false);
    }
}
