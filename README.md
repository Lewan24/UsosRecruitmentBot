# USOS Recruitment Bot

Simple automation tool built with C# and Playwright for processing large numbers of recruitment applications in the USOS system.

The application was created to solve a real operational problem during the annual recruitment process. Instead of manually opening and accepting hundreds of applications, the bot performs the repetitive actions automatically.

## Features

* Automatic login to USOS
* Automatic navigation to the applications management page
* Automatic filter configuration
* Automatic loading of all available applications
* Sequential processing of applications
* Logging using `Microsoft.Extensions.Logging`
* Configuration through environment variables

## Technology Stack

* .NET 10
* C#
* Microsoft Playwright
* Microsoft.Extensions.Hosting
* Microsoft.Extensions.Options
* Microsoft.Extensions.Logging

## Requirements

* .NET SDK 10.0 or newer
* Playwright browsers installed

Install Playwright browsers:

```bash
playwright install
```

or

```bash
pwsh bin/Debug/net10.0/playwright.ps1 install
```

## Configuration

Credentials should be provided through environment variables.

### Windows

```powershell
$env:USOS__Username="your_username"
$env:USOS__Password="your_password"
```

### Linux

```bash
export USOS__Username="your_username"
export USOS__Password="your_password"
```

## Running

```bash
dotnet run
```

The bot will:

1. Open a Chromium browser.
2. Navigate to the USOS applications page.
3. Authenticate.
4. Configure application filters.
5. Load all available items.
6. Process every application with the "przyjmij do rozpatrzenia" action.
7. Finish when no more applications are available.

## Project Structure

```text
src/
├── Configuration/
├── Constants/
├── Playwright/
├── Services/
└── Program.cs
```

### Main Components

| Component               | Responsibility                                      |
| ----------------------- | --------------------------------------------------- |
| `UsosBot`               | Orchestrates the entire process                     |
| `UsosLoginService`      | Handles authentication                              |
| `ApplicationsProcessor` | Processes recruitment applications                  |
| `BrowserFactory`        | Creates and configures Playwright browser instances |
| `UsosCredentials`       | Stores application credentials                      |

## Logging

The application uses standard .NET logging and writes logs to the console.

Example:

```text
info: Launching browser...
info: User not authenticated. Logging in...
info: Found 248 applications.
info: Processing application...
info: Finished successfully.
```

## Disclaimer

This tool was created for internal use during the annual recruitment process and targets a specific USOS workflow. Selectors, URLs and business rules may require adjustments if the USOS interface changes in the future.
