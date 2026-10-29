namespace ConfigWorkbook.Sheets;

internal sealed class PackagingSheet(SqlRunner sql, string itemTypeId) : Sheet(sql, itemTypeId)
{
    public override string Name => "Packaging";

    public override void Write(SheetWriter writer)
    {
        writer.Title("Package Name");
        writer.Text(string.Join(", ", ByItemType("PackageName").Select(r => r["package_name"]).Distinct()));
    }
}
