param([Boolean]$Launch = $false)

if ($Launch)
{
    Start-Process powershell.exe -Verb RunAs -ArgumentList ('-ExecutionPolicy Bypass -NoExit -Command ". {0}"' -f ($MyInvocation.MyCommand.Definition))
    Exit
}

#Lauch PS in project's folder
$SportifyBaseDir = Split-Path -Path $MyInvocation.MyCommand.Path
$SportifyBaseDir = [System.IO.Path]::GetFullPath("$SportifyBaseDir\..\..").TrimEnd("\\")
Set-Location $SportifyBaseDir

$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"

function Add-Migration
{
    param(
        [Parameter(Position=0,mandatory=$true)]
        [String]$MigrationName
    )

    Install-Ef-Tool-If-Not-Exists
    Set-Location $SportifyBaseDir/Server/DataServices
    dotnet ef migrations add $MigrationName
    Set-Location $SportifyBaseDir
}

function Undo-Migration
{
    Install-Ef-Tool-If-Not-Exists
    Set-Location $SportifyBaseDir/Server/DataServices
    dotnet ef migrations remove
    Set-Location $SportifyBaseDir
}

function Database-Update
{
    Install-Ef-Tool-If-Not-Exists
    Set-Location $SportifyBaseDir/Server/DataServices
    dotnet ef database update 
    Set-Location $SportifyBaseDir
}

function Install-Ef-Tool-If-Not-Exists
{
    $CheckResult = powershell.exe -nologo -noprofile -executionpolicy bypass -command "dotnet-ef"
    if ($LASTEXITCODE -gt 0)
    {
        dotnet tool install --global dotnet-ef
    }
    else 
    {
        Write-Host "dotnet-ef tool already installed" -ForegroundColor Yellow 
    }
}

function Seed-Database
{
    Set-Location ./ConfigFiles/SeedScript
    $CurrentLocation = Get-Location
    Remove-Directory-If-Exists "$CurrentLocation\bin"
    Remove-Directory-If-Exists "$CurrentLocation\obj"
    dotnet run -c Release
    Set-Location $SportifyBaseDir
}

function Remove-Directory-If-Exists
{
    param(
        [Parameter(Position=0,mandatory=$true)]
        [String]$path
    )

    if (Test-Path -Path $path)
    {
        Write-Host "Removing $path" -ForegroundColor Yellow 
        Remove-Item -Path $path -Force -Recurse
    }
}

function Write-Help
{
    Write-Output "*** Local Deployment avaliable commands ***"
    Write-Output "am           (Add-Migration)      Creates migration in DataServices project"
    Write-Output "um           (Undo-Migration)     Removes last migration"
    Write-Output "db-update    (Database-Update)    Updates database from migration in DataServices project"
    Write-Output "seed         (Seed-Database)      Seeds database"
}

Set-Alias am Add-Migration
Set-Alias um Undo-Migration
Set-Alias db-update Database-Update
Set-Alias seed Seed-Database
Write-Help