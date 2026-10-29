-- From "Actions by Item Type ID.sql".
SELECT a.[NAME] AS action_name
      ,ISNULL((SELECT name from innovator.[METHOD] where id = [CAN_EXECUTE]),'') AS can_execute
      ,[ITEM_QUERY] AS item_query
      ,ISNULL(a.[LABEL],'') AS label
      ,[LOCATION] AS location
      ,ISNULL((SELECT name from innovator.[METHOD] where id = a.[METHOD]),'') AS method
      ,ISNULL((SELECT name from innovator.[METHOD] where id = a.[ON_COMPLETE]),'') AS on_complete
      ,[TARGET] AS target
      ,[TYPE] AS type
  FROM innovator.[ACTION] a
  inner join innovator.ITEM_ACTION ia on a.id = ia.RELATED_ID
  WHERE ia.SOURCE_ID = @ItemTypeId
  ORDER BY a.NAME
