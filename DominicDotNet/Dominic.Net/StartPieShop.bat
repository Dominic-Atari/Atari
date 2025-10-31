@echo off
echo Starting Dominic's Pie Shop...
cd /d "C:\Users\Dominic\C#.Projects\DominicDotNet\Dominic.Net"

echo Starting the server...
start /B dotnet run

echo Waiting for server to start...
timeout /t 5 /nobreak >nul

echo Opening browser...
start http://localhost:5293

echo Pie Shop is running! Press Ctrl+C to stop the server.
pause