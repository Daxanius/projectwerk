# Server Setup
In deze guide leggen wij u uit hoe u de server voor .inside kunt opzetten. De server biedt een centrale
plek voor de data en controles aan voor de applicatie.

:warning: We gaan hier verder op de database setup guide.

## Omgevingsvariabelen
Om de server te runnen moet de server uw database connectie string meekrijgen. Deze hebben wij verkregen
in [deze](db_setup_express.md) guide. 

Om de database technologie te specifieren, stellen we de omgevingsvariabele `BRS_DATABASE` in. 
Het instellen van omgevingsvariabelen:

1. Ga naar de Windows zoekbalk en tik in "omgevingsvariabelen"
2. Selecteer "De omgevingsvariabelen van het systeem bewerken"
3. Klik onderaan op "omgevingsvariabelen"
4. Klik op "nieuw"
5. Geef het de naam `BRS_DATABASE` met de waarde van uw database type.

In het geval van MySQL wil u `MySQL` instellen, in het geval van express wil u dit als `MSServer` instellen.

Om de connection string aan onze server mee te geven, gaan wij een omgevingsvariabele instellen.

1. Maak een nieuwe omgevings variabele aan
2. Geef het de naam `BRS_CONNECTION_STRING_[Database Type hier]` met de waarde van uw connectie string

De haakjes vervang je door de database technologie dat je gebruikt bijvoorbeeld bij Express gebruik je `BRS_CONNECTION_STRING_MSSERVER`.

:trophy: De omgevingsvariabele is succesvol ingesteld!

## Configuratie
Als u onze applicatie heeft gedownload ziet u een Appsettings.json bestand. In dit bestand 
kunt u bepaalde configuratie instellingen aanpassen zoals het logniveau.

## Starten
Dubbelklik op het .exe bestand voor de server. Als u een console ziet met groene tekst dan
weet u dat het werkt. Er kunnen nu clients API requests naar de server gemaakt worden. 

De volgende stap is om uw eerste [client op te zetten](client_setup.md).
