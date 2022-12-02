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
	[FunctieNaam] VARCHAR(50) NOT NULL UNIQUE
)

CREATE TABLE [dbo].[AfspraakStatus](
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[AfspraakStatusNaam] VARCHAR(50) NOT NULL UNIQUE
)

CREATE TABLE [dbo].[Bedrijf](
	[Id] BIGINT NOT NULL IDENTITY PRIMARY KEY,
	[Naam] VARCHAR(255) NOT NULL,
	[BTWNr] VARCHAR(30) NOT NULL,
	[TeleNr] VARCHAR(30) NOT NULL,
	[Email] VARCHAR(255) NOT NULL,
	[Adres] VARCHAR(255) NOT NULL,
	[BTWChecked] BIT NOT NULL,
	[Status] INT NULL DEFAULT 1,
	CONSTRAINT [UN_BTW_Status] UNIQUE(BTWNr, Status)
)

CREATE TABLE [dbo].[Werknemer](
	[Id] BIGINT NOT NULL IDENTITY PRIMARY KEY,
	[ANaam] VARCHAR(255) NOT NULL,
	[VNaam] VARCHAR(255) NOT NULL	
)

CREATE TABLE [dbo].[Werknemerbedrijf](
	[Id] BIGINT NOT NULL IDENTITY PRIMARY KEY,
	[BedrijfId] BIGINT NOT NULL,
	[WerknemerId] BIGINT NOT NULL,
	[FunctieId] INT NOT NULL,
	[WerknemerEmail] VARCHAR(255) NOT NULL,
	[Status] INT NULL DEFAULT 1,
	CONSTRAINT [FK_WerknemerBedrijf_Bedrijf_Id] FOREIGN KEY ([BedrijfId]) REFERENCES [dbo].[Bedrijf](Id),
	CONSTRAINT [FK_WerknemerBedrijf_Werknemer_Id] FOREIGN KEY ([WerknemerId]) REFERENCES [dbo].[Werknemer](Id),
	CONSTRAINT [FK_Werknemer_Functie_Id] FOREIGN KEY ([FunctieId]) REFERENCES [dbo].[Functie](Id),
	CONSTRAINT [UN_BId_WId_FId_WMail_Status] UNIQUE(BedrijfId, WerknemerId, FunctieId, WerknemerEmail, Status)
)

CREATE TABLE [dbo].[Bezoeker](
	[Id] BIGINT NOT NULL IDENTITY PRIMARY KEY,
	[ANaam] VARCHAR(255) NOT NULL,
	[VNaam] VARCHAR(255) NOT NULL,
	[Email] VARCHAR(255) NOT NULL,
	[EigenBedrijf] VARCHAR(255) NULL
)

CREATE TABLE [dbo].[Afspraak](
	[Id] BIGINT NOT NULL IDENTITY PRIMARY KEY,
	[StartTijd] DATETIME NOT NULL DEFAULT GETDATE(),
	[EindTijd] DATETIME NULL,
	[WerknemerBedrijfId] BIGINT NOT NULL,
	[AfspraakStatusId] INT NOT NULL DEFAULT 1,
	[BezoekerId] BIGINT NOT NULL,
	CONSTRAINT [FK_Afspraak_WerknemerBedrijf_Id] FOREIGN KEY ([WerknemerBedrijfId]) REFERENCES [dbo].[Werknemerbedrijf](Id),
	CONSTRAINT [FK_Afspraak_AfspraakStatus_Id] FOREIGN KEY ([AfspraakStatusId]) REFERENCES [dbo].[AfspraakStatus](Id),
	CONSTRAINT [FK_Afspraak_Bezoeker_Id] FOREIGN KEY ([BezoekerId]) REFERENCES [dbo].[Bezoeker](Id)
)

CREATE TABLE [dbo].[ParkingContract](
	[Id] BIGINT NOT NULL IDENTITY PRIMARY KEY,
	[StartTijd] DATE NOT NULL,
	[EindTijd] DATE NOT NULL,
	[BedrijfId] BIGINT NOT NULL,
	[AantalPlaatsen] INT NOT NULL,
	[StatusId] INT NULL DEFAULT 1,
	CONSTRAINT [FK_ParkingContract_Bedrijf_Id] FOREIGN KEY ([BedrijfId]) REFERENCES [dbo].[Bedrijf](Id),
	CONSTRAINT [UN_Start_Eind_Bedrijf] UNIQUE ([StartTijd], [EindTijd], [BedrijfId], [StatusId])
)

CREATE TABLE [dbo].[ParkingPlaatsen](
	[NummerPlaat] VARCHAR(20) NOT NULL,
	[StartTijd] DATETIME NOT NULL,
	[EindTijd] DATETIME NULL,
	[BedrijfId] BIGINT NOT NULL,
	[StatusId] INT NULL DEFAULT 1,
	CONSTRAINT [FK_ParkingPlaatsen_Bedrijf_Id] FOREIGN KEY ([BedrijfId]) REFERENCES [dbo].[Bedrijf](Id),
	CONSTRAINT [UN_NummerPlaat_Start] UNIQUE ([NummerPlaat], [StartTijd])
)