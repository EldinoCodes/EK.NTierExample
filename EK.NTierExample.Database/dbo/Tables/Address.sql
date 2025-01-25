CREATE TABLE [dbo].[Address]
(
    [AddressId]    UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF.dbo.Address.AddressId] DEFAULT (NEWSEQUENTIALID()),    
    [CreatedDate]  DATETIME2 (7)    NOT NULL,
    [CreatedBy]    NVARCHAR (128)   NOT NULL,
    [ModifiedDate] DATETIME2 (7)    NOT NULL,
    [ModifiedBy]   NVARCHAR (128)   NOT NULL,
    [Address1]     NVARCHAR (512)  NOT NULL,
    [Address2]     NVARCHAR (512)   NULL,
    [Locality]     NVARCHAR (512)   NOT NULL,
    [Region]       NVARCHAR (512)   NOT NULL,
    [PostalCode]   NVARCHAR (32)    NOT NULL,
    [Country]      NVARCHAR (128)   NOT NULL, 
    CONSTRAINT [PK.dbo.Address] PRIMARY KEY CLUSTERED ([AddressId] ASC) ON [PRIMARY],
);
GO;

CREATE INDEX [IDX.dbo.Address.Address1]
ON [dbo].[Address]
(
    Address1
)
GO;
CREATE INDEX [IDX.dbo.Address.Locality]
ON [dbo].[Address]
(
    Locality
)
GO;
CREATE INDEX [IDX.dbo.Address.Region.Country]
ON [dbo].[Address]
(
    Region,
    Country
)
GO;