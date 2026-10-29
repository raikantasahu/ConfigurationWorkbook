namespace ConfigWorkbook.Sheets;

internal sealed class ActionsSheet(SqlRunner sql, string itemTypeId) : Sheet(sql, itemTypeId)
{
    private static readonly Column[] Columns =
    [
        TextColumn("action_name", "Action Name"),
        TextColumn("can_execute", "Method To Check Can Execute"),
        TextColumn("item_query", "Item Query"),
        TextColumn("label", "Label"),
        TextColumn("location", "Location"),
        TextColumn("method", "Method"),
        TextColumn("on_complete", "Method on Complete"),
        TextColumn("target", "Target"),
        TextColumn("type", "Type"),
    ];

    public override string Name => "Actions";

    public override void Write(SheetWriter writer) => writer.Table(Columns, ByItemType("Actions"));
}
