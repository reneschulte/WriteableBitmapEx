SET Id=WriteableBitmapEx
SET VERSION=1.6.11
..\3rdParty\nuget\nuget setApiKey [APIKEY] -source https://www.nuget.org/api/v2/package
..\3rdParty\nuget\nuget delete %ID% %VERSION%
..\3rdParty\nuget\nuget push ..\Build\nuget\%ID%.%VERSION%.nupkg -source https://www.nuget.org/api/v2/package