/*
DUMMY DATA FOR GROUPSWORK IN TSQL
*/

INSERT INTO Functie
	(FunctieNaam)
	VALUES('CEO'),
		  ('CFO'),
		  ('Sanitair medewerker'),
		  ('Administratief medewerker'),
		  ('Logistiek'),
		  ('Sociale media beinvloeder')



INSERT INTO AfspraakStatus
	(AfspraakStatusNaam)
	VALUES('In gang'),
		  ('Verwijderd'),
		  ('Stopgezet door gebruiker'),
		  ('Stopgezet door systeem'),
		  ('Stopgezet door administratief medewerker')

INSERT INTO Bezoeker
	(Vnaam, ANaam, Email, EigenBedrijf)
	VALUES('Bart', 'De Pauw', 'Diddler@Outlook.com', 'Diddler NV'),
		  ('Bart', 'Niet De Pauw', 'NietDiddler@Outlook.com', 'NietDiddler NV'),
		  ('Lars', 'Mars', 'ISh1dd3dMyPant@Outlook.com', 'Dominos')

INSERT INTO Werknemer
	(VNaam, ANaam)
	VALUES('Jan', 'Cornelis'),
		  ('Piet', 'Comelis'),
		  ('Joris', 'Conelis')

INSERT INTO Bedrijf
	(Naam, BTWNr, TeleNr, Email, Adres)
	VALUES('Alternate','0893031290', '03 871 11 11', 'info@alternate.be', 'Oeyvaersbosch 16-18 , 2630 Aartselaar, Belgie'),
		  ('Bol.com bv', '0824148721', '032 027 885 999', 'info@bol.be', 'Papendorpseweg 100 , 3528BJ Utrecht, Belgie')


INSERT INTO WerknemerBedrijf
	(BedrijfId, WerknemerId, FunctieId,WerknemerEmail)
	VALUES(1,1,1,'JanCornelis@Alternate.com'),
		  (1,2,2,'PietComelis@Alternate.com'),
		  (2,3,1,'JorisConelis@Bol.com'),
		  (2,3,2,'JorisConelis@Bol.com'),
		  (1,3,1,'JorisConelis@Alternate.com')

INSERT INTO Afspraak
	(StartTijd, WerknemerBedrijfId, BezoekerId)
	VALUES(GETDATE(), 1, 1),
		  (GETDATE(), 2, 2),
		  (GETDATE(), 3, 1)