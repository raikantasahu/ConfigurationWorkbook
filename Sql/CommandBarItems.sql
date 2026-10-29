-- From "CUI by Item Type ID.sql" (third query).
SELECT it.NAME AS itemtype_name
      ,pc.NAME AS presentation_configuration
      ,cbs.NAME AS command_bar_section
      ,(SELECT LABEL FROM innovator.ITEMTYPE WHERE ID = cbi.ITEMTYPE) AS itemtype
      ,cbi.NAME AS name
      ,cbi.ADDITIONAL_DATA AS additional_data
      ,ISNULL((SELECT NAME FROM innovator.METHOD WHERE ID = cbi.ON_INIT_HANDLER),'') AS init_method
      ,cbsi.SORT_ORDER AS sort_order
      ,cbsi.ACTION AS action
      ,cbsi.ALTERNATE AS alternate
      ,(SELECT NAME FROM innovator.[IDENTITY] WHERE ID = cbsi.ROLE) AS for_identity
      ,ISNULL(cbsi.ITEM_CLASSIFICATION,'') AS for_classification
  FROM innovator.COMMANDBARITEM cbi
  INNER JOIN innovator.COMMANDBARSECTIONITEM cbsi ON cbi.ID = cbsi.RELATED_ID
  INNER JOIN innovator.COMMANDBARSECTION cbs ON cbsi.SOURCE_ID = cbs.ID
  INNER JOIN innovator.PRESENTATIONCOMMANDBARSECTION pcom ON cbs.ID = pcom.RELATED_ID
  INNER JOIN innovator.PRESENTATIONCONFIGURATION pc ON pcom.SOURCE_ID = pc.ID
  INNER JOIN innovator.ITPRESENTATIONCONFIGURATION itpc ON pc.ID = itpc.RELATED_ID
  INNER JOIN innovator.ITEMTYPE it ON itpc.SOURCE_ID = it.ID
  WHERE it.ID = @ItemTypeId
  ORDER BY pc.NAME, cbs.NAME, cbsi.SORT_ORDER
