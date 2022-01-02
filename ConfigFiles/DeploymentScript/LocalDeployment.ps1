param([Boolean]$Launch = $false)

if ($Launch)
{
    Start-Process powershell.exe -Verb RunAs -ArgumentList ('-ExecutionPolicy Bypass -NoExit -Command ". {0}"' -f ($MyInvocation.MyCommand.Definition))
    Exit
}

# Lauch PS in project's folder
$SportifyBaseDir = Split-Path -Path $MyInvocation.MyCommand.Path
$SportifyBaseDir = [System.IO.Path]::GetFullPath("$SportifyBaseDir\..\..").TrimEnd("\\")
Set-Location $SportifyBaseDir

# Connection strings
$localDbConnectionStringTemplate = "Server=(localdb)\mssqllocaldb;Database={0};Trusted_Connection=True"

# Actions
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
    dotnet ef --startup-project ../SportifyWebApi migrations add $MigrationName
    Set-Location $SportifyBaseDir
}

function Undo-Migration
{
    Install-Ef-Tool-If-Not-Exists
    Set-Location $SportifyBaseDir/Server/DataServices
    dotnet ef migrations remove
    Set-Location $SportifyBaseDir
}

function Update-Database
{
    Install-Ef-Tool-If-Not-Exists
    Set-Location $SportifyBaseDir/Server/DataServices
    dotnet ef --startup-project ../SportifyWebApi database update 
    Set-Location $SportifyBaseDir
}

function Setup-Database([Switch]$Force)
{
    $moduleIsInstalled = Get-Module -ListAvailable -Name "sqlserver"
    if (!$moduleIsInstalled)
    {
        Write-Host "SqlServer module needs to be installed" -ForegroundColor yellow
        Install-Module "sqlserver"
    } 
    
    Import-Module "sqlserver"
    $masterConnectionString = [string]::Format($localDbConnectionStringTemplate, "master")
    $sportifyDbConnectionString = [string]::Format($localDbConnectionStringTemplate, "sportify_db")

    $masterDatabase = Get-SqlDatabase -ConnectionString $masterConnectionString
    $sportifyDb = Get-SqlDatabase -ConnectionString $sportifyDbConnectionString

    if ($null -eq $sportifyDb)
    {
        Write-Host "Setting up new database..."
        Update-Database
        Seed-Database
        Write-Host "Done!!!" -ForegroundColor Green
        return
    }

    if ($null -ne $sportifyDb -and $Force)
    {
        Write-Host "Processing old database delete..."
        $masterDatabase.ExecuteNonQuery("ALTER DATABASE sportify_db SET SINGLE_USER WITH ROLLBACK IMMEDIATE")
        $masterDatabase.ExecuteNonQuery("ALTER DATABASE sportify_db SET MULTI_USER WITH ROLLBACK IMMEDIATE")
        $sportifyDb.Drop()
        Write-Host "Setting up new database..."
        Update-Database
        Seed-Database
        Write-Host "Done!!!" -ForegroundColor Green
        return
    }
   
    Write-Error "Database already exists. Use -Force flag to reset database"
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
    Write-Output "setup-db     (Setup-Database)     Setup blank database. -Force to recreate if db exists"
    Write-Output "update-db    (Update-Database)    Applies all migration changes if exists"
    Write-Output "seed         (Seed-Database)      Seeds database"
    Write-Output "help         (Write-Help)         Writes this helpdesk"
}

Set-Alias am Add-Migration
Set-Alias um Undo-Migration
Set-Alias setup-db Setup-Database
Set-Alias update-db Update-Database
Set-Alias seed Seed-Database
Set-Alias help Write-Help
Write-Help