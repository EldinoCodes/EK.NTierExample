USE [EK.NTierExample]
GO

INSERT INTO [dbo].[Address]
           ([CreatedDate]
           ,[CreatedBy]
           ,[ModifiedDate]
           ,[ModifiedBy]
           ,[Address1]
           ,[Address2]
           ,[Locality]
           ,[Region]
           ,[PostalCode]
           ,[Country]
           )
     VALUES
           (GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', '1313 Mockingbird Lane', NULL, 'Waxahachie', 'Texas', '75165', 'USA'),
           (GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', '129 West 81st Street', 'Apartment 5A', 'New York', 'New York', '10024', 'USA'),
           (GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', '1407 Graymalkin Lane', NULL, 'North Salem', 'New York', '10560', 'USA')
GO