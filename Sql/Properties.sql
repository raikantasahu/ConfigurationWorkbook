-- From "Properties by ItemType ID.sql". The SourceId parameter is the ItemType that owns the properties.
SELECT i.NAME AS itemtype_name
      ,p.[NAME] AS property_name
      ,ISNULL(p.LABEL,'') AS label
      ,[DATA_TYPE] AS data_type
      ,CASE
        WHEN [DATA_TYPE] = 'item' THEN (select name from innovator.ITEMTYPE where id = [DATA_SOURCE])
        WHEN [DATA_TYPE] = 'list' or [DATA_TYPE] = 'filter list'  THEN (select name from innovator.LIST where id = [DATA_SOURCE])
        else '' END AS data_source
      ,ISNULL(CAST([STORED_LENGTH] AS VARCHAR),'') AS length
      ,ISNULL(CAST([PREC] AS VARCHAR),'') AS precision
      ,ISNULL(CAST([SCALE] AS VARCHAR),'') AS scale
      ,[IS_REQUIRED] AS required
      ,[IS_KEYED] AS is_unique
      ,[IS_INDEXED] AS indexed
      ,[IS_FEDERATED] AS federated
      ,[IS_DISCOVERABLE] AS discoverable
      ,[IS_HIDDEN] AS hidden
      ,[IS_HIDDEN2] AS hidden2
      ,[COLUMN_ALIGNMENT] AS alignment
      ,ISNULL(CAST([COLUMN_WIDTH] AS VARCHAR),'') AS width
      ,ISNULL(CAST([SORT_ORDER] AS VARCHAR),'') AS sort_order
      ,ISNULL(CAST([KEYED_NAME_ORDER] AS VARCHAR),'') AS keyed_name_order
      ,ISNULL(CAST([ORDER_BY] AS VARCHAR),'') AS order_by
      ,ISNULL([DEFAULT_VALUE],'') AS default_value
      ,ISNULL([DEFAULT_SEARCH],'') AS default_search
      ,ISNULL([PATTERN],'') AS pattern
      ,ISNULL([FOREIGN_PROPERTY],'') AS foreign_property
      ,ISNULL([HELP_TOOLTIP],'') AS tooltip
      ,ISNULL([HELP_TEXT],'') AS help_text
      ,[TRACK_HISTORY] AS track_history
      ,[BEHAVIOR] AS behavior
      ,ISNULL(CAST([MIN_RANGE] AS VARCHAR),'') AS min_range
      ,ISNULL(CAST([MAX_RANGE] AS VARCHAR),'') AS max_range
      ,[RANGE_INCLUSIVE] AS range_inclusive
      ,[READONLY] AS read_only
  FROM [innovator].[PROPERTY] p
    inner join innovator.ITEMTYPE i on p.SOURCE_ID = i.ID
  WHERE p.SOURCE_ID = @SourceId
  ORDER BY p.SORT_ORDER, p.NAME
