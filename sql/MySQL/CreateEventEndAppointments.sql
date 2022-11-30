SET GLOBAL event_scheduler = ON;

CREATE EVENT IF NOT EXIST event_BeeindigAfsprakenSysteem
ON SCHEDULE EVERY 6 HOUR
STARTS CURRENT_TIMESTAMP
ON COMPLETION PRESERVE
DO
  UPDATE Groupswork.Afspraak
  SET AfspraakStatusId = 4,
  EindTijd = DATE_ADD(CONVERT(CONVERT(NOW(), DATE), DATETIME), INTERVAL -1 SECOND)
  WHERE AfspraakStatusId = 1 AND CONVERT(StartTijd, DATE) < CONVERT(NOW(), DATE);