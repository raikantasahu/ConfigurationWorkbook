-- From "Views by ItemType ID.sql" (second query). The BODY join is removed, because it can repeat rows.
SELECT f.name AS form_name
      ,fe.FORM_EVENT AS form_event
      ,m.name AS method_name
      ,fe.[SORT_ORDER] AS sort_order
  FROM innovator.FORM f
  inner join innovator.FORM_EVENT fe on f.id = fe.SOURCE_ID
  inner join innovator.METHOD m on fe.RELATED_ID = m.id and m.IS_CURRENT = '1'
  WHERE f.id = @FormId
  ORDER BY fe.[SORT_ORDER]
