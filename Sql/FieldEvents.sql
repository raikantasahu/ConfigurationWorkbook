-- From "Form Fields by Form ID.sql" (second query).
SELECT fi.name AS field_name
      ,fe.FIELD_EVENT AS field_event
      ,m.name AS method_name
      ,fe.[SORT_ORDER] AS sort_order
  FROM innovator.FORM f
  inner join innovator.BODY b on f.id = b.SOURCE_ID
  inner join innovator.FIELD fi on b.id = fi.SOURCE_ID
  inner join innovator.FIELD_EVENT fe on fi.id = fe.SOURCE_ID
  inner join innovator.METHOD m on fe.RELATED_ID = m.id and m.IS_CURRENT = '1'
  WHERE f.id = @FormId
  ORDER BY fi.name, fe.[SORT_ORDER]
