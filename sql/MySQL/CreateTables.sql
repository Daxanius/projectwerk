/*
CREATES DATABASE if it doesn't exist

CREATE TABLES FOR GROUPSWORK IN MYSQL
NOTE: DEFAULT NOW() IN AFSPRAAK STARTDATE
MIGHT WANT TO CHANGE LENGTHS ON CERTAIN VARCHARS

FOR Bedrijf, Werknemerbedrijf, Bezoeker, Afspraak
STATUS 1 == VALID (DEFAULT)
STATUS 2 == INVALID
*/
CREATE DATABASE IF NOT EXISTS Groupswork;

CREATE TABLE IF NOT EXISTS Groupswork.Functie(
	Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	FunctieNaam VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS Groupswork.AfspraakStatus(
	Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	AfspraakStatusNaam VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS Groupswork.Bedrijf(
	Id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	Naam VARCHAR(255) NOT NULL,
	BTWNr VARCHAR(30) NOT NULL,
	TeleNr VARCHAR(30) NOT NULL,
	Email VARCHAR(255) NOT NULL,
	Adres VARCHAR(255) NOT NULL,
	BTWChecked BIT NOT NULL,
	Status INT NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Groupswork.Werknemer(
	Id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	ANaam VARCHAR(255) NOT NULL,
	VNaam VARCHAR(255) NOT NULL	
);

CREATE TABLE IF NOT EXISTS Groupswork.Werknemerbedrijf(
	Id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	BedrijfId BIGINT NOT NULL,
	WerknemerId BIGINT NOT NULL,
	FunctieId INT NOT NULL,
	WerknemerEmail VARCHAR(255) NOT NULL,
	Status INT NOT NULL DEFAULT 1,
	CONSTRAINT FK_WerknemerBedrijf_Bedrijf_Id FOREIGN KEY (BedrijfId) REFERENCES Groupswork.Bedrijf(Id),
	CONSTRAINT FK_WerknemerBedrijf_Werknemer_Id FOREIGN KEY (WerknemerId) REFERENCES Groupswork.Werknemer(Id),
	CONSTRAINT FK_Werknemer_Functie_Id FOREIGN KEY (FunctieId) REFERENCES Groupswork.Functie(Id)
);

CREATE TABLE IF NOT EXISTS Groupswork.Bezoeker(
	Id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	ANaam VARCHAR(255) NOT NULL,
	VNaam VARCHAR(255) NOT NULL,
	Email VARCHAR(255) NOT NULL,
	EigenBedrijf VARCHAR(255) NULL
);

CREATE TABLE IF NOT EXISTS Groupswork.Afspraak(
	Id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	StartTijd DATETIME NOT NULL DEFAULT NOW(),
	EindTijd DATETIME NULL,
	WerknemerBedrijfId BIGINT NOT NULL,
	AfspraakStatusId INT NOT NULL DEFAULT 1,
	BezoekerId BIGINT NOT NULL,
	CONSTRAINT FK_Afspraak_WerknemerBedrijf_Id FOREIGN KEY (WerknemerBedrijfId) REFERENCES Groupswork.Werknemerbedrijf(Id),
	CONSTRAINT FK_Afspraak_AfspraakStatus_Id FOREIGN KEY (AfspraakStatusId) REFERENCES Groupswork.AfspraakStatus(Id),
	CONSTRAINT FK_Afspraak_Bezoeker_Id FOREIGN KEY (BezoekerId) REFERENCES Groupswork.Bezoeker(Id)
);

CREATE TABLE IF NOT EXISTS Groupswork.ParkingContract(
	StartTijd DATE NOT NULL,
	EindTijd DATE NOT NULL,
	BedrijfId BIGINT NOT NULL,
	AantalPlaatsen INT NOT NULL,
	CONSTRAINT FK_ParkingContract_Bedrijf_Id FOREIGN KEY (BedrijfId) REFERENCES Groupswork.Bedrijf(Id),
	CONSTRAINT UN_Start_Eind_Bedrijf UNIQUE (StartTijd, EindTijd, BedrijfId)
);

CREATE TABLE IF NOT EXISTS Groupswork.ParkingPlaatsen(
	NummerPlaat VARCHAR(20) NOT NULL,
	StartTijd DATETIME NOT NULL,
	EindTijd DATETIME NULL,
	BedrijfId BIGINT NOT NULL,
	CONSTRAINT FK_ParkingPlaatsen_Bedrijf_Id FOREIGN KEY (BedrijfId) REFERENCES Groupswork.Bedrijf(Id),
	CONSTRAINT UN_NummerPlaat_Start UNIQUE (NummerPlaat, StartTijd)
);