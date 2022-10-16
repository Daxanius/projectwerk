/*
CREATE TABLES FOR GROUPSWORK IN TSQL
NOTE: DEFAULT GETDATE() IN AFSPRAAK STARTDATE
MIGHT WANT TO CHANGE LENGTHS ON CERTAIN VARCHARS

FOR Bedrijf, Werknemerbedrijf, Bezoeker, Afspraak
STATUS 1 == VALID (DEFAULT)
STATUS 2 == INVALID
*/

CREATE TABLE [dbo].[Functie](
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[FunctieNaam] VARCHAR(50) NOT NULL
)

CREATE TABLE [dbo].[AfspraakStatus](
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[AfspraakStatusNaam] VARCHAR(50) NOT NULL
)

CREATE TABLE [dbo].[Bedrijf](
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[Naam] VARCHAR(255) NOT NULL,
	[BTWNr] VARCHAR(30) NOT NULL,
	[TeleNr] VARCHAR(30) NOT NULL,
	[Email] VARCHAR(255) NOT NULL,
	[Adres] VARCHAR(255) NOT NULL,
	[Status] INT NOT NULL DEFAULT 1
)

CREATE TABLE [dbo].[Werknemer](
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[ANaam] VARCHAR(255) NOT NULL,
	[VNaam] VARCHAR(255) NOT NULL,
	[FunctieId] INT NOT NULL,
	CONSTRAINT [FK_Werknemer_Functie_Id] FOREIGN KEY ([FunctieId]) REFERENCES [dbo].[Functie](Id)
)

CREATE TABLE [dbo].[Werknemerbedrijf](
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[BedrijfId] INT NOT NULL,
	[WerknemerId] INT NOT NULL,
	[Status] INT NOT NULL DEFAULT 1,
	CONSTRAINT [FK_WerknemerBedrijf_Bedrijf_Id] FOREIGN KEY ([BedrijfId]) REFERENCES [dbo].[Bedrijf](Id),
	CONSTRAINT [FK_WerknemerBedrijf_Werknemer_Id] FOREIGN KEY ([WerknemerId]) REFERENCES [dbo].[Werknemer](Id)
)

CREATE TABLE [dbo].[Bezoeker](
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[ANaam] VARCHAR(255) NOT NULL,
	[VNaam] VARCHAR(255) NOT NULL,
	[Email] VARCHAR(255) NOT NULL,
	[EigenBedrijf] VARCHAR(255) NOT NULL,
	[Status] INT NOT NULL DEFAULT 1
)

CREATE TABLE [dbo].[Afspraak](
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[StartTijd] DATETIME NOT NULL DEFAULT GETDATE(),
	[EindTijd] DATETIME NULL,
	[WerknemerBedrijfId] INT NOT NULL,
	[AfspraakStatusId] INT NOT NULL DEFAULT 1,
	[BezoekerId] INT NOT NULL,
	CONSTRAINT [FK_Afspraak_WerknemerBedrijf_Id] FOREIGN KEY ([WerknemerBedrijfId]) REFERENCES [dbo].[Werknemerbedrijf](Id),
	CONSTRAINT [FK_Afspraak_AfspraakStatus_Id] FOREIGN KEY ([AfspraakStatusId]) REFERENCES [dbo].[AfspraakStatus](Id),
	CONSTRAINT [FK_Afspraak_Bezoeker_Id] FOREIGN KEY ([BezoekerId]) REFERENCES [dbo].[Bezoeker](Id)
)

