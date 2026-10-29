namespace ConfigWorkbook.Sheets;

internal sealed class CanAddSheet(SqlRunner sql, string itemTypeId) : Sheet(sql, itemTypeId)
{
    private static readonly Column[] Columns =
    [
        TextColumn("name", "Name"),
        NumberColumn("can_add", "Can Add"),
        TextColumn("class_path", "Class Path"),
    ];

    public override string Name => "Can Add";

    public override void Write(SheetWriter writer) => writer.Table(Columns, ByItemType("CanAdd"));
}
