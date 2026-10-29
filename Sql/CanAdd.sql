-- From "Can Add by Item Type.sql".
SELECT i.NAME AS name
      ,ca.CAN_ADD AS can_add
      ,ISNULL(ca.CLASS_PATH,'') AS class_path
  FROM innovator.[CAN_ADD] ca
  INNER JOIN innovator.[IDENTITY] i ON ca.RELATED_ID = i.ID
  WHERE ca.SOURCE_ID = @ItemTypeId
  ORDER BY i.NAME
