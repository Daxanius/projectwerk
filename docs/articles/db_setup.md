# Database Setup
In deze guide leggen we u stapsgewijs uit hoe u een database server kan opzetten.

## De Software
Vooreerst zal u databank software nodig hebben die op de achtergrond actief zal zijn.

* Installeer [Microsoft Express](https://www.microsoft.com/en-us/Download/details.aspx?id=101064)
* Installeer [Microsoft SQL Server Managment Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)

Deze software zal u gebruiken voor het beheren van uw data. 

Microsoft Express is een database engine die T-SQL Querries accepteert om zo verschillende database bewerkingen uit te voeren.

Microsoft SQL Server Managment Studio is een grafische user interface die het eenvoudig maakt om databanken aan te maken en bewerkingen uit te voeren.

## De Databank
Voordat u de structuur van onze databank inlaad, moeten u eerst een databank hebben waar u mee kan gaan werken.

De volgende stappen leggen uit hoe u een databank kunt aanmaken:

1. Start Microsoft SQL Server Managment Studio op
2. Klik op "connect"
3. Klik met rechtermuisknop op de Databases folder
4. Klik op "new database" 
5. Geef de database een naam en klik op OK

U heeft nu een lege databank aangemaakt waarmee u kan werken. Hoera :tada:!

## De Structuur
U heeft uw databank aangemaakt, maar de databank bevat nog niet alle
informatie die we nodig hebben om onze server te doen draaien.

Om de structuur aan te maken, bieden wij een SQL script aan dat u zal moeten uitvoeren op de databank. 

1. Selecteer uw nieuw aangemaakte Databank
2. Klik boven in de menubalk op "new query"
3. Paste de code van het `CreateTables.sql` bestand (in de repo) in de query
4. Klik boven in de menubalk op "execute"

Zie daar! U heeft succesvol uw databank klaargezet voor gebruik :thumbsup:.

## Connectie String
Om een connectie met uw databank te leggen zal u een connectie string nodig hebben.
Een connectie string is simpelweg en lijn tekst die zegt hoe uw computer of programma met de databank in
verbinding zal komen.

Om de connectie string te verkrijgen volgt u volgende stappen:

1. Klik met rechtermuisknop op uw Databank in Microsoft SQL Server Managment Studio
2. Selecteer "properties"
3. Kopieëer de ConnectionString property

Gelukt?! Goed dan bent u helemaal klaar voor gebruik!
