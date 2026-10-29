-- From "Life Cycle Maps and States by Item Type ID.sql" (second query).
SELECT lcs.[NAME] AS name
      ,lcs.[LABEL] AS label
      ,ISNULL([ITEM_BEHAVIOR],'') AS item_behavior
      ,[SET_IS_RELEASED] AS released
      ,[SET_NOT_LOCKABLE] AS lockable
      ,ISNULL((SELECT NAME FROM innovator.PERMISSION WHERE ID = [STATE_PERMISSION_ID]),'') AS state_permission
      ,ISNULL([WORKFLOW],'') AS workflow
      ,ISNULL(HISTORY_TEMPLATE,'') AS history_template
      ,[X] AS x
      ,[Y] AS y
  FROM [innovator].[LIFE_CYCLE_STATE] lcs
  WHERE lcs.SOURCE_ID = @MapId
  ORDER BY lcs.[NAME]
