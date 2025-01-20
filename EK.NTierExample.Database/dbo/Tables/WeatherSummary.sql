CREATE TABLE [dbo].[WeatherSummary]
(
    [WeatherSummaryId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF.dbo.WeatherSummary.WeatherSummaryId] DEFAULT (NEWSEQUENTIALID()),
    [CreatedDate]  DATETIME2 (7)    NOT NULL,
    [CreatedBy]    NVARCHAR (128)   NOT NULL,
    [ModifiedDate] DATETIME2 (7)    NOT NULL,
    [ModifiedBy]   NVARCHAR (128)   NOT NULL,
    [Description]      NVARCHAR (128)   NOT NULL,
    CONSTRAINT [PK.dbo.WeatherSummary] PRIMARY KEY CLUSTERED ([WeatherSummaryId] ASC) ON [PRIMARY]
)
GO;

CREATE INDEX [IDX.dbo.WeatherSummary.Description]
ON [dbo].[WeatherSummary]
(
    [Description]
)
GO;