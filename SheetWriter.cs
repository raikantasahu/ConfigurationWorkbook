using System.Globalization;
using ClosedXML.Excel;
using ConfigWorkbook.Sheets;

namespace ConfigWorkbook;

/// <summary>Writes titles and tables down a worksheet, one row after the other.</summary>
internal sealed class SheetWriter
{
    private const int MaxCellText = 32767;
    private const double MaxColumnWidth = 60;

    private readonly IXLWorksheet _sheet;
    private int _row = 1;

    public SheetWriter(IXLWorksheet sheet) => _sheet = sheet;

    /// <summary>A bold line of text in column A. A large title starts a main section.</summary>
    public void Title(string text, bool large = false)
    {
        var cell = _sheet.Cell(_row++, 1);
        cell.Value = text;
        cell.Style.Font.Bold = true;
        if (large)
        {
            cell.Style.Font.FontSize = 14;
        }
    }

    public void Text(string text) => _sheet.Cell(_row++, 1).Value = text;

    public void Blank() => _row++;

    /// <summary>
    /// A bold header row, then one row for each result row. With <paramref name="whenEmpty"/>,
    /// a table with no rows gets that text in its first column.
    /// </summary>
    public void Table(IReadOnlyList<Column> columns, IEnumerable<Row> rows, int firstColumn = 1, string? whenEmpty = null)
    {
        for (var i = 0; i < columns.Count; i++)
        {
            var cell = _sheet.Cell(_row, firstColumn + i);
            cell.Value = columns[i].Header;
            cell.Style.Font.Bold = true;
        }

        _row++;
        var count = 0;
        foreach (var row in rows)
        {
            for (var i = 0; i < columns.Count; i++)
            {
                SetValue(_sheet.Cell(_row, firstColumn + i), row[columns[i].Key], columns[i].IsNumber(row));
            }

            _row++;
            count++;
        }

        if (count == 0 && whenEmpty is not null)
        {
            _sheet.Cell(_row++, firstColumn).Value = whenEmpty;
        }
    }

    /// <summary>Sizes the columns to their content, up to a maximum width.</summary>
    public void Finish()
    {
        foreach (var column in _sheet.ColumnsUsed())
        {
            column.AdjustToContents();
            if (column.Width > MaxColumnWidth)
            {
                column.Width = MaxColumnWidth;
            }
        }
    }

    private static void SetValue(IXLCell cell, string value, bool number)
    {
        if (value.Length == 0)
        {
            return;
        }

        if (number && double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var d))
        {
            cell.Value = d;
        }
        else
        {
            cell.Value = value.Length > MaxCellText ? value[..MaxCellText] : value;
        }
    }
}
