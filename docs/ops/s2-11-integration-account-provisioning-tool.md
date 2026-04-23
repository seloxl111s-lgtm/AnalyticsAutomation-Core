# S2-11 Integration Account Provisioning Tool

Status: Implemented
Owner: Coder 1 / Platform Owner
Module: App.Maintenance / Auth / Korobochka / Ops
Type: maintenance tool + runbook
Goal: safely create or rotate the dedicated integration account for Coder 2 and Coder 3 on Korobochka.
Decision source: S2-10 runbook.
Contracts: no shared contract changes.
Migration: none.
Feature flag: none.
Security: no passwords, password hashes, access tokens, or refresh tokens in repo, chat, docs, or logs.
Production deploy: not included.

## Scope and guardrails

- use a repo-tracked maintenance tool instead of `DevelopmentBootstrap`;
- keep the normal auth boundary intact;
- do not add hidden auth bypasses;
- do not add API endpoints;
- do not change DTOs, shared contracts, public routes, or migrations;
- do not store the integration password in repo files;
- do not print the password, password hash, `accessToken`, or `refreshToken`.

## Dedicated account defaults

The tool provisions a single dedicated account with these fixed values:

- login: `integration-web-android`;
- display name: `Web/Android Integration Account`;
- role: `platform_owner`;
- group node: `root`;
- active: `true`.

If the account already exists, the tool updates the password hash, display name, `is_active`, and `current_group_node_id`.
If the `platform_owner` link is missing, the tool adds it.
The tool does not create a new role and does not change permissions.

## Configuration source

`App.Maintenance` reuses the same database key shape as `App.Api`:

- `ConnectionStrings:MainDatabase`;
- `Database:Host`;
- `Database:Port`;
- `Database:Database`;
- `Database:Username`;
- `Database:PasswordFilePath`.

Configuration is loaded in this order:

1. `src\App.Api\appsettings.json`
2. `src\App.Api\appsettings.{DOTNET_ENVIRONMENT|ASPNETCORE_ENVIRONMENT}.json` if present
3. process environment variables

If the tool cannot locate `src\App.Api\appsettings.json` automatically, set `AA_APP_API_SETTINGS_DIR` to the directory that contains that file before running the command.

## Run on Korobochka

Open PowerShell in the repository root on Korobochka:

```powershell
Set-Location C:\Codex\AnalyticsAutomation-Core
```

Provide the password through an environment variable without saving it in the repo:

```powershell
$securePassword = Read-Host 'Integration account password' -AsSecureString
$env:AA_INTEGRATION_ACCOUNT_PASSWORD = [System.Net.NetworkCredential]::new('', $securePassword).Password
```

Run the maintenance command:

```powershell
dotnet run --project tools\App.Maintenance\App.Maintenance.csproj -- integration-account upsert
```

Expected safe output:

```text
login: integration-web-android
status: created
role: platform_owner
group-node: root
```

or:

```text
login: integration-web-android
status: updated
role: platform_owner
group-node: root
```

Clear the temporary environment variable after the command:

```powershell
Remove-Item Env:AA_INTEGRATION_ACCOUNT_PASSWORD
```

## Verify sign-in without printing secrets

Keep the same PowerShell session only long enough to verify sign-in.
Do not echo `$env:AA_INTEGRATION_ACCOUNT_PASSWORD`.
Do not print the sign-in response object.
Do not copy `accessToken` or `refreshToken` into chat, docs, screenshots, or logs.

Example verification flow:

```powershell
$baseUrl = 'http://<korobochka-app-api-url>'
$deviceId = [guid]::NewGuid()

$signInBody = @{
    login = 'integration-web-android'
    password = $env:AA_INTEGRATION_ACCOUNT_PASSWORD
    deviceId = $deviceId
} | ConvertTo-Json

$signInResponse = Invoke-RestMethod `
    -Method Post `
    -Uri "$baseUrl/api/auth/sign-in" `
    -ContentType 'application/json' `
    -Body $signInBody

if ([string]::IsNullOrWhiteSpace($signInResponse.accessToken)) {
    throw 'Sign-in failed: access token was not returned.'
}

$authorizedStatus = (Invoke-WebRequest `
    -Method Get `
    -Uri "$baseUrl/api/group-tree/nodes" `
    -Headers @{ Authorization = "Bearer $($signInResponse.accessToken)" }).StatusCode

$anonymousStatus = $null
try {
    Invoke-WebRequest -Method Get -Uri "$baseUrl/api/group-tree/nodes" | Out-Null
    $anonymousStatus = 200
}
catch {
    if ($null -eq $_.Exception.Response) {
        throw
    }

    $anonymousStatus = [int]$_.Exception.Response.StatusCode
}

"Authorized status: $authorizedStatus"
"Anonymous status: $anonymousStatus"

Remove-Variable signInResponse -ErrorAction SilentlyContinue
```

Expected verification result:

- authenticated `GET /api/group-tree/nodes` returns `200`;
- anonymous `GET /api/group-tree/nodes` remains `401`.

## Rotation

To rotate the password, repeat the same `integration-account upsert` command with a new value in `AA_INTEGRATION_ACCOUNT_PASSWORD`.
The tool overwrites the stored password hash and keeps the account pinned to `platform_owner` and `root`.

## Rollback

Reset:

- run `integration-account upsert` again with a newly chosen password distributed out-of-band.

Disable:

- connect to the same PostgreSQL database on Korobochka;
- run:

```sql
update app.auth_users
set is_active = false
where normalized_login = 'INTEGRATION-WEB-ANDROID';
```

- verify that `POST /api/auth/sign-in` now fails for the integration account.

The disable step does not print the password or tokens and does not require any API changes.
