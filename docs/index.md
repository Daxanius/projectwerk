# **.inside**
Een tool gemaakt door Wout, Bjorn, Stan & Balder onder opdracht van Allphi
binnen het Graduaat Programmeren HoGent.

![Preview](images/inside.jpg)

## De Opdracht
Een bedrijvenpark met een centrale receptie wil een applicatie die de bezoekers van het bedrijvenpark kan registreren. Dit is nodig in kader van de brandveiligheid, op deze manier weet men ten allen tijde wie er aanwezig is op de site. Iedereen die op bezoek komt bij één van de bedrijven moet zich eerst aanmelden aan de receptie. Om dit zo vlug mogelijk te laten verlopen krijgen de bezoekers de mogelijkheid om zichzelf aan te melden via een touchscreen die voorzien wordt aan de inkom. Via het touchscreen kunnen ze hun eigen gegevens en de persoon waarmee ze een afspraak hebben ingeven. Bij het vertrekkunnen de bezoekers zich terug uitschrijven via hetzelfde touchscreen. Wij houden hierbij zeker rekening met de privacy van de bezoekers met betrekking to de GDPR.

## Onze Oplossing
Onze oplossing bied een toegankelijk, makkelijk te onderhouden en uitbreidbaar systeem. Wij bieden een mooie user interface gemaakt met WPF dat in contact komt met onze backend.

Onze backend maakt gebruik van een REST service om toegang te bieden aan andere (sooren) applicaties voor gebruiksgemak. Dit zorgt ervoor dat wij bijvoorbeeld ook gemakkelijk een website kunnen maken voor .inside die API calls maakt naar onze backend.

Daarbovenop heeft het gebruikmaken van een REST een voordeel: het kan centraal runnen. De host van de server kan ook op Linux (bv Ubuntu) draaien zonder probleem, aangezien de server cross platform is.

Jammer genoeg zal de UI (in WPF) op een windows omgeving exclusief moeten draaien.