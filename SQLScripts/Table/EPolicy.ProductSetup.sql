USE [INSYS]
GO
/****** Object:  Table [epolicy].[ProductSetup]    Script Date: 7/15/2026 15:34:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [epolicy].[ProductSetup](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TOC] [varchar](5) NULL,
	[SourceId] [varchar](20) NULL,
	[InsuredId] [varchar](20) NULL,
	[CompanyCode] [varchar](10) NULL,
	[DocumentType] [varchar](5) NULL,
	[IsCertificate] [bit] NULL,
	[IsCustomLayout] [bit] NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [varchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_ProductSetup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [epolicy].[ProductSetup] ON 

INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'0216', N'MK0IB00009', N'MK0IB00009', N'BF', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'1219', N'MK0IB00009', N'MK0IB00009', N'BF', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, N'0216', N'M99CD19001', N'D99BY24001', N'BMI', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, N'0203', N'M99BN23001', N'M99BN23001', N'BNI', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (5, N'0216', N'M99CD19001', N'D99CH25005', N'CSI', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (6, N'0216', N'M99EI25002', N'D99PT25048', N'EIO', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (7, N'1227', N'M99EI25001', N'D99PT25048', N'EIO', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (8, N'0215', N'M99PE24001', N'M99PE24001', N'EKB', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (9, N'0214', N'M99PGA0003X', N'M99PGA0003', N'GFI', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10, N'1012', N'M99PR22001', N'D99MA24145', N'GLI', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11, N'0214', N'M99GO24001', N'M99GO24001', N'GR', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (12, N'0214', N'M99GO24001', N'M99GO24001', N'GR', N'PN', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (13, N'0214', N'M99PKB0001', N'M99PKB0001', N'KBFM', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (14, N'0214', N'M99KSB0001', N'M99KSB0001', N'KSIS', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (15, N'0215', N'M99KSB0001', N'M99KSB0001', N'KSIS', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (16, N'0214', N'M99LVB0001', N'D70LVB0001', N'LVI', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17, N'0215', N'M99LVB0001', N'D70LVB0001', N'LVI', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18, N'1009', N'M99LVB0001', N'D70LVB0001', N'LVI', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (19, N'0216', N'M99MU15001', N'M99MU15001', N'MUF', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20, N'1014', N'M99PP24001', N'M99PP24001', N'PAM', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (21, N'1012', N'M99PR22001', N'D99SU24207', N'SAT', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (22, N'0216', N'M90PS24001', N'M90PS24001', N'SKBF', N'PS', 1, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[ProductSetup] ([Id], [TOC], [SourceId], [InsuredId], [CompanyCode], [DocumentType], [IsCertificate], [IsCustomLayout], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (23, N'1015', N'', N'', N'', N'PS', 0, 1, N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [epolicy].[ProductSetup] OFF
GO
ALTER TABLE [epolicy].[ProductSetup] ADD  CONSTRAINT [DF_ProductSetup_CreatedBy]  DEFAULT ('system') FOR [CreatedBy]
GO
ALTER TABLE [epolicy].[ProductSetup] ADD  CONSTRAINT [DF_ProductSetup_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [epolicy].[ProductSetup] ADD  CONSTRAINT [DF_ProductSetup_UpdatedBy]  DEFAULT ('system') FOR [UpdatedBy]
GO
ALTER TABLE [epolicy].[ProductSetup] ADD  CONSTRAINT [DF_ProductSetup_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
