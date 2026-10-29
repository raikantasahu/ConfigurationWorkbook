namespace ConfigWorkbook.Sheets;

internal sealed class PropertiesSheet(SqlRunner sql, string itemTypeId) : Sheet(sql, itemTypeId)
{
    /// <summary>Data types whose Default Value is written as a number.</summary>
    private static readonly string[] NumberTypes = ["integer", "decimal", "float", "boolean"];

    /// <summary>The property columns. The Relationship Types sheet uses them too.</summary>
    public static readonly Column[] Columns =
    [
        TextColumn("property_name", "Property Name"),
        TextColumn("label", "Label"),
        TextColumn("data_type", "Data Type"),
        TextColumn("data_source", "Data Source"),
        NumberColumn("length", "Length"),
        NumberColumn("precision", "Precision"),
        NumberColumn("scale", "Scale"),
        NumberColumn("required", "Required"),
        NumberColumn("is_unique", "Unique"),
        NumberColumn("indexed", "Indexed"),
        NumberColumn("federated", "Federated"),
        NumberColumn("discoverable", "Discoverable"),
        NumberColumn("hidden", "Hidden on Main Search"),
        NumberColumn("hidden2", "Hidden on Relationship"),
        TextColumn("alignment", "Alignment"),
        NumberColumn("width", "Width"),
        NumberColumn("sort_order", "Sort Order"),
        NumberColumn("keyed_name_order", "Keyed Name Order"),
        NumberColumn("order_by", "Order By"),
        new("default_value", "Default Value",
            NumberWhen: r => NumberTypes.Contains(r["data_type"], StringComparer.OrdinalIgnoreCase)),
        TextColumn("default_search", "Default Search"),
        TextColumn("pattern", "Pattern"),
        TextColumn("foreign_property", "Foreign Property"),
        TextColumn("tooltip", "Tooltip"),
        TextColumn("help_text", "Help Text"),
        NumberColumn("track_history", "Track History"),
        TextColumn("behavior", "Behavior"),
        NumberColumn("min_range", "Min Range"),
        NumberColumn("max_range", "Max Range"),
        NumberColumn("range_inclusive", "Range Inclusive"),
        NumberColumn("read_only", "Read Only"),
    ];

    public override string Name => "Properties";

    public override void Write(SheetWriter writer) =>
        writer.Table([TextColumn("itemtype_name", "ItemType Name"), .. Columns], Sql.Run("Properties", ("SourceId", ItemTypeId)));
}
