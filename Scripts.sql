USE [MyBank]
GO
/****** Object:  Table [dbo].[RegisterUsers]    Script Date: 25-01-2024 19:23:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegisterUsers](
	[RegisterUserId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Username] [varchar](100) NOT NULL,
	[PasswordSalt] [binary](100) NULL,
	[PasswordHash] [binary](100) NULL,
	[Device] [varchar](100) NULL,
	[Ipaddress] [varchar](100) NULL,
	[Token] [varchar](250) NULL,
	[Browser] [varchar](100) NULL,
	[LastLogin] [datetime2](7) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK__RegisterUserId_RegisterUser] PRIMARY KEY CLUSTERED 
(
	[RegisterUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 25-01-2024 19:23:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[Username] [varchar](100) NOT NULL,
	[Balance] [decimal](18, 2) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK__Users_UserId] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__Users__Balance]  DEFAULT ((0.00)) FOR [Balance]
GO
ALTER TABLE [dbo].[RegisterUsers]  WITH CHECK ADD  CONSTRAINT [FK_RegisterUser_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[RegisterUsers] CHECK CONSTRAINT [FK_RegisterUser_Users]
GO
