# Database Setup
In deze guide leggen we u stapsgewijs uit hoe u een database server kan opzetten.

## De Software
Vooreerst zal u databank software nodig hebben die op de achtergrond actief zal zijn.

* Installeer [Microsoft Express](https://www.microsoft.com/en-us/Download/details.aspx?id=101064)
* Installeer [Microsoft SQL Server Managment Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)

Deze twee programmas zal u gebruiken om de database te beheren. 

Microsoft Express is een database engine die T-SQL Querries accepteert om verscheidene database bewerkingen uit te voeren.

Microsoft SQL Server Managment Studio is een grafische user interface dat het u gemakkelijk maakt om databanken aan te maken en bewerkingen uit te voeren.

## De Databank
Voordat wij de structuur van onze databank kunnen inladen, moeten wij eerst een databank hebben waar wij mee kunnen werken.

De volgende stappen leggen uit hoe u een databank kunt aanmaken:

1. Start Microsoft SQL Server Managment Studio op
2. Klik op "connect"
3. Klik met rechtermuisknop op de Databases folder
4. Klik op "new database" 
5. Geef de database een naam en klik op OK

U heeft nu een lege databank aangemaakt waarmee u kan werken. Hoera!

## De Structuur
We hebben onze databank aangemaakt, maar de databank bevat nog niet alle
tabellen en informatie dat we nodig hebben om onze server te laten draaien.

Om de structuur aan te maken, bieden wij een SQL script aan dat u zal moeten uitvoeren op de databank. 

1. Selecteer uw nieuw aangemaakte Databank
2. Klik boven in de menubalk op "new query"
3. Paste de code van het `CreateTables.sql` bestand (in de repo) in de query
4. Klik boven in de menubalk op "execute"

Success! U heeft nu uw databank klaargezet voor gebruik.

## Connection String
Om een connectie met uw databank aan te leggen zal u iets nodig hebben dat heet een connection string.
Een connection string is simpelweg en lijn tekst dat zegt hoe uw computer of programma met een databank in
verbinding moet komen.

Om de connection string te verkrijgen volgt u de volgende stappen:

1. Klik met rechtermuisknop op uw Databank in Microsoft SQL Server Managment Studio
2. Selecteer "properties"
3. Kopieëer de ConnectionString property

U heeft nu uw connection string!
