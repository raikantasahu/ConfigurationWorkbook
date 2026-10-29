-- From "Server Events by Item Type ID.sql".
SELECT m.name AS name
      ,m.METHOD_TYPE AS method_type
      ,m.GENERATION AS version
      ,(select name from innovator.[identity] where id = m.EXECUTION_ALLOWED_TO) AS execution_allowed_to
      ,ISNULL(m.TEMPLATE,'') AS template
      ,ISNULL(m.COMMENTS,'') AS comments
      ,[SERVER_EVENT] AS server_event
      ,[SORT_ORDER] AS sort_order
      ,[EVENT_VERSION] AS event_version
  FROM [innovator].[SERVER_EVENT] se
  inner join innovator.[METHOD] m on m.id = se.RELATED_ID
  WHERE se.SOURCE_ID = @ItemTypeId
  ORDER BY SERVER_EVENT, m.NAME, SORT_ORDER
