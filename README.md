# GeoCatch
Small school project for practicing EF-Core Code First Aproach. All SQL CRUD operations runs async.
 
## Installation 
Specify [bing-maps API key](https://docs.microsoft.com/en-us/bingmaps/getting-started/bing-maps-dev-center-help/getting-a-bing-maps-key) & SQL-Connection string inside [App.Congig.](Geocaching/App.config)

Package Manager Console:
```sh
PM> add-migration Initial

PM> Update-Database
```
Import [sample database data](SampleDbData.txt) using the GUIs "Load From File"-button.


## Layout
![Alt Text](https://github.com/FkLaagom/GeoCatch/blob/master/MD/Example.png)

## Syfte
>"Syftet med detta projektarbete är att ni ska bli vana vid att använda Entity Framework Core genom att använda det i en okänd miljö. 
>Istället för att använda det tillsammans med konsolen eller ASP.NET kommer ni att >utveckla en GUI-app med WPF
>
>Applikationen som ni ska ta fram är en interaktiv karta för personer som ägnar sig åt geocaching.
>Geocaching är en form av "kurragömma" som går ut på att hitta dolda föremål enbart med hjälp av GPS-koordinater.
>Applikationen ska visa upp geocaches på en karta samt personerna som har lagt ut och letat upp dem.
>
>Den databasdesign som er applikation ska använda sig av är förutbestämd och måste följas; med andra ord måste ni använda EF Core på ett >sådant sätt att databasen som >skapas vid migrering matchar den förutbestämda. Följande diagram visar denna förutbestämda databasdesign:"

![Alt Text](https://github.com/FkLaagom/GeoCatch/blob/master/MD/Databasdesign.png)
