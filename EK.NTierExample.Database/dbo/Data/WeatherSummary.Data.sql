USE [NTierExample]
GO

INSERT INTO [dbo].[WeatherSummary]
           ([CreatedDate]
           ,[CreatedBy]
           ,[ModifiedDate]
           ,[ModifiedBy]
           ,[Description])
     VALUES
           (GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', 'Freezing'),
           (GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', 'Bracing'),
           (GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', 'Chilly'),
           (GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', 'Cool'),
           (GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', 'Mild'),
           (GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', 'Warm'),
           (GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', 'Balmy'),
           (GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', 'Hot'),
           (GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', 'Sweltering'),
           (GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', 'Scorching')
GO