namespace ConfigWorkbook.Sheets;

internal sealed class ReportsSheet(SqlRunner sql, string itemTypeId) : Sheet(sql, itemTypeId)
{
    private static readonly Column[] Columns =
    [
        TextColumn("name", "Name"),
        TextColumn("description", "Description"),
        TextColumn("type", "Type"),
        TextColumn("target", "Target"),
        TextColumn("method", "Method"),
        TextColumn("location", "Location"),
    ];

    public override string Name => "Reports";

    public override void Write(SheetWriter writer) => writer.Table(Columns, ByItemType("Reports"));
}
