namespace ConfigWorkbook.Sheets;

internal sealed class RelationshipTypesSheet(SqlRunner sql, string itemTypeId) : Sheet(sql, itemTypeId)
{
    private static readonly Column[] Columns =
    [
        NumberColumn("tab_order", "Tab Order"),
        TextColumn("name", "NAME"),
        TextColumn("tab_label", "Tab Label"),
        TextColumn("related_item", "Related Item"),
        NumberColumn("auto_search", "Auto Search"),
        NumberColumn("copy_permissions", "Copy Permissions on Paste"),
        NumberColumn("create_related", "Create Related"),
        NumberColumn("default_page_size", "Default Page Size"),
        TextColumn("description", "Description"),
        TextColumn("grid_view", "Grid View"),
        NumberColumn("hide_in_all", "Hide in All"),
        NumberColumn("min_occurs", "Min Occurs"),
        NumberColumn("max_occurs", "Max Occurs"),
        NumberColumn("open_related_form", "Open Related Form"),
        NumberColumn("requires_related", "Requires Related"),
        TextColumn("new_related_option", "New Related Option"),
        TextColumn("behavior", "Behavior"),
    ];

    public override string Name => "Relationship Types";

    /// <summary>
    /// The RelationshipTypes table, then one section for each RelationshipType. A null
    /// RelationshipType (no related ItemType) also gets the properties of its relationship
    /// ItemType, from the second column onward.
    /// </summary>
    public override void Write(SheetWriter writer)
    {
        var relationships = ByItemType("RelationshipTypes");
        writer.Title("RelationshipTypes", large: true);
        writer.Table(Columns, relationships);
        writer.Blank();

        writer.Title("Corresponding Item Type Properties", large: true);
        foreach (var rel in relationships.OrderBy(r => r["name"], StringComparer.OrdinalIgnoreCase))
        {
            if (rel["is_null"] == "1")
            {
                writer.Title($"{rel["name"]} - NULL Relationship");
                writer.Table(PropertiesSheet.Columns, Sql.Run("Properties", ("SourceId", rel["relationship_id"])), firstColumn: 2);
            }
            else
            {
                writer.Title(rel["name"]);
            }

            writer.Blank();
        }
    }
}
