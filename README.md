# Demo app voor Applied Software Project

Dit project demonstreert een eenvoudige Clean Architecture opzet met Domain, Application, Infrastructure en Presentation (Blazor Server), zonder database (in-memory store).

Validatiekaart (waar hoort welke regel?)
- Domain (Entities/ValueObjects): ononderhandelbare domein-invarianten die altijd moeten gelden.
  - City: Name mag niet leeg/whitespace zijn; Population >= 0; CountryId > 0.
  - Doel: objecten kunnen niet in een ongeldige staat bestaan.
- Application (Validators/Use-cases): use‑case-specifieke en infrastructuur-afhankelijke regels.
  - AddCityDtoValidator: Name verplicht en <= 100 tekens; Population 0..50.000.000; CountryId > 0; land moet bestaan; naam moet uniek zijn (via repository).
  - CityDtoValidator (update): zelfde syntactische regels; land moet bestaan.
  - Services roepen validators aan vóór repository calls en vertalen not-found naar NotFoundException.
- Presentation (UI/DataAnnotations): UX-validatie en feedback (zelfde basisregels als Application) en het tonen van validator-/domainfouten.
  - AddCityForm toont ValidationException/NotFoundException berichten aan de gebruiker.

Belangrijke afspraken
- Types: Population is overal int en geharmoniseerd met validatieregels (0..50.000.000).
- Id’s: worden in-memory toegekend (GetNextCityId/GetNextCountryId) bij Add; ook bij bulk-add wordt per item Add aangeroepen.
- Repositories: Update/DeleteById geven bool terug; services vertalen false naar NotFoundException.
- Tests: validators en services zijn afgedekt met unit tests voor zowel happy path als foutpaden.

Projectstructuur (beknopt)
- Domain: Entities (City, Country), Common/IEntity, domein-guards in City.
- Application: DTOs, Interfaces, Services, Validation (FluentValidation), Exceptions/NotFoundException.
- Infrastructure: Data/InMemoryDataStore, Repositories, UOW/UnitOfWork, seeding.
- Presentation: Blazor Components (UI), DI-registratie van services, repositories en validators.

Opmerkingen
- UI-validatie is aanvullend; de waarheid ligt in Domain (invarianten) en Application (validators).
- Zonder database blijft het patroon hetzelfde; uniqueness/exists checks verlopen via repositories (in-memory).
