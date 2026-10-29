namespace ConfigWorkbook.Sheets;

/// <summary>
/// One column of a table: the query column it reads, its header text, and when its values are
/// written as number cells. A value that does not parse as a number stays text.
/// </summary>
internal sealed record Column(string Key, string Header, bool Number = false, Func<Row, bool>? NumberWhen = null)
{
    public bool IsNumber(Row row) => Number || (NumberWhen?.Invoke(row) ?? false);
}

/// <summary>
/// One worksheet of the configuration workbook. A subclass gives the sheet name, the columns
/// of its tables, and the code that writes its sections.
/// </summary>
internal abstract class Sheet(SqlRunner sql, string itemTypeId)
{
    protected SqlRunner Sql { get; } = sql;

    protected string ItemTypeId { get; } = itemTypeId;

    /// <summary>The worksheet name (the tab label).</summary>
    public abstract string Name { get; }

    public abstract void Write(SheetWriter writer);

    /// <summary>Runs a query whose only parameter is the ItemType id.</summary>
    protected IReadOnlyList<Row> ByItemType(string query) => Sql.Run(query, ("ItemTypeId", ItemTypeId));

    protected static Column TextColumn(string key, string header) => new(key, header);

    protected static Column NumberColumn(string key, string header) => new(key, header, Number: true);
}
