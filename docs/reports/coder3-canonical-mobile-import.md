# Coder 3 Canonical Mobile Import

## Current step
- `MOB-CANON-IMPORT-01`

## Scope imported in this slice
- shell/navigation/menu baseline
- Russian UI strings
- local shell mode/options/access/flag stubs
- shared placeholders only
- unit tests for shell/navigation/state seams

## Explicitly not imported yet
- Android native picker/camera
- selected media store
- outbox
- duplicate-precheck
- restart-resilience snapshots
- repair/rebind
- backend S1 adapters
- PreUploadCheck
- UploadReceipt

## Source used
- previous backup path: `C:\Users\yarad\source\repos\AndroidA_git_wrong_repo_backup_20260419_151951`
- canonical current project: `C:\Users\yarad\source\repos\AndroidA_core`

## Validation
- `dotnet test .\tests\Unit\App.Mobile.Android.Foundation.Tests\App.Mobile.Android.Foundation.Tests.csproj -c Debug`
  - passed: `11/11`
  - note: informational `NETSDK1057`
- `dotnet build .\src\App.Mobile.Android\App.Mobile.Android.csproj -f net10.0-android -m:1`
  - success
  - warnings: `2`
  - warnings source: existing `src\App.UI.Shared\ExampleJsInterop.cs` analyzer warnings `CA1716` and `CA1816`
- `dotnet sln .\AnalyticsAutomation-Core.sln list`
  - solution includes `tests\Unit\App.Mobile.Android.Foundation.Tests\App.Mobile.Android.Foundation.Tests.csproj`
- `git status --short`
  - canonical mobile/import slice changes present in docs, `App.Mobile.Android`, `App.UI.Shared`, tests, and `AnalyticsAutomation-Core.sln`

## Files changed
- `AnalyticsAutomation-Core.sln`
- `docs/reports/coder3-canonical-mobile-import.md`
- `docs/reports/coder3-mobile-foundation-reconciliation.md`
- `docs/task-cards/MOB-CANON-IMPORT-01.txt`
- `src/App.Mobile.Android/MauiProgram.cs`
- `src/App.Mobile.Android/Components/_Imports.razor`
- `src/App.Mobile.Android/Components/Layout/MainLayout.razor`
- `src/App.Mobile.Android/Components/Layout/MainLayout.razor.css`
- `src/App.Mobile.Android/Components/Layout/NavMenu.razor`
- `src/App.Mobile.Android/Components/Layout/NavMenu.razor.css`
- `src/App.Mobile.Android/Components/Pages/Home.razor`
- `src/App.Mobile.Android/Components/Pages/Upload.razor`
- `src/App.Mobile.Android/Components/Pages/Queue.razor`
- `src/App.Mobile.Android/Components/Pages/NotFound.razor`
- `src/App.Mobile.Android/Localization/MobileUiText.cs`
- `src/App.Mobile.Android/Navigation/MobileViewId.cs`
- `src/App.Mobile.Android/Navigation/MobileMenuEntry.cs`
- `src/App.Mobile.Android/Navigation/MobileViewRegistry.cs`
- `src/App.Mobile.Android/Navigation/MobileNavigationState.cs`
- `src/App.Mobile.Android/Options/MobileShellOptions.cs`
- `src/App.Mobile.Android/Options/MobileShellOptionsValidator.cs`
- `src/App.Mobile.Android/State/MobileShellMode.cs`
- `src/App.Mobile.Android/Services/Abstractions/IFeatureFlagReader.cs`
- `src/App.Mobile.Android/Services/Abstractions/IMobileAccessContext.cs`
- `src/App.Mobile.Android/Services/Stubs/StubFeatureFlagReader.cs`
- `src/App.Mobile.Android/Services/Stubs/StubMobileAccessContext.cs`
- `src/App.UI.Shared/_Imports.razor`
- `src/App.UI.Shared/Components/Common/EmptyState.razor`
- `src/App.UI.Shared/Localization/SharedUiText.cs`
- `src/App.UI.Shared/Pages/HomePlaceholder.razor`
- `src/App.UI.Shared/Pages/UploadPlaceholder.razor`
- `src/App.UI.Shared/Pages/QueuePlaceholder.razor`
- `src/App.UI.Shared/wwwroot/shared.css`
- `tests/Unit/App.Mobile.Android.Foundation.Tests/App.Mobile.Android.Foundation.Tests.csproj`
- `tests/Unit/App.Mobile.Android.Foundation.Tests/MobileShellOptionsValidatorTests.cs`
- `tests/Unit/App.Mobile.Android.Foundation.Tests/MobileNavigationStateTests.cs`
- `tests/Unit/App.Mobile.Android.Foundation.Tests/MobileViewRegistryTests.cs`
- `tests/Unit/App.Mobile.Android.Foundation.Tests/StubFeatureFlagReaderTests.cs`
- `tests/Unit/App.Mobile.Android.Foundation.Tests/StubMobileAccessContextTests.cs`

## Test status
- pass, `11/11`

## Build status
- success, `2` existing analyzer warnings in `App.UI.Shared\ExampleJsInterop.cs`

## Solution status
- canonical solution now includes the minimal mobile foundation unit test project

## Manual steps pending
- none for this slice

## Runtime check
- passed on physical Android

## Runtime status
- shell import ok

## Runtime note
- minor visual differences from the previous local foundation were observed and accepted as non-blocking for this slice
- this slice restores only the canonical shell/navigation/Russian baseline
- media/outbox/restart/repair features are intentionally not imported yet
- visual polish and alignment belong to a later dedicated slice only if needed

## Waiting for coder 1
- no immediate blocker for this import slice
- S1 integration later follows the real handoff docs already present in canonical repo

## Waiting for coder 2
- review later for App.UI.Shared placeholder/shared UI boundary

## Next code step
- `MOB-CANON-IMPORT-02 - Android media picker/camera + selected-media cache reconciliation, after this import slice is committed/pushed/PR-ready`
