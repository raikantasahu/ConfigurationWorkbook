-- The Relationship Views that show a Tree Grid View, with the Tree Grid View and its Query Definition.
-- The PARAMETERS of such a view hold the id of the Tree Grid View, for example 'tgvdId=<32 hex>'.
SELECT rel.[NAME] AS name
      ,tgv.[NAME] AS tree_grid_view
      ,ISNULL(qd.[NAME],'') AS query_definition
  FROM [innovator].[RELATIONSHIP_VIEW] rv
  INNER JOIN [innovator].[RELATIONSHIPTYPE] rel ON rv.SOURCE_ID = rel.ID
  INNER JOIN [innovator].[RB_TREEGRIDVIEWDEFINITION] tgv
          ON tgv.ID = SUBSTRING(rv.[PARAMETERS], CHARINDEX('tgvdId=', rv.[PARAMETERS]) + 7, 32)
  LEFT JOIN [innovator].[QRY_QUERYDEFINITION] qd ON tgv.QUERY_DEFINITION = qd.ID
  WHERE rel.SOURCE_ID = @ItemTypeId
    AND rv.[PARAMETERS] LIKE '%tgvdId=%'
  ORDER BY rv.[SORT_ORDER]
