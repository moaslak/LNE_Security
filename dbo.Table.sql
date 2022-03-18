CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Firmanavn] NVARCHAR(50) NOT NULL, 
    [Vej] NVARCHAR(50) NOT NULL, 
    [Husnummer] INT NOT NULL, 
    [Postnummer] INT NOT NULL, 
    [By] NVARCHAR(50) NOT NULL, 
    [Land] NVARCHAR(50) NOT NULL, 
    [Valuta] MONEY NOT NULL
)
