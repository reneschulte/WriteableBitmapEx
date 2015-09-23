SET Id=WriteableBitmapEx
SET VERSION=1.5.0.0
..\3rdParty\nuget\nuget delete %ID% %VERSION%
..\3rdParty\nuget\nuget push ..\Build\nuget\%ID%.%VERSION%.nupkg