-- From "Reports By Item Type ID.sql".
SELECT r.NAME AS name
      ,r.DESCRIPTION AS description
      ,r.TYPE AS type
      ,r.TARGET AS target
      ,m.NAME AS method
      ,r.LOCATION AS location
  FROM innovator.REPORT r
  INNER JOIN innovator.ITEM_REPORT ir ON r.ID = ir.RELATED_ID
  INNER JOIN innovator.METHOD m on r.METHOD = m.ID
  WHERE ir.SOURCE_ID = @ItemTypeId
  ORDER BY r.NAME
