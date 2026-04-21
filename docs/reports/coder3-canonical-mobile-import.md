# Coder 3 Canonical Mobile Import

## Current step
- `MOB-CANON-IMPORT-02`

## Previous slice PR #67
- ready for review
- stacked development continues locally on top of the current branch
- PR `#67` is still waiting for review/merge or owner override
- this is stacked local work and must not be pushed as a separate PR until the `PR #67` strategy is resolved

## Scope imported in this slice
- Android native video picker
- Android native camera capture
- local selected-media descriptor/cache
- selected local media card on `Upload`
- minimal tests for the selected-media store

## Explicitly not imported yet
- outbox
- selected-media to outbox handoff
- duplicate-precheck
- restart snapshots
- repair/rebind
- backend S1 adapters
- PreUploadCheck
- UploadReceipt

## Source used
- previous backup path: `C:\Users\yarad\source\repos\AndroidA_git_wrong_repo_backup_20260419_151951`
- canonical current project: `C:\Users\yarad\source\repos\AndroidA_core`

## Validation
- `dotnet test .\tests\Unit\App.Mobile.Android.Foundation.Tests\App.Mobile.Android.Foundation.Tests.csproj -c Debug`
  - passed: `14/14`
  - note: informational `NETSDK1057`
- `dotnet build .\src\App.Mobile.Android\App.Mobile.Android.csproj -f net10.0-android -m:1`
  - success
  - warnings: `2`
  - warnings source: existing `src\App.UI.Shared\ExampleJsInterop.cs` analyzer warnings `CA1716` and `CA1816`

## Files changed
- `docs/reports/coder3-canonical-mobile-import.md`
- `docs/reports/coder3-mobile-foundation-reconciliation.md`
- `docs/task-cards/MOB-CANON-IMPORT-02.txt`
- `src/App.Mobile.Android/MauiProgram.cs`
- `src/App.Mobile.Android/Localization/MobileUiText.cs`
- `src/App.Mobile.Android/Media/MobileMediaCapabilityState.cs`
- `src/App.Mobile.Android/Media/MobileMediaSource.cs`
- `src/App.Mobile.Android/Media/MobileMediaCapabilitySnapshot.cs`
- `src/App.Mobile.Android/Media/MobileMediaOperationStatus.cs`
- `src/App.Mobile.Android/Media/MobileMediaOperationResult.cs`
- `src/App.Mobile.Android/Media/LocalSelectedMediaDescriptor.cs`
- `src/App.Mobile.Android/Services/Abstractions/IMobileMediaService.cs`
- `src/App.Mobile.Android/Services/Abstractions/IMobileSelectedMediaStore.cs`
- `src/App.Mobile.Android/Services/Android/AndroidNativeMediaService.cs`
- `src/App.Mobile.Android/Services/Local/InMemoryMobileSelectedMediaStore.cs`
- `src/App.Mobile.Android/Components/Pages/Upload.razor`
- `src/App.Mobile.Android/Components/Pages/Upload.razor.css`
- `src/App.Mobile.Android/Platforms/Android/MainApplication.cs`
- `src/App.Mobile.Android/Platforms/Android/AndroidManifest.xml`
- `tests/Unit/App.Mobile.Android.Foundation.Tests/App.Mobile.Android.Foundation.Tests.csproj`
- `tests/Unit/App.Mobile.Android.Foundation.Tests/InMemoryMobileSelectedMediaStoreTests.cs`

## Test status
- pass, `14/14`

## Build status
- success, `2` existing analyzer warnings in `App.UI.Shared\ExampleJsInterop.cs`

## Runtime blocker
- previous black screen: resolved

## Runtime check
- passed on physical Android for:
  - visible app screen
  - `Upload` screen visible
  - native video picker opens
  - video can be selected

## Runtime status
- video picker ok

## Manual steps pending
- confirm selected local media card after pick/capture
- confirm clear selected media action
- confirm camera capture behavior

## Waiting for coder 1
- no immediate blocker for this import slice
- backend/S1 integration still not started

## Waiting for coder 2
- unchanged
- `App.UI.Shared` remains untouched in this slice

## Next code step
- `MOB-CANON-IMPORT-03 - local outbox foundation reconciliation`, but only after `MOB-CANON-IMPORT-02` manual checks are fully closed and `PR #67` strategy remains controlled
