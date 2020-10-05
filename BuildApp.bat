rem Build Project. Replace paths with your local paths
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\msbuild.exe" /p:Configuration=Release /t:SignAndroidPackage "C:\Users\Amarjeet.Yelwande\source\repos\MyAutoCompleteAndroidApp\AutoComplete\MyCompany.AutoComplete.Xamarin.csproj"

rem If build is successful goto release directory and show contents
if %ERRORLEVEL% == 0 GOTO SHOWRELEASEFOLDERCONTENTS
:RETURN
rem remove below pause in production script
pause

rem If build fails due to any reason pause and analyse error
else pause

rem Show release folder contenst for presence of .APK files
:SHOWRELEASEFOLDERCONTENTS
set releaseDirectory="C:\Users\Amarjeet.Yelwande\source\repos\MyAutoCompleteAndroidApp\AutoComplete\bin\Release"
CD /D %releaseDirectory%
dir *.apk
GOTO RETURN
              