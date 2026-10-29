using ClosedXML.Excel;
using ConfigWorkbook.Sheets;

namespace ConfigWorkbook;

/// <summary>
/// Builds the configuration workbook of one ItemType. The sheets and their sections follow the
/// sample "CAD Configuration Workbook.xlsx". SheetFactory gives the sheets, in workbook order.
/// </summary>
internal sealed class WorkbookBuilder
{
    private readonly SqlRunner _sql;
    private readonly string _itemTypeId;

    public WorkbookBuilder(SqlRunner sql, string itemTypeId)
    {
        _sql = sql;
        _itemTypeId = itemTypeId;
    }

    public void Build(string path)
    {
        using var workbook = new XLWorkbook();
        foreach (var sheet in SheetFactory.CreateAll(_sql, _itemTypeId))
        {
            Console.WriteLine($"  {sheet.Name}");
            var writer = new SheetWriter(workbook.AddWorksheet(sheet.Name));
            sheet.Write(writer);
            writer.Finish();
        }

        workbook.SaveAs(path);
    }
}
