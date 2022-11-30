/*Tables*/
DROP TABLE IF EXISTS Groupswork.Afspraak;
DROP TABLE IF EXISTS Groupswork.Bezoeker;
DROP TABLE IF EXISTS Groupswork.Werknemerbedrijf;
DROP TABLE IF EXISTS Groupswork.Werknemer;
DROP TABLE IF EXISTS Groupswork.ParkingContract;
DROP TABLE IF EXISTS Groupswork.ParkingPlaatsen;
DROP TABLE IF EXISTS Groupswork.Bedrijf;
DROP TABLE IF EXISTS Groupswork.AfspraakStatus;
DROP TABLE IF EXISTS Groupswork.Functie;

/*Procedures*/
/*Drops Procedure that adds parking contracts to all companies, current time + a year, everyone gets 50 spots*/
DROP PROCEDURE IF EXISTS Groupswork.GenerateParkeerPlaatsen;

/*Drops Procedure that adds 500 visitors and appointments*/
DROP PROCEDURE IF EXISTS Groupswork.GenerateBulkAppointments;

/*Drops Events if they exist*/
DROP EVENT IF EXISTS Groupswork.event_BeeindigAfsprakenSysteem;
SET GLOBAL event_scheduler = OFF;