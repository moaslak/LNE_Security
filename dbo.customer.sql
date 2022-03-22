CREATE TABLE [dbo].[Customer]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[FirstName] varchar(50) NOT NULL,
	[LastName] varchar(50) NOT NULL, 
    [PhoneNumber] VARCHAR(20) NOT NULL, 
    [Email] VARCHAR(50) NOT NULL, 
    [WorkPhone] VARCHAR(20) NULL, 
    [Type] VARCHAR(20) NOT NULL, 
    [StreetName] VARCHAR(35) NOT NULL, 
    [HouseNumber] VARCHAR(10) NULL, 
    [ZipCode] NCHAR(10) NULL, 
    [Country] VARCHAR(50) NULL,


)
