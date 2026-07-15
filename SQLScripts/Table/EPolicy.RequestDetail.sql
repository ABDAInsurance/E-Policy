USE [INSYS]
GO

/****** Object:  Table [epolicy].[RequestDetail]    Script Date: 7/15/2026 15:37:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [epolicy].[RequestDetail](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[HeaderId] [bigint] NOT NULL,
	[Ano] [bigint] NULL,
	[PolicyNo] [varchar](30) NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [varchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_RequestDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [epolicy].[RequestDetail] ADD  CONSTRAINT [DF_RequestDetail_CreatedBy]  DEFAULT ('system') FOR [CreatedBy]
GO

ALTER TABLE [epolicy].[RequestDetail] ADD  CONSTRAINT [DF_RequestDetail_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [epolicy].[RequestDetail] ADD  CONSTRAINT [DF_RequestDetail_UpdatedBy]  DEFAULT ('system') FOR [UpdatedBy]
GO

ALTER TABLE [epolicy].[RequestDetail] ADD  CONSTRAINT [DF_RequestDetail_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
