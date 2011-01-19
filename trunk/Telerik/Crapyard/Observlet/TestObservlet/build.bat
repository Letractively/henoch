echo off
cls

if not defined SVN_HOME goto noSVN_HOME
:SVN_HOME

if not defined NANT_HOME goto noNANT_HOME
:NANT_HOME


if not defined NUNIT_HOME goto noNUNIT_HOME
:NUNIT_HOME

md .\bin\Release
copy .\bin\Release\ApplicationTypes.dll .\bin /Y
%NANT_HOME%\NAnt.exe -t:net-3.5 rebuild -buildfile:"default.build" -logger:NAnt.Core.DefaultLogger -logfile:rebuild-DomeinenTest.log

pause
exit

:noNANT_HOME
echo WARNING: environmentvariables not set (noNANT_HOME). 
echo Reinstall with environmental variables or define variables.
set NANT_HOME=%systemroot%\temp\nant\bin
set NANTCONTRIB_HOME=%systemroot%\temp\nant\nantcontrib-0.85\bin
set Path=%Path%;%NANT_HOME%;%NANTCONTRIB_HOME%
echo downloading NANT (if there is no NANT)...
svn export -q -r HEAD "https://hellow0rld.googlecode.com/svn/trunk/OpenSourceTools/Source/nant-0.86-beta1/" %systemroot%\temp\nant
echo using NANT_HOME
echo.
goto NANT_HOME

:noNUNIT_HOME
echo WARNING: environmentvariables not set (noNUNIT_HOME). 
echo Reinstall with environmental variables or define variables.
set NUNIT_HOME=%systemroot%\temp\nunit\bin
set Path=%Path%;%NUNIT_HOME%;%ARCGISHOME%
svn export -q -r HEAD "https://hellow0rld.googlecode.com/svn/trunk/OpenSourceTools/Source/NUnit.2.4.8" %systemroot%\temp\nunit
echo using NUNIT_HOME
echo.
goto NUNIT_HOME

:noSVN_HOME
echo WARNING: environmentvariables not set (noSVN_HOME). 
echo Reinstall with environmental variables or define variables.
set SVN_HOME=%systemroot%\temp\Slik.subversion.client\bin
set Path=%Path%;%SVN_HOME%;
echo downloading svn client from Slik (if there is no svn client)...
svn export -q -r HEAD "https://hellow0rld.googlecode.com/svn/trunk/OpenSourceTools/Source/Slik.subversion.client/" %systemroot%\temp\Slik.subversion.client
echo using SVN_HOME
echo.
goto SVN_HOME

