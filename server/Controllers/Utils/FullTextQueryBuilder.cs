namespace pbj.Utils
{
    public static class FullTextQueryBuilder
{
    // Returns: (booleanQuery, plain, tagExact)
    public static (string booleanQuery, string plain, string tagExact) Build(string input)
    {
        input ??= string.Empty;
        var plain = input.Trim();

        var tokens = new List<string>();
        var inQuote = false;
        var sb = new System.Text.StringBuilder();

        foreach (var ch in input)
        {
            if (ch == '"') { inQuote = !inQuote; sb.Append(ch); }
            else if (char.IsWhiteSpace(ch) && !inQuote)
            {
                if (sb.Length > 0) { tokens.Add(sb.ToString()); sb.Clear(); }
            }
            else sb.Append(ch);
        }
        if (sb.Length > 0) tokens.Add(sb.ToString());

        var parts = new List<string>();
        string tagExact = null;

        foreach (var raw in tokens)
        {
            var t = raw.Trim();
            if (string.IsNullOrEmpty(t)) continue;

            // keep quoted phrase and require it
            if (t.StartsWith("\"") && t.EndsWith("\"") && t.Length > 2)
            {
                parts.Add("+" + t);
                continue;
            }

            // simple clean; skip super-short tokens
            var cleaned = new string(t.Where(ch => char.IsLetterOrDigit(ch) || ch == '_' || ch == '-').ToArray());
            if (cleaned.Length >= 3)
            {
                parts.Add("+" + cleaned + "*");       // prefix match
                tagExact ??= cleaned.ToLowerInvariant(); // maybe match an exact CSV tag
            }
        }

        var booleanQuery = parts.Count > 0 ? string.Join(" ", parts) : "";
        return (booleanQuery, plain, tagExact);
    }
}

}
