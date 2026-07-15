USE [INSYS]
GO
/****** Object:  Table [epolicy].[msPartner]    Script Date: 7/15/2026 15:32:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [epolicy].[msPartner](
	[CompanyCode] [varchar](10) NOT NULL,
	[CompanyName] [varchar](100) NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [varchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_msPartner] PRIMARY KEY CLUSTERED 
(
	[CompanyCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [epolicy].[msPartner] ([CompanyCode], [CompanyName], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'BF', N'BUANA FINANCE', N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[msPartner] ([CompanyCode], [CompanyName], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'BMI', N'BYD MOTOR INDONESIA', N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[msPartner] ([CompanyCode], [CompanyName], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'BNI', N'BANK NEGARA INDONESIA', N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[msPartner] ([CompanyCode], [CompanyName], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'CSI', N'CHERY SALES INDONESIA', N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[msPartner] ([CompanyCode], [CompanyName], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'EIO', N'PT ERA INOVASI OTOMOTIF', N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[msPartner] ([CompanyCode], [CompanyName], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'EKB', N'PT ENERGI KREASI BERSAMA (ELECTRUM)', N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[msPartner] ([CompanyCode], [CompanyName], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'GFI', N'PT GRATAMA FINANCE INDONESIA', N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[msPartner] ([CompanyCode], [CompanyName], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'GLI', N'PT. GLOBAL LOYALTY INDONESIA', N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[msPartner] ([CompanyCode], [CompanyName], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'GR', N'GO RENTAL', N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[msPartner] ([CompanyCode], [CompanyName], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'KBFM', N'PT. KB FINANSIA MULTI FINANCE', N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[msPartner] ([CompanyCode], [CompanyName], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'KSIS', N'KOPERASI SWADAYA INTI SEJAHTERA', N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[msPartner] ([CompanyCode], [CompanyName], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'LVI', N'LOLC VENTURA INDONESIA', N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[msPartner] ([CompanyCode], [CompanyName], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'MUF', N'MANDIRI UTAMA FINANCE', N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[msPartner] ([CompanyCode], [CompanyName], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'PAM', N'PT PLAZA AUTO MEGA (SUBARU INDONESIA)', N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[msPartner] ([CompanyCode], [CompanyName], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'SAT', N'PT. SUMBER ALFARIA TRIJAYA', N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
INSERT [epolicy].[msPartner] ([CompanyCode], [CompanyName], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'SKBF', N'PT SUNINDO KOOKMIN BEST FINANCE', N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime), N'system', CAST(N'2026-07-01T00:00:00.000' AS DateTime))
GO
ALTER TABLE [epolicy].[msPartner] ADD  CONSTRAINT [DF_msPartner_CreatedBy]  DEFAULT ('system') FOR [CreatedBy]
GO
ALTER TABLE [epolicy].[msPartner] ADD  CONSTRAINT [DF_msPartner_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [epolicy].[msPartner] ADD  CONSTRAINT [DF_msPartner_UpdatedBy]  DEFAULT ('system') FOR [UpdatedBy]
GO
ALTER TABLE [epolicy].[msPartner] ADD  CONSTRAINT [DF_msPartner_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
