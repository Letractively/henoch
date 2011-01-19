echo off
cls

%NANT_HOME%\NAnt.exe -t:net-3.5 build -buildfile:"CreateReport.build" -logger:NAnt.Core.DefaultLogger -logfile:Report-DomeinenTest.log

pause
exit
