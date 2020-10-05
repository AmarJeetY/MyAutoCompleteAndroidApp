"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\msbuild.exe" /p:Configuration=Release /t:SignAndroidPackage "C:\Users\Amarjeet.Yelwande\source\repos\NumberManipulator\AutoComplete\MyCompany.AutoComplete.Xamarin.csproj"

if %ERRORLEVEL% == 0 GOTO RUN
:AFTER
pause

else pause

:RUN
set releaseDirectory="C:\Users\Amarjeet.Yelwande\source\repos\NumberManipulator\AutoComplete\bin\Release"
CD /D %releaseDirectory%
dir *.apk
GOTO AFTER
              