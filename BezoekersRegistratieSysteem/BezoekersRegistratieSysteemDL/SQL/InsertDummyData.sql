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
		('walters people','BE0874633459','092105740','gent@walterspeople.com','guldensporenpark 25 9820 merelbeke', 1)


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