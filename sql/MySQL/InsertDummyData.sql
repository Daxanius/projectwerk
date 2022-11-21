/*
DUMMY DATA FOR GROUPSWORK FOR MYSQL
*/

INSERT INTO Groupswork.Functie
	(FunctieNaam)
	VALUES('Ceo'),
		  ('Cfo'),
		  ('Sanitair medewerker'),
		  ('Administratief medewerker'),
		  ('Logistiek'),
		  ('Sociale media beinvloeder');

INSERT INTO Groupswork.AfspraakStatus
	(AfspraakStatusNaam)
	VALUES('In gang'),
		  ('Verwijderd'),
		  ('Stopgezet door gebruiker'),
		  ('Stopgezet door systeem'),
		  ('Stopgezet door administratief medewerker');

INSERT INTO Groupswork.Bezoeker
	(Vnaam, ANaam, Email, EigenBedrijf)
	VALUES('Bart', 'Smis', 'BartSmis@Outlook.com', 'Smisses NV'),
		  ('Niet', 'Geert', 'NietGeert@Outlook.com', 'NietGeert NV'),
		  ('David', 'Brex', 'Davidbrex@Outlook.com', 'Dominos');

INSERT INTO Groupswork.Werknemer
	(VNaam, ANaam)
	VALUES('Ruby','Tucker'),
	      ('Sharon','Cunningham'),
		  ('Mary','Walsh'),
		  ('Blanca','Buchanan'),
		  ('Elsa','Davidson'),
		  ('Kelley','Cook'),
		  ('Gretchen','Mendez'),
		  ('Velma','Schwartz'),
		  ('Johnny','Obrien'),
		  ('Rosalie','Vargas'),
		  ('Gilberto','Briggs'),
		  ('Lorenzo','Estrada'),
		  ('Mamie','Black'),
		  ('Ken','Graham'),
		  ('Tami','Bell'),
		  ('Dawn','Hodges'),
		  ('Ted','Simpson'),
		  ('Candice','Bridges'),
		  ('Lela','Curry'),
	  	  ('Cheryl','Lawson'),
		  ('Sophie','Knight'),
		  ('Samuel','Fitzgerald'),
		  ('Hugh','Hicks'),
		  ('Anthony','Bryant'),
		  ('Fred','Parks'),
		  ('Clifton','Frazier'),
		  ('Shelia','Coleman'),
		  ('Bradford','Page'),
		  ('Jeffrey','Foster'),
		  ('Tony','Matthews'),
		  ('Pearl','Robertson'),
		  ('Opal','Meyer'),
		  ('Perry','Sharp'),
		  ('Hilda','Munoz'),
		  ('Orville','Parsons'),
		  ('George','Hughes'),
		  ('Misty','Garrett'),
		  ('Marcella','Lawrence'),
		  ('Caleb','Scott'),
		  ('Johnnie','Daniel'),
		  ('Virgil','Jimenez'),
		  ('Agnes','Fox'),
		  ('Phillip','Mcdonald'),
		  ('Ray','Morales'),
		  ('Boyd','Smith'),
		  ('Ron','Brown'),
		  ('Melanie','Mccoy'),
		  ('Mae','Turner'),
		  ('Genevieve','Mcdaniel'),
		  ('Laurence','Stephens'),
		  ('Sam','Jenkins'),
		  ('Cory','Phelps'),
		  ('Colin','Evans'),
		  ('Margaret','Mcbride'),
		  ('Sonja','Lloyd'),
		  ('Samantha','Rogers'),
		  ('Christian','Park'),
		  ('Alexandra','Medina'),
		  ('Henry','Underwood'),
		  ('Anita','Maxwell'),
		  ('Katie','White'),
		  ('Susie','Clarke'),
		  ('Angela','Floyd'),
		  ('Caroline','Hernandez'),
		  ('Manuel','Cohen'),
		  ('Laurie','Hines'),
		  ('Bryant','Montgomery'),
		  ('Brandy','Palmer'),
		  ('Lawrence','Hawkins'),
		  ('Estelle','Clayton'),
		  ('Wayne','Ball'),
		  ('Dolores','Klein'),
		  ('Betty','Hill'),
		  ('Edward','King'),
		  ('Priscilla','Erickson'),
		  ('Steven','Price'),
		  ('Oliver','Moran'),
		  ('Ross','Leonard'),
		  ('Jasmine','Carpenter'),
		  ('Victor','Horton'),
		  ('Teresa','Barton'),
		  ('Trevor','Richards'),
		  ('Holly','Roberts'),
		  ('Mildred','Hart'),
		  ('Randolph','Day'),
		  ('Marlene','Owens'),
		  ('Tabitha','Adams'),
		  ('Emilio','Anderson'),
		  ('Juana','Larson'),
		  ('Edwin','Alvarez'),
		  ('Veronica','Franklin'),
		  ('Stewart','Fowler'),
		  ('Rebecca','Webster'),
		  ('Maggie','Ross'),
		  ('Darnell','Martin'),
		  ('Vicky','Hubbard'),
		  ('Annette','Herrera'),
		  ('Kyle','Gardner'),
		  ('Benjamin','Oliver'),
		  ('Jeanette','Reynolds');


INSERT INTO Groupswork.Bedrijf
	(Naam, BTWNr, TeleNr, Email, Adres, BTWChecked)
  VALUES('allphi','BE0838576480','093961130','info@allphi.be','guldensporenpark 24 9820 merelbeke', 1),
		('orbid','BE0463174208','092729911','info@orbid.be','guldensporenpark 29 9820 merelbeke', 1),
		('cerence','BE0476308008','092398000','info@cerence.com','guldensporenpark 32 9820 merelbeke', 1),
		('bistro insieme','BE0506713746','0489243921','bistro@insieme.eu','guldensporenpark 31 9820 merelbeke', 1),
		('thermo fisher','BE0449564217','092725599','info@thermofisher.com','guldensporenpark 26 9820 merelbeke', 1),
		('cadmes','BE0652874237','092222323','info@cadmes.com','guldensporenpark 12 9820 merelbeke', 1),
		('hybrid software','BE0839161252','093295753','info-eur@hybridsoftware.com','guldensporenpark 18 9820 merelbeke', 1),
		('doubleVerify','BE0423956118','0483631624','advertisersupport@doubleverify.com','guldensporenpark 2 9820 merelbeke', 1),
		('infor','BE0456494767','092361636','middle.east@infor.com','guldensporenpark 78 9820 merelbeke', 1),
		('eastman','BE0859910443','18003278626','taminco@eastman.be','guldensporenpark 74 9820 merelbeke', 1),
		('santander','BE0445641853','092355000','santander@consumerfinance.be','guldensporenpark 81 9820 merelbeke', 1),
		('xplore','BE0865300673','038719966','info@appfoundry.be','guldensporenpark 88 9820 merelbeke', 1),
		('axxes','BE0877961252','033034404','info@axxessid.com','guldensporenlaan 2 9820 merelbeke', 1),
		('linak','NL801383572B01','092300109','sales@linak.cn','guldensporenpark 31 9820 merelbeke', 1),
		('walters people','BE0874633459','092105740','gent@walterspeople.com','guldensporenpark 25 9820 merelbeke', 1);


INSERT INTO Groupswork.WerknemerBedrijf(BedrijfId, WerknemerId, FunctieId,WerknemerEmail)
VALUES(14, 1, 2, 'RubyTucker@linak.cn'),
(3, 1, 6, 'RubyTucker@cerence.com'),
(2, 2, 4, 'SharonCunningham@orbid.be'),
(11, 3, 1, 'MaryWalsh@consumerfinance.be'),
(13, 4, 6, 'BlancaBuchanan@axxessid.com'),
(6, 5, 5, 'ElsaDavidson@cadmes.com'),
(3, 6, 2, 'KelleyCook@cerence.com'),
(8, 7, 1, 'GretchenMendez@doubleverify.com'),
(11, 8, 4, 'VelmaSchwartz@consumerfinance.be'),
(1, 9, 6, 'JohnnyObrien@allphi.be'),
(6, 10, 2, 'RosalieVargas@cadmes.com'),
(5, 11, 1, 'GilbertoBriggs@thermofisher.com'),
(15, 12, 3, 'LorenzoEstrada@walterspeople.com'),
(14, 13, 4, 'MamieBlack@linak.cn'),
(4, 14, 5, 'KenGraham@insieme.eu'),
(7, 15, 2, 'TamiBell@hybridsoftware.com'),
(7, 16, 3, 'DawnHodges@hybridsoftware.com'),
(6, 17, 5, 'TedSimpson@cadmes.com'),
(10, 18, 3, 'CandiceBridges@eastman.be'),
(7, 19, 5, 'LelaCurry@hybridsoftware.com'),
(2, 20, 6, 'CherylLawson@orbid.be'),
(5, 21, 5, 'SophieKnight@thermofisher.com'),
(11, 22, 5, 'SamuelFitzgerald@consumerfinance.be'),
(1, 23, 6, 'HughHicks@allphi.be'),
(4, 24, 5, 'AnthonyBryant@insieme.eu'),
(6, 25, 4, 'FredParks@cadmes.com'),
(4, 26, 6, 'CliftonFrazier@insieme.eu'),
(12, 27, 4, 'SheliaColeman@appfoundry.be'),
(6, 28, 5, 'BradfordPage@cadmes.com'),
(15, 29, 5, 'JeffreyFoster@walterspeople.com'),
(9, 30, 3, 'TonyMatthews@infor.com'),
(8, 31, 3, 'PearlRobertson@doubleverify.com'),
(8, 32, 1, 'OpalMeyer@doubleverify.com'),
(15, 33, 2, 'PerrySharp@walterspeople.com'),
(10, 34, 6, 'HildaMunoz@eastman.be'),
(14, 35, 5, 'OrvilleParsons@linak.cn'),
(10, 36, 3, 'GeorgeHughes@eastman.be'),
(7, 37, 5, 'MistyGarrett@hybridsoftware.com'),
(1, 38, 5, 'MarcellaLawrence@allphi.be'),
(12, 39, 3, 'CalebScott@appfoundry.be'),
(15, 40, 5, 'JohnnieDaniel@walterspeople.com'),
(10, 41, 5, 'VirgilJimenez@eastman.be'),
(2, 42, 4, 'AgnesFox@orbid.be'),
(13, 43, 6, 'PhillipMcdonald@axxessid.com'),
(5, 44, 3, 'RayMorales@thermofisher.com'),
(8, 45, 1, 'BoydSmith@doubleverify.com'),
(7, 46, 4, 'RonBrown@hybridsoftware.com'),
(8, 47, 5, 'MelanieMccoy@doubleverify.com'),
(7, 48, 6, 'MaeTurner@hybridsoftware.com'),
(4, 49, 6, 'GenevieveMcdaniel@insieme.eu'),
(15, 50, 2, 'LaurenceStephens@walterspeople.com'),
(2, 51, 5, 'SamJenkins@orbid.be'),
(2, 51, 4, 'SamJenkins@orbid.be'),
(12, 52, 5, 'CoryPhelps@appfoundry.be'),
(14, 53, 5, 'ColinEvans@linak.cn'),
(8, 54, 2, 'MargaretMcbride@doubleverify.com'),
(11, 55, 1, 'SonjaLloyd@consumerfinance.be'),
(3, 56, 2, 'SamanthaRogers@cerence.com'),
(9, 57, 6, 'ChristianPark@infor.com'),
(13, 58, 5, 'AlexandraMedina@axxessid.com'),
(5, 59, 5, 'HenryUnderwood@thermofisher.com'),
(11, 60, 4, 'AnitaMaxwell@consumerfinance.be'),
(4, 61, 2, 'KatieWhite@insieme.eu'),
(3, 62, 3, 'SusieClarke@cerence.com'),
(14, 63, 4, 'AngelaFloyd@linak.cn'),
(13, 64, 3, 'CarolineHernandez@axxessid.com'),
(5, 65, 6, 'ManuelCohen@thermofisher.com'),
(3, 66, 6, 'LaurieHines@cerence.com'),
(2, 67, 5, 'BryantMontgomery@orbid.be'),
(5, 68, 4, 'BrandyPalmer@thermofisher.com'),
(2, 69, 5, 'LawrenceHawkins@orbid.be'),
(6, 70, 1, 'EstelleClayton@cadmes.com'),
(14, 71, 2, 'WayneBall@linak.cn'),
(9, 72, 1, 'DoloresKlein@infor.com'),
(12, 73, 2, 'BettyHill@appfoundry.be'),
(10, 74, 3, 'EdwardKing@eastman.be'),
(14, 75, 4, 'PriscillaErickson@linak.cn'),
(1, 76, 2, 'StevenPrice@allphi.be'),
(5, 77, 4, 'OliverMoran@thermofisher.com'),
(3, 78, 1, 'RossLeonard@cerence.com'),
(2, 78, 1, 'RossLeonard@orbid.be'),
(10, 79, 2, 'JasmineCarpenter@eastman.be'),
(9, 80, 1, 'VictorHorton@infor.com'),
(6, 81, 6, 'TeresaBarton@cadmes.com'),
(11, 82, 6, 'TrevorRichards@consumerfinance.be'),
(4, 83, 3, 'HollyRoberts@insieme.eu'),
(8, 84, 6, 'MildredHart@doubleverify.com'),
(1, 85, 6, 'RandolphDay@allphi.be'),
(2, 86, 2, 'MarleneOwens@orbid.be'),
(3, 87, 2, 'TabithaAdams@cerence.com'),
(1, 88, 1, 'EmilioAnderson@allphi.be'),
(14, 89, 5, 'JuanaLarson@linak.cn'),
(10, 90, 6, 'EdwinAlvarez@eastman.be'),
(6, 91, 3, 'VeronicaFranklin@cadmes.com'),
(14, 92, 5, 'StewartFowler@linak.cn'),
(9, 93, 6, 'RebeccaWebster@infor.com'),
(8, 94, 3, 'MaggieRoss@doubleverify.com'),
(10, 95, 6, 'DarnellMartin@eastman.be'),
(4, 96, 1, 'VickyHubbard@insieme.eu'),
(14, 97, 5, 'AnnetteHerrera@linak.cn'),
(14, 98, 5, 'KyleGardner@linak.cn'),
(6, 99, 1, 'BenjaminOliver@cadmes.com'),
(5, 100, 4, 'JeanetteReynolds@thermofisher.com');

INSERT INTO Groupswork.Afspraak
	(StartTijd, WerknemerBedrijfId, BezoekerId)
	VALUES(NOW(), 1, 1),
		  (NOW(), 30, 2),
		  (NOW(), 3, 3);


/*Voegt een parkingcontract met 50 plaatsen toe aan een bedrijf die status 1 heeft*/
DROP PROCEDURE IF EXISTS Groupswork.GenerateParkeerPlaatsen;

DELIMITER $$
CREATE procedure Groupswork.GenerateParkeerPlaatsen()
BEGIN
DECLARE i INT;
DECLARE AantalBedrijven INT;
SET AantalBedrijven = (SELECT COUNT(*) FROM Groupswork.Bedrijf WHERE Status = 1);
SET i = 0;
  label: 
WHILE (i < AantalBedrijven) DO
    	INSERT INTO Groupswork.ParkingContract(StartTijd, EindTijd, BedrijfId, AantalPlaatsen) 
			VALUES(CONVERT(NOW(),date), 
			DATE_ADD(CONVERT(NOW(), date), INTERVAL 1 YEAR), 
			(SELECT id
			FROM Groupswork.Bedrijf
			WHERE STATUS = 1
			ORDER BY Id
			LIMIT i, 1), 
			50);
    SET i = i + 1;
  END
WHILE label;
END; $$
DELIMITER ;

CALL Groupswork.GenerateParkeerPlaatsen;
