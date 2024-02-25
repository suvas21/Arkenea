USE [Arkenea]
GO

/****** Object:  Table [dbo].[Roles]    Script Date: 2/25/2024 5:46:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NULL
) ON [PRIMARY]
GO


