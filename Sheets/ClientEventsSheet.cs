namespace ConfigWorkbook.Sheets;

internal sealed class ClientEventsSheet(SqlRunner sql, string itemTypeId) : Sheet(sql, itemTypeId)
{
    private static readonly Column[] Columns =
    [
        TextColumn("name", "Name"),
        TextColumn("method_type", "Method Type"),
        NumberColumn("version", "Version"),
        TextColumn("execution_allowed_to", "execution_allowed_to"),
        TextColumn("template", "Template"),
        TextColumn("comments", "Comments"),
        TextColumn("event", "Event"),
        NumberColumn("sort_order", "sort-order"),
    ];

    public override string Name => "Client Events";

    public override void Write(SheetWriter writer) => writer.Table(Columns, ByItemType("ClientEvents"));
}
