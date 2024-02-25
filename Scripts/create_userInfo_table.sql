USE [Arkenea]
GO

/****** Object:  Table [dbo].[UserInfo]    Script Date: 2/25/2024 5:46:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](150) NOT NULL,
	[CreatedOn] [datetime] NOT NULL
) ON [PRIMARY]
GO


