# BankApp


# Bank System - ASP.NET Core Banklösning

## Introduktion

Detta projekt är en självständig ASP.NET Core-lösning för banken, utvecklad för att hantera kunder, deras konton med saldo samt transaktioner. Lösningen är uppbyggd enligt OOP-principer med tydlig separation mellan databasentiteter och ViewModel.

## Funktionaliteter

- **Hem sida (Startsida)**  
  Visar statistik: antal kunder, antal konton och den totala summan av saldon på alla konton. Denna sida är publik och kräver ingen autentisering.

- **Kundbild**  
  Sök kund via kundnummer. Kundbilden visar all kundinformation samt en lista över kundens konton. Totalbeloppet för kundens konton summeras och visas.

- **Kundsökning**  
  Sök efter kund på namn och stad. Sökresultatet visar kundnummer, personnummer, namn, adress och stad. Resultatet är paginerat med 50 poster per sida.

- **Kontobild**  
  När du klickar på ett kontonummer i kundbilden visas en sida med kontoinformation (kontonummer och saldo) samt transaktioner i fallande datumordning. Vid fler än 20 transaktioner laddas ytterligare 20 poster med JavaScript vid knapptryckning.

- **Transaktioner**  
  Systemet hanterar insättningar, uttag och överföringar. Saldo ändras endast via en transaktion (inga direkta ändringar). Systemet kontrollerar att uttag eller överföringar inte överstiger kontots saldo.

- **ASP.NET Core Identity**  
  Roller skapas via seed vid uppstart:
  - **Admin**: Kan administrera användare (minst en tom vy med korrekt behörighet för G-krav).
  - **Cashier**: Kan hantera kunder och deras konton.
  
  Två användare seedas:
  - **richard.chalk@systementor.se** (Admin, lösenord: Hejsan123#)
  - **richard.erdos.chalk@gmail.se** (Cashier, lösenord: Hejsan123#)



## Teknologier & Verktyg

- **Backend:** ASP.NET Core, Entity Framework Core (Database First)
- **Frontend:** ASP.NET Core Razor Pages, Bootstrap
- **Autentisering:** ASP.NET Core Identity
- **Övrigt:** AutoMapper, inbyggd inputvalidering enligt best practices, OOP med tydliga ViewModels
- **Databas:** SQL server 
  **Viktigt:** Efter nedladdning, kör följande SQL-syntax på databasen:
  ```sql
  ALTER AUTHORIZATION ON DATABASE::[database-name] TO [sa]
