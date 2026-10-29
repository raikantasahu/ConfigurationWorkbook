-- From "Views by ItemType ID.sql" (first query).
SELECT f.id AS form_id
      ,f.NAME AS form_name
      ,[TYPE] AS type
      ,ISNULL([DESCRIPTION],'') AS description
      ,ISNULL(CAST([DISPLAY_PRIORITY] AS VARCHAR),'') AS display_priority
      ,ISNULL([FORM_CLASSIFICATION],'') AS form_classification
      ,ISNULL(v.[LABEL],'') AS label
      ,ISNULL([METHOD],'') AS method
      ,ISNULL((SELECT NAME FROM innovator.[IDENTITY] where id = [ROLE]),'') AS identity_name
      ,ISNULL([URL],'') AS url
      ,[SHOW_SSVC] AS ssvc
  FROM [innovator].[VIEW] v
  inner join innovator.FORM f on v.RELATED_ID = f.id
  WHERE v.SOURCE_ID = @ItemTypeId
  ORDER BY f.NAME
