# Dokumentace - Systém pro správu útulku pro psy
Zde je popsáno jak aplikace funguje a které zdroje byli využity k vývoji.


## 📋 Obsah
1. [Popis projektu](#popis-projektu)
2. [Použité technologie](#použité-technologie)
3. [Struktura projektu](#struktura-projektu)
4. [Funkcionality](#funkcionality)
5. [Autentizace a autorizace](#autentizace-a-autorizace)
6. [Databázové modely](#databázové-modely)
7. [Instalace a spuštění](#instalace-a-spuštění)
8. [Použité zdroje a knihovny](#použité-zdroje-a-knihovny)
9. [Architektura aplikace](#architektura-aplikace)

---

## Popis projektu

Aplikace **Útulek pro psy** je webová aplikace postavená na ASP.NET Core MVC frameworku, která slouží pro komplexní správu útulku pro psy. Aplikace umožňuje správu psů, majitelů, útulků, zdravotních záznamů, skladů, rezervací a dalších entit souvisejících s provozem útulku.

# Použité technologie

### Backend
- **ASP.NET Core 8.0** - Webový framework
- **Entity Framework Core 8.0** - ORM pro práci s databází
- **SQLite 8.0** - Embedded databázový systém
- **C# 12** - Programovací jazyk

### Frontend
- **Bootstrap 5.3.2** - CSS framework pro responzivní design
- **Bootstrap Icons 1.11.1** - Ikony
- **jQuery 3.7.1** - JavaScript knihovna
- **jQuery Validation** - Validace formulářů
- **Razor Pages** - Template engine pro generování HTML

### Bezpečnost
- **Session-based authentication** - Autentizace pomocí session
- **SHA-256** - Hashování hesel
- **Custom Authorization** - Vlastní systém autorizace s úrovněmi oprávnění

---

##  Struktura projektu

```
DogShelterMvc/
├── Attributes/
│   └── AuthorizeAttribute.cs          # Vlastní autorizační atribut
├── Controllers/                        # MVC Controllery
│   ├── AccountController.cs           # Přihlášení, registrace, odhlášení
│   ├── DogsController.cs              # Správa psů
│   ├── OwnersController.cs            # Správa majitelů
│   ├── SheltersController.cs          # Správa útulků
│   ├── AddressesController.cs         # Správa adres
│   ├── MedicalRecordsController.cs     # Zdravotní záznamy
│   ├── DogImagesController.cs         # Obrázky psů
│   ├── LogsController.cs              # Zobrazení logů
│   └── ... (další controllery)
├── Data/
│   └── DogShelterDbContext.cs         # DbContext a konfigurace entit
├── Helpers/
│   ├── PasswordHelper.cs              # Hashování a ověřování hesel
│   ├── PermissionHelper.cs            # Pomocné metody pro kontrolu oprávnění
│   └── LogHelper.cs                   # Pomocné metody pro logování
├── Models/                             # Entity modely
│   ├── Dog.cs
│   ├── Owner.cs
│   ├── Shelter.cs
│   ├── User.cs
│   └── ... (další modely)
├── Views/                              # Razor views
│   ├── Shared/
│   │   └── _Layout.cshtml             # Hlavní layout
│   ├── Home/
│   │   └── Index.cshtml               # Úvodní stránka
│   └── ... (views pro jednotlivé controllery)
├── wwwroot/                            # Statické soubory
│   ├── css/
│   │   └── site.css                   # Vlastní CSS styly
│   └── js/
│       └── site.js                    # JavaScript soubory
├── Program.cs                          # Vstupní bod aplikace
└── DogShelterMvc.csproj               # Projektový soubor
```

---

##  Funkcionality

### Veřejné funkce (bez přihlášení)
-  Prohlížení seznamu psů
-  Zobrazení detailu psa
-  Zobrazení obrázků psů

### Funkce pro přihlášené uživatele (minimální oprávnění >= 1)
- Správa základních entit (Psi, Majitelé, Adresy, Útulky, Pavilony)
-  Správa zdravotních záznamů (Zdravotní záznamy, Procedury, Karantény, Historie psů)
-  Správa skladů a vybavení (Sklady, Krmivo, Hračky, Lékařské vybavení)
-  Správa rezervací
-  Správa obrázků psů
-  Zobrazení logů

### Funkce pro administrátory (oprávnění >= 100)
-  Všechny funkce běžných uživatelů
-  Správa uživatelů (vytváření, úprava, mazání)
-  Úprava oprávnění uživatelů

### CRUD operace
Všechny entity podporují standardní CRUD operace:
- **Create** - Vytvoření nového záznamu
- **Read** - Zobrazení seznamu a detailu
- **Update** - Úprava existujícího záznamu
- **Delete** - Smazání záznamu

### Logování
- Automatické logování všech změn (CREATE, UPDATE, DELETE)
- Uložení staré a nové hodnoty pro UPDATE operace
- Zobrazení uživatele, který změnu provedl
- Časové razítko každé změny

---

##  Autentizace a autorizace

### Autentizace
- **Session-based** - Uživatel se přihlásí pomocí uživatelského jména a hesla
- **Hesla** - Hashování pomocí SHA-256 algoritmu
- **Session data** - Uložení UserId, Username a Perms do session

### Autorizace
- **Role-based** - Systém založený na úrovních oprávnění (Perms)
- **Úrovně oprávnění**:
  - `0` - Nepřihlášený uživatel (pouze prohlížení psů)
  - `>= 1` - Běžný uživatel (správa entit)
  - `>= 10` - Uživatel s vyššími oprávněními (mazání záznamů)
  - `>= 100` - Administrátor (správa uživatelů)

### Custom AuthorizeAttribute
Vlastní autorizační atribut `[Attributes.Authorize]`:
- Kontrola přihlášení uživatele
- Kontrola úrovně oprávnění
- Automatické přesměrování na přihlášení při nedostatečných oprávněních

### Veřejné stránky
- `Home/Index` - Úvodní stránka
- `Dogs/Index` - Seznam psů
- `Dogs/Details` - Detail psa
- `DogImages/Image` - Zobrazení obrázku

---

##  Databázové modely

### Hlavní entity

#### Dog (Pes)
- `Id` - Primární klíč
- `Name` - Jméno psa
- `Age` - Věk
- `BodyColor` - Barva srsti
- `DatumPrijeti` - Datum přijetí
- `DuvodPrijeti` - Důvod přijetí
- `StavPes` - Stav psa
- `UtulekId` - FK na Shelter
- `KarantenaId` - FK na Quarantine
- `MajitelId` - FK na Owner
- `ObrazekId` - FK na DogImage

#### Owner (Majitel)
- `Id` - Primární klíč
- `Name` - Jméno
- `Surname` - Příjmení
- `Phone` - Telefon
- `Email` - Email
- `AddressID` - FK na Address

#### Shelter (Útulek)
- `Id` - Primární klíč
- `Name` - Název útulku
- `Telephone` - Telefon
- `Email` - Email
- `AddressID` - FK na Address

#### User (Uživatel)
- `Id` - Primární klíč
- `Uname` - Uživatelské jméno
- `Hash` - Hash hesla (SHA-256)
- `Perms` - Úroveň oprávnění (ulong)

#### DogImage (Obrázek psa)
- `Id` - Primární klíč
- `FileName` - Název souboru
- `ImageData` - Binární data obrázku (byte[])

#### Log (Log záznam)
- `Id` - Primární klíč
- `CUser` - Uživatel, který změnu provedl
- `EventTime` - Čas změny
- `TableName` - Název tabulky
- `Operation` - Typ operace (CREATE/UPDATE/DELETE)
- `OldValue` - Stará hodnota (JSON)
- `NewValue` - Nová hodnota (JSON)

### Další entity
- **Address** - Adresy
- **Quarantine** - Karantény
- **Pavilion** - Pavilony
- **MedicalRecord** - Zdravotní záznamy
- **Procedure** - Procedury
- **DogHistory** - Historie psů
- **Storage** - Sklady
- **Feed** - Krmivo
- **Hracka** - Hračky
- **MedicalEquipment** - Lékařské vybavení
- **Reservation** - Rezervace

### Vztahy mezi entitami
- **Dog** → **Shelter** (Many-to-One)
- **Dog** → **Owner** (Many-to-One)
- **Dog** → **DogImage** (Many-to-One)
- **Dog** → **Dog** (Otec, Matka - self-reference)
- **Owner** → **Address** (Many-to-One)
- **Shelter** → **Address** (Many-to-One)
- **MedicalRecord** → **Dog** (Many-to-One)
- **MedicalRecord** → **Procedure** (Many-to-One)

---

## Instalace a spuštění

### Požadavky
- **.NET 8.0 SDK** nebo novější
- **Visual Studio 2022** (volitelně) nebo **Visual Studio Code**
- **Webový prohlížeč** (Chrome, Firefox, Edge, atd.)

### Instalace

1. **Klonování nebo stažení projektu**
   ```bash
   # Pokud máte Git
   git clone <repository-url>
   cd bdas_2_dog_shelter-master/DogShelterMvc
   ```

2. **Obnovení NuGet balíčků**
   ```bash
   dotnet restore
   ```

3. **Spuštění aplikace**
   ```bash
   dotnet run
   ```

4. **Otevření v prohlížeči**
   - Aplikace běží na: `https://localhost:5002
   - Přesná adresa se zobrazí v konzoli po spuštění

### První spuštění
- Databáze SQLite (`dogshelter.db`) se automaticky vytvoří při prvním spuštění
- Vytvoří se default admin uživatel:
  - **Uživatelské jméno**: `admin`
  - **Heslo**: `admin`
  - **Oprávnění**: `999` (Administrátor)
  - po vytvoření se dá heslo změnit a přidávat a odebírat uživatele dle potřeby

### Vývojové prostředí
Pro vývoj můžete použít:
- **Visual Studio 2022** - Otevřete `.sln` soubor nebo `.csproj`
- **Visual Studio Code** - S rozšířením C# Dev Kit
- **Rider** - JetBrains IDE

---

## 📚 Použité zdroje a knihovny

### NuGet balíčky

#### Microsoft.EntityFrameworkCore.Sqlite (8.0.0)
- **Popis**: SQLite provider pro Entity Framework Core
- **Odkaz**: https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite
- **Použití**: Databázový provider pro SQLite

#### Microsoft.EntityFrameworkCore.Tools (8.0.0)
- **Popis**: Nástroje pro Entity Framework Core (migrace, scaffolding)
- **Odkaz**: https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools
- **Použití**: Vývojové nástroje pro práci s databází

#### Microsoft.EntityFrameworkCore.Design (8.0.0)
- **Popis**: Design-time komponenty pro Entity Framework Core
- **Odkaz**: https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design
- **Použití**: Podpora pro migrace a scaffolding

#### Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation (8.0.0)
- **Popis**: Runtime kompilace Razor views
- **Odkaz**: https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
- **Použití**: Hot reload Razor views během vývoje

### CDN knihovny (načítány z CDN)

#### Bootstrap 5.3.2
- **Popis**: CSS framework pro responzivní design
- **Odkaz**: https://getbootstrap.com/
- **Verze**: 5.3.2
- **Použití**: Layout, komponenty, responzivní grid

#### Bootstrap Icons 1.11.1
- **Popis**: Ikony pro Bootstrap
- **Odkaz**: https://icons.getbootstrap.com/
- **Verze**: 1.11.1
- **Použití**: Ikony v navigaci, tlačítkách, menu

#### jQuery 3.7.1
- **Popis**: JavaScript knihovna
- **Odkaz**: https://jquery.com/
- **Verze**: 3.7.1
- **Použití**: DOM manipulace, AJAX požadavky

#### jQuery Validation 1.21.0
- **Popis**: Validace formulářů
- **Odkaz**: https://jqueryvalidation.org/
- **Verze**: 1.21.0
- **Použití**: Client-side validace formulářů

#### jQuery Validation Unobtrusive 4.0.0
- **Popis**: Unobtrusive validace pro ASP.NET
- **Odkaz**: https://github.com/aspnet/jquery-validation-unobtrusive
- **Verze**: 4.0.0
- **Použití**: Integrace jQuery Validation s ASP.NET

### Dokumentace a reference

#### ASP.NET Core
- **Odkaz**: https://learn.microsoft.com/en-us/aspnet/core/
- **Dokumentace**: Oficiální dokumentace ASP.NET Core

#### Entity Framework Core
- **Odkaz**: https://learn.microsoft.com/en-us/ef/core/
- **Dokumentace**: Oficiální dokumentace EF Core

#### SQLite
- **Odkaz**: https://www.sqlite.org/
- **Dokumentace**: Oficiální dokumentace SQLite

#### Bootstrap
- **Odkaz**: https://getbootstrap.com/docs/5.3/
- **Dokumentace**: Oficiální dokumentace Bootstrap 5

---

## 🏗️ Architektura aplikace

### MVC Pattern
Aplikace používá **Model-View-Controller** architektonický vzor:

- **Model** - Entity modely v `Models/` složce
- **View** - Razor views v `Views/` složce
- **Controller** - Controllery v `Controllers/` složce

### Vrstvy aplikace

1. **Prezentační vrstva**
   - Razor Views
   - CSS/JavaScript
   - Bootstrap komponenty

2. **Logická vrstva**
   - Controllers
   - Helpers (PasswordHelper, PermissionHelper, LogHelper)
   - Attributes (AuthorizeAttribute)

3. **Datová vrstva**
   - Entity Framework Core
   - DogShelterDbContext
   - SQLite databáze

### Tok dat

```
Uživatel → View → Controller → DbContext → SQLite Database
                ↓
            Helpers (validace, logování)
                ↓
            Response → View → Uživatel
```

### Bezpečnostní vrstvy

1. **Autentizace** - Session-based, kontrola přihlášení
2. **Autorizace** - Role-based, kontrola oprávnění
3. **Hashování hesel** - SHA-256 algoritmus
4. **Logování** - Audit trail všech změn
