-- From "Permissions by Item Type and LC States.sql".
SELECT p.[NAME] AS permission_name
      ,(SELECT NAME FROM INNOVATOR.[IDENTITY] WHERE ID = a.RELATED_ID) AS identity_name
      ,a.CAN_DISCOVER AS can_discover
      ,a.CAN_GET AS can_get
      ,a.CAN_UPDATE AS can_update
      ,a.CAN_DELETE AS can_delete
      ,a.SHOW_PERMISSIONS_WARNING AS show_permissions_warning
      ,a.CAN_CHANGE_ACCESS AS can_change_access
  FROM [innovator].[PERMISSION] p
  inner join innovator.ACCESS a ON p.id = a.SOURCE_ID
  WHERE p.id IN (SELECT RELATED_ID FROM innovator.ALLOWED_PERMISSION WHERE SOURCE_ID = @ItemTypeId)
     OR p.id IN (SELECT STATE_PERMISSION_ID FROM innovator.LIFE_CYCLE_STATE
                  WHERE SOURCE_ID IN (SELECT RELATED_ID FROM innovator.ITEMTYPE_LIFE_CYCLE WHERE SOURCE_ID = @ItemTypeId))
  ORDER BY p.NAME, identity_name
