namespace ConfigWorkbook.Sheets;

internal sealed class ServerEventsSheet(SqlRunner sql, string itemTypeId) : Sheet(sql, itemTypeId)
{
    private static readonly Column[] Columns =
    [
        TextColumn("name", "Name"),
        TextColumn("method_type", "Method Type"),
        NumberColumn("version", "Version"),
        TextColumn("execution_allowed_to", "Execution Allowed To"),
        TextColumn("template", "Template"),
        TextColumn("comments", "Comments"),
        TextColumn("server_event", "Server Event"),
        NumberColumn("sort_order", "Sort Order"),
        TextColumn("event_version", "Event Version"),
    ];

    public override string Name => "Server Events";

    public override void Write(SheetWriter writer)
    {
        writer.Title("Server Events");
        writer.Table(Columns, ByItemType("ServerEvents"));
    }
}
