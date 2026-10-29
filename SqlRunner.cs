using System.Text.RegularExpressions;
using System.Xml;
using Aras.IOM;

namespace ConfigWorkbook;

/// <summary>One result row. A column that is missing from the response (a SQL NULL) reads as "".</summary>
internal sealed class Row
{
    private readonly Dictionary<string, string> _values;

    public Row(Dictionary<string, string> values) => _values = values;

    public string this[string column] => _values.TryGetValue(column, out var value) ? value : "";

    /// <summary>A copy of this row with one column set to a new value.</summary>
    public Row With(string column, string value) =>
        new(new Dictionary<string, string>(_values, _values.Comparer) { [column] = value });
}

/// <summary>
/// Runs the queries in the Sql folder (embedded in the program) on the Innovator server through
/// ApplySQL, and reads the rows of the response.
/// </summary>
internal sealed partial class SqlRunner
{
    private readonly Innovator _innovator;

    public SqlRunner(Innovator innovator) => _innovator = innovator;

    [GeneratedRegex("^[0-9A-F]{32}$", RegexOptions.IgnoreCase)]
    private static partial Regex IdPattern();

    [GeneratedRegex(@"^\s*--.*$", RegexOptions.Multiline)]
    private static partial Regex CommentLine();

    /// <summary>
    /// Runs Sql/&lt;query&gt;.sql. Each parameter replaces the text @Name in the query with the
    /// quoted id. ApplySQL has no bind parameters, so the id is checked to be a 32-character
    /// hex Aras id before the text replacement.
    /// </summary>
    public IReadOnlyList<Row> Run(string query, params (string Name, string Id)[] parameters)
    {
        var sql = Load(query);
        foreach (var (name, id) in parameters)
        {
            if (!IdPattern().IsMatch(id))
            {
                throw new ArgumentException($"{query}.sql: '{id}' is not an Aras id.", nameof(parameters));
            }

            sql = Regex.Replace(sql, $@"@{name}\b", $"'{id}'");
        }

        var result = _innovator.applySQL(sql);
        if (result.isEmpty())
        {
            return [];
        }

        if (result.isError())
        {
            throw new InvalidOperationException($"{query}.sql failed: {result.getErrorString()}");
        }

        return ReadRows(result.dom);
    }

    private static string Load(string query)
    {
        using var stream = typeof(SqlRunner).Assembly.GetManifestResourceStream($"Sql.{query}.sql")
            ?? throw new InvalidOperationException($"The query Sql/{query}.sql is not in the program.");
        using var reader = new StreamReader(stream);

        // Remove the "--" comment lines. ApplySQL joins the lines of the query, so a line
        // comment would hide all of the SQL after it, and the server returns no rows.
        return CommentLine().Replace(reader.ReadToEnd(), "");
    }

    /// <summary>
    /// Each row is an &lt;Item&gt; element, and each column is a child element of it. Column names
    /// are compared without case, because the server can change their case.
    /// </summary>
    private static List<Row> ReadRows(XmlDocument dom)
    {
        var rows = new List<Row>();
        foreach (XmlElement item in dom.GetElementsByTagName("Item"))
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (XmlNode child in item.ChildNodes)
            {
                if (child is XmlElement column)
                {
                    values[XmlConvert.DecodeName(column.LocalName)] = column.InnerText;
                }
            }

            rows.Add(new Row(values));
        }

        return rows;
    }
}
