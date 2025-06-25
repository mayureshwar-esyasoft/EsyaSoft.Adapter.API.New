SELECT TOP (1000) [ServiceDirectionTypeID]
      ,[ServiceDirectionTypeName]
      ,[ServiceDirectionTypeDescription]
  FROM [ADAPTER].[dbo].[ServiceDirectionType]

  --TRUNCATE TABLE [ADAPTER].[dbo].[ServiceDirectionType]


  Insert into [ADAPTER].[dbo].[ServiceDirectionType] ([ServiceDirectionTypeID], [ServiceDirectionTypeName], [ServiceDirectionTypeDescription])
  values (100,'SAP2Adapter', 'SAP OUT - SAP to Adapter')
  Insert into [ADAPTER].[dbo].[ServiceDirectionType] ([ServiceDirectionTypeID], [ServiceDirectionTypeName], [ServiceDirectionTypeDescription])
  values (101,'AdapterToSAP', 'SAP IN - Adapter to SAP')

  Insert into [ADAPTER].[dbo].[ServiceDirectionType] ([ServiceDirectionTypeID], [ServiceDirectionTypeName], [ServiceDirectionTypeDescription])
  values (200,'MDMToAdapter', 'MDM OUT - MDM to Adapter')
  Insert into [ADAPTER].[dbo].[ServiceDirectionType] ([ServiceDirectionTypeID], [ServiceDirectionTypeName], [ServiceDirectionTypeDescription])
  values (201,'AdapterToMDM', 'MDM IN - Adapter to MDM') 