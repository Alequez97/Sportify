param([Boolean]$Launch = $false)

if ($Launch)
{
    Start-Process powershell.exe -Verb RunAs -ArgumentList ('-ExecutionPolicy Bypass -NoExit -Command ". {0}"' -f ($MyInvocation.MyCommand.Definition))
    Exit
}

#Lauch PS in project's folder
$Workspace = Split-Path -Path $MyInvocation.MyCommand.Path
$Workspace = [System.IO.Path]::GetFullPath("$Workspace\..\..").TrimEnd("\\")
Set-Location $Workspace

$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"

function Add-Migration
{
    param(
        [String]$MigrationName = $(throw 'Migration name parameter is mandatory')
    )

    Install-Ef-Tool-If-Not-Exists
    Set-Location $Workspace/Server/DataServices
    dotnet ef migrations add $MigrationName
    Set-Location ../..
}

function Undo-Migration
{
    Install-Ef-Tool-If-Not-Exists
    Set-Location $Workspace/Server/DataServices
    dotnet ef migrations remove
    Set-Location ../..
}

function Database-Update
{
    Install-Ef-Tool-If-Not-Exists
    Set-Location $Workspace/Server/DataServices
    dotnet ef database update 
    Set-Location ../..
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
    dotnet run -c Release
    Set-Location ../..
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