SET OUTDIR=..\Build\nuget
SET INDIR=..\Build\Release
mkdir %OUTDIR%
copy /Y %INDIR%\* %OUTDIR%
..\3rdParty\nuget\nuget pack -outputdirectory %OUTDIR%