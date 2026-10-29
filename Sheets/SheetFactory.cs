namespace ConfigWorkbook.Sheets;

/// <summary>Creates the sheets of the configuration workbook, in workbook order.</summary>
internal static class SheetFactory
{
    public static IReadOnlyList<Sheet> CreateAll(SqlRunner sql, string itemTypeId) =>
    [
        new PackagingSheet(sql, itemTypeId),
        new PropertiesSheet(sql, itemTypeId),
        new RelationshipTypesSheet(sql, itemTypeId),
        new ViewsSheet(sql, itemTypeId),
        new ServerEventsSheet(sql, itemTypeId),
        new ActionsSheet(sql, itemTypeId),
        new LifeCycleMapsSheet(sql, itemTypeId),
        new PermissionsSheet(sql, itemTypeId),
        new CanAddSheet(sql, itemTypeId),
        new ClientEventsSheet(sql, itemTypeId),
        new ReportsSheet(sql, itemTypeId),
        new ClientStyleSheet(sql, itemTypeId),
    ];
}
