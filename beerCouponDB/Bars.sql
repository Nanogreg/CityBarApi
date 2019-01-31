CREATE TABLE [dbo].[Bars]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] VARCHAR (50) NOT NULL,
	[Address] VARCHAR (100),
	[About] VARCHAR (50),
	[TAGS] VARCHAR (200), /*TAGS are separated by - */
	[BarPic] VARCHAR(100),
	[FkCityId] INT NOT NULL,
	FOREIGN KEY (FkCityId) REFERENCES Cities(Id)
)

