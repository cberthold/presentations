
USE [Taxes]
GO
/****** Object:  Table [dbo].[Address]    Script Date: 6/16/2016 10:18:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Address](
	[SalesOrderId] [uniqueidentifier] NOT NULL,
	[Address1] [varchar](150) NOT NULL,
	[Address2] [varchar](150) NOT NULL,
	[City] [varchar](50) NOT NULL,
	[State] [varchar](10) NOT NULL,
	[Zip] [varchar](14) NOT NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[SalesOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SalesOrder]    Script Date: 6/16/2016 10:18:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesOrder](
	[Id] [uniqueidentifier] NOT NULL,
	[SubTotal] [decimal](18, 2) NOT NULL,
	[Tax] [decimal](18, 2) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_SalesOrder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SalesOrderLineItem]    Script Date: 6/16/2016 10:18:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SalesOrderLineItem](
	[Id] [uniqueidentifier] NOT NULL,
	[SalesOrderId] [uniqueidentifier] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[LineTotal] [decimal](18, 2) NOT NULL,
	[Product] [varchar](200) NOT NULL,
 CONSTRAINT [PK_SalesOrderLineItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StateTax]    Script Date: 6/16/2016 10:18:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StateTax](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_StateTax_Id]  DEFAULT (newid()),
	[State] [varchar](10) NOT NULL,
	[TaxPercent] [decimal](12, 8) NOT NULL,
 CONSTRAINT [PK_StateTax] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[StateTax] ([Id], [State], [TaxPercent]) VALUES (N'e758cf03-b787-4cc5-9e10-09b601269731', N'DC', CAST(0.00000000 AS Decimal(12, 8)))
GO
INSERT [dbo].[StateTax] ([Id], [State], [TaxPercent]) VALUES (N'c0efef97-5a0e-420c-86a1-6e6a6e44ca6a', N'FL', CAST(6.00000000 AS Decimal(12, 8)))
GO
INSERT [dbo].[StateTax] ([Id], [State], [TaxPercent]) VALUES (N'b628fbe0-e144-4d3e-9fbd-9e2291b64e8a', N'GA', CAST(8.00000000 AS Decimal(12, 8)))
GO
ALTER TABLE [dbo].[SalesOrderLineItem] ADD  CONSTRAINT [DF_SalesOrderLineItem_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[SalesOrderLineItem] ADD  CONSTRAINT [DF_SalesOrderLineItem_Price]  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[SalesOrderLineItem] ADD  CONSTRAINT [DF_SalesOrderLineItem_LineTotal]  DEFAULT ((0)) FOR [LineTotal]
GO
ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_Address_SalesOrder] FOREIGN KEY([SalesOrderId])
REFERENCES [dbo].[SalesOrder] ([Id])
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_Address_SalesOrder]
GO
ALTER TABLE [dbo].[SalesOrderLineItem]  WITH CHECK ADD  CONSTRAINT [FK_SalesOrderLineItem_SalesOrder] FOREIGN KEY([SalesOrderId])
REFERENCES [dbo].[SalesOrder] ([Id])
GO
ALTER TABLE [dbo].[SalesOrderLineItem] CHECK CONSTRAINT [FK_SalesOrderLineItem_SalesOrder]
GO
USE [master]
GO
ALTER DATABASE [Taxes] SET  READ_WRITE 
GO
