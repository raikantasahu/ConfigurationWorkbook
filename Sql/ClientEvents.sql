-- From "Client Events by Item Type ID.sql".
SELECT m.NAME AS name
      ,m.METHOD_TYPE AS method_type
      ,m.GENERATION AS version
      ,m.EXECUTION_ALLOWED_TO AS execution_allowed_to
      ,m.TEMPLATE AS template
      ,m.COMMENTS AS comments
      ,ce.CLIENT_EVENT AS event
      ,ce.SORT_ORDER AS sort_order
  FROM innovator.CLIENT_EVENT ce
  INNER JOIN innovator.METHOD m ON ce.RELATED_ID = m.ID
  WHERE ce.SOURCE_ID = @ItemTypeId
  ORDER BY ce.CLIENT_EVENT, ce.SORT_ORDER
