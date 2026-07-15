USE [INSYS]
GO

/****** Object:  Table [epolicy].[Request]    Script Date: 7/15/2026 15:36:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [epolicy].[Request](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ParentAno] [bigint] NULL,
	[PolicyNo] [varchar](30) NULL,
	[StartCertificateNo] [varchar](10) NULL,
	[EndCertificateNo] [varchar](10) NULL,
	[TOC] [varchar](5) NULL,
	[CompanyCode] [varchar](10) NULL,
	[DocumentType] [varchar](5) NULL,
	[ApiUrl] [varchar](200) NULL,
	[IsCertificate] [bit] NULL,
	[IsCustomLayout] [bit] NULL,
	[IsUploadToCare] [bit] NULL,
	[DownloadUrl] [varchar](200) NULL,
	[IsTax] [bit] NULL,
	[FailedCount] [int] NULL,
	[Status] [varchar](1) NULL,
	[Message] [varchar](200) NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [varchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Request] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [epolicy].[Request] ADD  CONSTRAINT [DF_Request_CreatedBy]  DEFAULT ('system') FOR [CreatedBy]
GO

ALTER TABLE [epolicy].[Request] ADD  CONSTRAINT [DF_Request_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [epolicy].[Request] ADD  CONSTRAINT [DF_Request_UpdatedBy]  DEFAULT ('system') FOR [UpdatedBy]
GO

ALTER TABLE [epolicy].[Request] ADD  CONSTRAINT [DF_Request_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
