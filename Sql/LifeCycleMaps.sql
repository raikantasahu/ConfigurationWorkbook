-- From "Life Cycle Maps and States by Item Type ID.sql" (first query).
SELECT map.ID AS map_id
      ,map.[NAME] AS life_cycle_map
      ,ISNULL(map.[DESCRIPTION],'') AS description
      ,ISNULL(map.[LABEL],'') AS label
      ,(SELECT NAME FROM INNOVATOR.[LIFE_CYCLE_STATE] WHERE ID = [START_STATE]) AS start_state
  FROM innovator.ITEMTYPE_LIFE_CYCLE ilc
  inner join [innovator].[LIFE_CYCLE_MAP] map on ilc.RELATED_ID = map.id
  WHERE ilc.SOURCE_ID = @ItemTypeId
  ORDER BY map.NAME
