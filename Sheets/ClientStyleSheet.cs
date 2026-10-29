namespace ConfigWorkbook.Sheets;

internal sealed class ClientStyleSheet(SqlRunner sql, string itemTypeId) : Sheet(sql, itemTypeId)
{
    private static readonly Column[] PresentationConfigurationColumns =
    [
        TextColumn("name", "Name"),
        TextColumn("color", "Color"),
        TextColumn("client", "Client"),
    ];

    private static readonly Column[] CommandBarSectionColumns =
    [
        TextColumn("itemtype_name", "ItemType Name"),
        TextColumn("presentation_configuration", "Presentation Configuration Name"),
        TextColumn("name", "Name"),
        TextColumn("builder_method", "Builder Method"),
        TextColumn("label", "Label"),
        TextColumn("description", "Description"),
        TextColumn("additional_data", "Additional Data"),
        TextColumn("location", "Location"),
        NumberColumn("sort_order", "Sort Order"),
        TextColumn("for_identity", "For Identity"),
        TextColumn("for_classification", "For Classification"),
    ];

    private static readonly Column[] CommandBarItemColumns =
    [
        TextColumn("itemtype_name", "ItemType Name"),
        TextColumn("presentation_configuration", "Presentation Configuration Name"),
        TextColumn("command_bar_section", "Command Bar Section Name"),
        TextColumn("itemtype", "ItemType"),
        TextColumn("name", "Name"),
        TextColumn("additional_data", "Additional Data"),
        TextColumn("init_method", "Init Method"),
        NumberColumn("sort_order", "Sort Order"),
        TextColumn("action", "Action"),
        TextColumn("alternate", "Alternate"),
        TextColumn("for_identity", "For Identity"),
        TextColumn("for_classification", "For Classification"),
    ];

    public override string Name => "Client Style (CUI)";

    public override void Write(SheetWriter writer)
    {
        writer.Title("Presentation Configurations");
        writer.Table(PresentationConfigurationColumns, ByItemType("PresentationConfigurations"));
        writer.Blank();
        writer.Title("Command Bar Sections");
        writer.Table(CommandBarSectionColumns, ByItemType("CommandBarSections"));
        writer.Blank();
        writer.Title("Command Bar Items");
        writer.Table(CommandBarItemColumns, ByItemType("CommandBarItems"));
    }
}
