-- The package that contains the ItemType.
SELECT pd.NAME AS package_name
  FROM innovator.PACKAGEELEMENT pe
  INNER JOIN innovator.PACKAGEGROUP pg ON pe.SOURCE_ID = pg.ID
  INNER JOIN innovator.PACKAGEDEFINITION pd ON pg.SOURCE_ID = pd.ID
  WHERE pe.ELEMENT_ID = @ItemTypeId
