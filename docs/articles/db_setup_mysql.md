# Database Setup MySQL
In deze guide leggen we u stapsgewijs uit hoe u een MySQL database server kan opzetten.

## De Software
Vooreerst zal u databank software nodig hebben die op de achtergrond actief zal zijn.

* Installeer [MySQL](https://dev.mysql.com/downloads/installer/)
* Installeer [MySQL Workbench](https://www.mysql.com/products/workbench/)

Deze software zal u gebruiken voor het beheren van uw data. 

Microsoft Express is een database engine die SQL Querries accepteert om zo verschillende database bewerkingen uit te voeren.

MySQL Workbench is een grafische user interface die het eenvoudig maakt om databanken aan te maken en bewerkingen uit te voeren.

## De Databank
Voordat u de structuur van onze databank inlaad, moeten u eerst een databank hebben waar u mee kan gaan werken.

De volgende stappen leggen uit hoe u een databank kunt aanmaken:

1. Start MySQL Workbench op
2. Klik op het plusje om een connectie toe te voegen
3. Geef de connectie gegevens in van de databank en klik op "ok"
4. Login op de databank
5. Klik met rechtermuisknop op "schemas" en selecteer "create schema"
6. Geef de database een naam en klik op "Apply"

:tada: U heeft nu een lege databank aangemaakt waar we mee aan de slag kunnen. Hoera!

## De Structuur
U heeft uw databank aangemaakt, maar de databank bevat nog niet alle
informatie die we nodig hebben om onze server te doen draaien.

Om de structuur aan te maken, bieden wij een SQL script aan dat u zal moeten uitvoeren op de databank. 

1. Selecteer uw nieuw aangemaakte Databank
2. Klik boven in de menubalk op "new query"
3. Paste de code van het `CreateTables.sql` bestand (in de repo) in de query
4. Klik boven in de menubalk op "execute"

:thumbsup: Zie daar! U heeft succesvol uw databank klaargezet voor gebruik.

## Connectie String
Om een connectie met uw databank te leggen zal u een connectie string nodig hebben.
Een connectie string is simpelweg en lijn tekst die zegt hoe uw computer of programma met de databank in
verbinding zal komen.

Om de connectie string te verkrijgen volgt u volgende stappen:

1. Klik met rechtermuisknop op uw Databank in MySQL Workbench
2. Selecteer "Copy Connection String to Clipboard"

## Probleem?
Kom je toevallig de error met dit bericht tegen: `General error: 3065 
Expression #2 of ORDER BY clause is not in SELECT list`, dan kun je [dit](https://stackoverflow.com/questions/36829911/how-to-resolve-order-by-clause-is-not-in-select-list-caused-mysql-5-7-with-sel) artikel volgen om het op te lossen.

:rocket: Gelukt?! Goed dan bent u helemaal klaar voor gebruik!
