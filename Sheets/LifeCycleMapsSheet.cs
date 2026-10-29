namespace ConfigWorkbook.Sheets;

internal sealed class LifeCycleMapsSheet(SqlRunner sql, string itemTypeId) : Sheet(sql, itemTypeId)
{
    private static readonly Column[] MapColumns =
    [
        TextColumn("life_cycle_map", "Life Cycle Map"),
        TextColumn("description", "Description"),
        TextColumn("label", "Label"),
        TextColumn("start_state", "Start State"),
    ];

    private static readonly Column[] StateColumns =
    [
        TextColumn("name", "Name"),
        TextColumn("label", "Label"),
        TextColumn("item_behavior", "Item Behavior"),
        NumberColumn("released", "Released"),
        NumberColumn("lockable", "Lockable"),
        TextColumn("state_permission", "State Permission"),
        TextColumn("workflow", "Workflow"),
        TextColumn("history_template", "History Template"),
        NumberColumn("x", "X"),
        NumberColumn("y", "Y"),
    ];

    public override string Name => "Life Cycle Maps";

    /// <summary>The Life Cycle Maps table, then the states of each map.</summary>
    public override void Write(SheetWriter writer)
    {
        var maps = ByItemType("LifeCycleMaps");
        writer.Table(MapColumns, maps);
        foreach (var map in maps)
        {
            writer.Blank();
            writer.Title($"Life Cycle States - {map["life_cycle_map"]}");
            writer.Table(StateColumns, Sql.Run("LifeCycleStates", ("MapId", map["map_id"])));
        }
    }
}
