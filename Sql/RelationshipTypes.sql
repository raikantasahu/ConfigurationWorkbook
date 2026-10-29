-- From "RelationshipTypes by Source_Id (ItemType Id).sql".
SELECT rel.RELATIONSHIP_ID AS relationship_id
      ,CASE WHEN rel.RELATED_ID IS NULL THEN 1 ELSE 0 END AS is_null
      ,rel.[SORT_ORDER] AS tab_order
      ,rel.[NAME] AS name
      ,rel.[LABEL] AS tab_label
      ,ISNULL((SELECT NAME FROM innovator.ITEMTYPE where id = rel.related_id),'') AS related_item
      ,rel.[AUTO_SEARCH] AS auto_search
      ,rel.[COPY_PERMISSIONS] AS copy_permissions
      ,rel.[CREATE_RELATED] AS create_related
      ,ISNULL(CAST(rel.[DEFAULT_PAGE_SIZE] AS VARCHAR),'') AS default_page_size
      ,ISNULL(rel.[DESCRIPTION],'') AS description
      ,ISNULL(rel.[GRID_VIEW],'') AS grid_view
      ,rel.[HIDE_IN_ALL] AS hide_in_all
      ,ISNULL(CAST(rel.[MIN_OCCURS] AS VARCHAR),'') AS min_occurs
      ,ISNULL(CAST(rel.[MAX_OCCURS] AS VARCHAR),'') AS max_occurs
      ,rel.[NEW_SHOW_RELATED] AS open_related_form
      ,rel.[RELATED_NOTNULL] AS requires_related
      ,CASE WHEN rel.[RELATED_OPTION] = '0' THEN 'Pick Only'
            WHEN rel.[RELATED_OPTION] = '1' THEN 'Create Only'
            WHEN rel.[RELATED_OPTION] = '2' THEN 'Pick & Create'
       END AS new_related_option
      ,rel.[BEHAVIOR] AS behavior
  FROM [innovator].[RELATIONSHIPTYPE] rel
  WHERE rel.SOURCE_ID = @ItemTypeId
  ORDER BY rel.[SORT_ORDER]
