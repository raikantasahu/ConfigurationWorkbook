namespace ConfigWorkbook.Sheets;

internal sealed class PermissionsSheet(SqlRunner sql, string itemTypeId) : Sheet(sql, itemTypeId)
{
    private static readonly Column[] Columns =
    [
        TextColumn("permission_name", "Permission Name"),
        TextColumn("identity_name", "Identity"),
        NumberColumn("can_discover", "Can Discover"),
        NumberColumn("can_get", "Can Get"),
        NumberColumn("can_update", "Can Update"),
        NumberColumn("can_delete", "Can Delete"),
        NumberColumn("show_permissions_warning", "Show Permission"),
        NumberColumn("can_change_access", "Can Change Access"),
    ];

    public override string Name => "Permissions";

    /// <summary>One group of rows for each permission. Only the first row of a group shows its name.</summary>
    public override void Write(SheetWriter writer)
    {
        var groups = ByItemType("Permissions").GroupBy(r => r["permission_name"]).ToList();
        var rows = new List<Row>();
        foreach (var group in groups)
        {
            if (rows.Count > 0)
            {
                rows.Add(new Row(new Dictionary<string, string>()));
            }

            rows.AddRange(group.Select((r, i) => i == 0 ? r : r.With("permission_name", "")));
        }

        writer.Table(Columns, rows);
    }
}
