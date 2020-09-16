USE [CateringPro]
GO

/****** Object:  Table [dbo].[Discounts]    Script Date: 9/16/2020 10:56:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Discounts](
	[Id] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Code] [nchar](15) NOT NULL,
	[Name] [nchar](100) NOT NULL,
	[Value] [decimal](18, 2) NOT NULL,
	[Type] [int] NOT NULL,
	[DateFrom] [date] NOT NULL,
	[DateTo] [date] NOT NULL,
	[Categories] [nchar](100) NOT NULL,
 CONSTRAINT [PK_Discounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


