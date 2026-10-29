-- From "CUI by Item Type ID.sql" (first query).
SELECT pc.NAME AS name
      ,pc.COLOR AS color
      ,itpc.CLIENT AS client
  FROM innovator.PRESENTATIONCONFIGURATION pc
  INNER JOIN innovator.ITPRESENTATIONCONFIGURATION itpc ON pc.ID = itpc.RELATED_ID
  WHERE itpc.SOURCE_ID = @ItemTypeId
  ORDER BY pc.NAME
