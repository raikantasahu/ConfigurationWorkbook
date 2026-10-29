namespace ConfigWorkbook.Sheets;

internal sealed class ViewsSheet(SqlRunner sql, string itemTypeId) : Sheet(sql, itemTypeId)
{
    private static readonly Column[] ViewColumns =
    [
        TextColumn("form_name", "Form Name"),
        TextColumn("type", "Type"),
        TextColumn("description", "Description"),
        NumberColumn("display_priority", "Display Priority"),
        TextColumn("form_classification", "Form Classification"),
        TextColumn("label", "Label"),
        TextColumn("method", "Method"),
        TextColumn("identity_name", "Identity"),
        TextColumn("url", "URL"),
        NumberColumn("ssvc", "SSVC"),
    ];

    private static readonly Column[] FormFieldColumns =
    [
        TextColumn("field_name", "Field Name"),
        TextColumn("bg_color", "Background Color"),
        NumberColumn("border_width", "Border Width"),
        TextColumn("container", "Container Name"),
        NumberColumn("display_length", "Display Length"),
        TextColumn("display_length_unit", "Display Length Unit"),
        TextColumn("field_type", "Field Type"),
        TextColumn("font_color", "Font Color"),
        TextColumn("font_family", "Font Family"),
        TextColumn("font_size", "Font Size"),
        TextColumn("font_weight", "Font Weight"),
        NumberColumn("height", "Height"),
        TextColumn("html_code", "HTML"),
        NumberColumn("disabled", "Disabled"),
        NumberColumn("visible", "Visible"),
        TextColumn("label", "Label"),
        TextColumn("label_position", "Label Position"),
        TextColumn("legend", "Legend"),
        NumberColumn("list_no_blank", "List No Blank"),
        NumberColumn("listbox_size", "Listbox Size"),
        TextColumn("orientation", "Orientation"),
        TextColumn("positioning", "Positioning"),
        TextColumn("property", "Property"),
        NumberColumn("show_help", "SHOW_HELP"),
        NumberColumn("tab_index", "Tab Index"),
        NumberColumn("tab_stop", "Tab Stop"),
        TextColumn("text_align", "Text Align"),
        NumberColumn("textarea_cols", "Columns"),
        NumberColumn("textarea_rows", "Rows"),
        NumberColumn("width", "Width"),
        NumberColumn("x", "X"),
        NumberColumn("xclass_field_width", "XClass Field Width"),
        NumberColumn("xclass_text_width", "XClass Text Width"),
        NumberColumn("y", "Y"),
        NumberColumn("z_index", "Z-Index"),
        TextColumn("css", "CSS"),
        NumberColumn("sort_order", "Sort Order"),
    ];

    private static readonly Column[] FieldEventColumns =
    [
        TextColumn("field_name", "Field Name"),
        TextColumn("field_event", "Field Event"),
        TextColumn("method_name", "Method Name"),
        NumberColumn("sort_order", "Sort Order"),
    ];

    private static readonly Column[] FormEventColumns =
    [
        TextColumn("form_name", "Form Name"),
        TextColumn("form_event", "Form Event"),
        TextColumn("method_name", "Method Name"),
        NumberColumn("sort_order", "Sort Order"),
    ];

    public override string Name => "Views";

    /// <summary>The Views table, then the fields, field events and form events of each form.</summary>
    public override void Write(SheetWriter writer)
    {
        var views = ByItemType("Views");
        writer.Title("Views", large: true);
        writer.Table(ViewColumns, views);
        writer.Blank();

        writer.Title("Forms", large: true);
        foreach (var form in views.DistinctBy(v => v["form_id"]))
        {
            var formId = ("FormId", form["form_id"]);
            writer.Title(form["form_name"]);
            writer.Title("Form Fields");
            writer.Table(FormFieldColumns, Sql.Run("FormFields", formId));
            writer.Blank();
            writer.Title("Field Events");
            writer.Table(FieldEventColumns, Sql.Run("FieldEvents", formId), whenEmpty: "None");
            writer.Blank();
            writer.Title("Form Events");
            writer.Table(FormEventColumns, Sql.Run("FormEvents", formId), whenEmpty: "None");
            writer.Blank();
        }
    }
}
