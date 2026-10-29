-- From "Form Fields by Form ID.sql" (first query).
SELECT fi.name AS field_name
      ,isnull(fi.[BG_COLOR],'') AS bg_color
      ,fi.[BORDER_WIDTH] AS border_width
      ,ISNULL(fi.[CONTAINER],'') AS container
      ,ISNULL(CAST(fi.[DISPLAY_LENGTH] as varchar),'') AS display_length
      ,fi.[DISPLAY_LENGTH_UNIT] AS display_length_unit
      ,fi.[FIELD_TYPE] AS field_type
      ,fi.[FONT_COLOR] AS font_color
      ,fi.[FONT_FAMILY] AS font_family
      ,fi.[FONT_SIZE] AS font_size
      ,fi.[FONT_WEIGHT] AS font_weight
      ,ISNULL(CAST(fi.[HEIGHT] as varchar),'') AS height
      ,ISNULL(fi.[HTML_CODE],'') AS html_code
      ,fi.[IS_DISABLED] AS disabled
      ,fi.[IS_VISIBLE] AS visible
      ,ISNULL(fi.[LABEL],'') AS label
      ,ISNULL(fi.[LABEL_POSITION],'') AS label_position
      ,ISNULL(fi.[LEGEND],'') AS legend
      ,fi.[LIST_NO_BLANK] AS list_no_blank
      ,ISNULL(CAST(fi.[LISTBOX_SIZE] as varchar),'') AS listbox_size
      ,ISNULL(fi.[ORIENTATION],'') AS orientation
      ,ISNULL(fi.[POSITIONING],'') AS positioning
      ,isnull((select name from innovator.Property where id = fi.[PROPERTYTYPE_ID]),'') AS property
      ,fi.[SHOW_HELP] AS show_help
      ,ISNULL(CAST(fi.[TAB_INDEX] as varchar),'') AS tab_index
      ,ISNULL(fi.[TAB_STOP],'') AS tab_stop
      ,ISNULL(fi.[TEXT_ALIGN],'') AS text_align
      ,ISNULL(fi.[TEXTAREA_COLS],'') AS textarea_cols
      ,ISNULL(fi.[TEXTAREA_ROWS],'') AS textarea_rows
      ,ISNULL(CAST(fi.[WIDTH] as varchar),'') AS width
      ,fi.[X] AS x
      ,ISNULL(fi.[XCLASS_FIELD_WIDTH],'') AS xclass_field_width
      ,ISNULL(fi.[XCLASS_TEXT_WIDTH],'') AS xclass_text_width
      ,fi.[Y] AS y
      ,ISNULL(fi.[Z_INDEX],'') AS z_index
      ,ISNULL(fi.[CSS],'') AS css
      ,fi.[SORT_ORDER] AS sort_order
  FROM innovator.FORM f
  inner join innovator.BODY b on f.id = b.SOURCE_ID
  inner join innovator.FIELD fi on b.id = fi.SOURCE_ID
  WHERE f.id = @FormId
  ORDER BY fi.[SORT_ORDER]
