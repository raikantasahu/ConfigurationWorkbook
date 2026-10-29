-- From "CUI by Item Type ID.sql" (second query).
SELECT it.NAME AS itemtype_name
      ,pc.NAME AS presentation_configuration
      ,cbs.NAME AS name
      ,ISNULL(cbs.BUILDER_METHOD,'') AS builder_method
      ,ISNULL(cbs.LABEL,'') AS label
      ,ISNULL(cbs.DESCRIPTION,'') AS description
      ,ISNULL(cbs.ADDITIONAL_DATA,'') AS additional_data
      ,(SELECT NAME FROM innovator.CUI_LOCATION WHERE ID = cbs.LOCATION) AS location
      ,pcom.SORT_ORDER AS sort_order
      ,(SELECT NAME FROM innovator.[IDENTITY] WHERE ID = pcom.ROLE) AS for_identity
      ,ISNULL(pcom.ITEM_CLASSIFICATION,'') AS for_classification
  FROM innovator.COMMANDBARSECTION cbs
  INNER JOIN innovator.PRESENTATIONCOMMANDBARSECTION pcom ON cbs.ID = pcom.RELATED_ID
  INNER JOIN innovator.PRESENTATIONCONFIGURATION pc ON pcom.SOURCE_ID = pc.ID
  INNER JOIN innovator.ITPRESENTATIONCONFIGURATION itpc ON pc.ID = itpc.RELATED_ID
  INNER JOIN innovator.ITEMTYPE it ON itpc.SOURCE_ID = it.ID
  WHERE it.ID = @ItemTypeId
  ORDER BY pc.NAME, pcom.SORT_ORDER
