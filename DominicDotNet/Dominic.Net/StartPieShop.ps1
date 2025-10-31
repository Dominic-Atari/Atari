# Dominic's Pie Shop Launcher
Write-Host "Starting Dominic's Pie Shop..." -ForegroundColor Green

# Change to the project directory
Set-Location "C:\Users\Dominic\C#.Projects\DominicDotNet\Dominic.Net"

# Start the application in background
Write-Host "Starting server..." -ForegroundColor Yellow
$job = Start-Job -ScriptBlock {
    Set-Location "C:\Users\Dominic\C#.Projects\DominicDotNet\Dominic.Net"
    dotnet run
}

# Wait for the server to be ready
Write-Host "Waiting for server to start..." -ForegroundColor Yellow
Start-Sleep 5

# Test if server is responding
$maxAttempts = 10
$attempt = 0
$serverReady = $false

while ($attempt -lt $maxAttempts -and -not $serverReady) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:5293" -TimeoutSec 2 -ErrorAction Stop
        $serverReady = $true
        Write-Host "Server is ready!" -ForegroundColor Green
    }
    catch {
        $attempt++
        Write-Host "Waiting for server... (attempt $attempt/$maxAttempts)" -ForegroundColor Yellow
        Start-Sleep 1
    }
}

if ($serverReady) {
    # Open the browser to the exact URL
    Write-Host "Opening browser to http://localhost:5293" -ForegroundColor Green
    Start-Process "http://localhost:5293"
    Write-Host "Pie Shop is running! Press Ctrl+C to stop." -ForegroundColor Green
} else {
    Write-Host "Server failed to start properly. Please check for errors." -ForegroundColor Red
}

# Wait for the job to complete or be stopped
try {
    Receive-Job $job -Wait
} finally {
    Stop-Job $job -ErrorAction SilentlyContinue
    Remove-Job $job -ErrorAction SilentlyContinue
}