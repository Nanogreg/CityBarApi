CREATE TABLE [dbo].[Cities]
(

	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[PostCode] INT NOT NULL UNIQUE,
	[Name] VARCHAR(50) not null,
	[About] VARCHAR(150), 
	[BannerPic] VARCHAR(100)
)
