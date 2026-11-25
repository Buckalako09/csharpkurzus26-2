# WorkoutTracker – Edzéstervkezelő alkalmazás

## Leírás

A WorkoutTracker egy C# nyelven készült konzolos alkalmazás, amely gyakorlatok és edzéstervek kezelésére szolgál. A felhasználó saját gyakorlatokat hozhat létre, ezeket edzéstervekhez rendelheti, valamint nyilvántarthatja a teljesített edzéseit.

## Funkciók

- Gyakorlatok hozzáadása, törlése és listázása
- Gyakorlatok keresése és szűrése izomcsoport alapján
- Edzéstervek létrehozása és törlése
- Gyakorlatok hozzárendelése edzéstervekhez
- Edzéstervek és a hozzájuk tartozó gyakorlatok megtekintése
- Edzések teljesítettnek jelölése
- Teljesített edzések előzményeinek megtekintése
- Statisztikák megjelenítése

## Felhasznált technológiák

A program C# nyelven, .NET 10 keretrendszerrel készült.

A gyakorlatok, edzéstervek és teljesített edzések tárolásához generikus listákat (`List<T>`) használ. A gyakorlatokat egy `record` típus reprezentálja.

A keresési, rendezési és statisztikai műveletek LINQ segítségével működnek. A program interface-eket használ a különböző szolgáltatások elkülönítésére, a hibás műveleteket pedig kivételkezeléssel kezeli.

## Adatok mentése

Az alkalmazás az adatokat JSON-formátumban menti és tölti be. A mentést a `JsonDataRepository` osztály végzi.

A program újraindításakor a korábban elmentett adatok visszatölthetők.

## Futtatás

A projekt Visual Studióban a `WorkoutTracker.slnx` vagy a `WorkoutTracker.csproj` fájl megnyitásával indítható.

A program konzolos menürendszert használ, amelyben a kívánt művelet számának megadásával lehet választani.