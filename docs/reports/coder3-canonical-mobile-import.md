# Coder 3 Canonical Mobile Import

## Current step
- MOB-CANON-REPORT-00-03 local report-first baseline replay.

## Main baseline
- IMPORT-01..07 are in main.
- BACKEND-01 unresolved business-object binding guard is in main.
- Latest main SHA reviewed for this replay: `6a7acd4`.
- TEAM COORDINATION LOG #89 records the Android `businessObjectKey` decision:
  https://github.com/uVormik/AnalyticsAutomation-Core/issues/89#issuecomment-4321720298

## Completed Android media/outbox foundation
- IMPORT-01 shell/navigation
- IMPORT-02 media picker/cache
- IMPORT-03 local outbox
- IMPORT-04 selected media to outbox draft
- IMPORT-05 local duplicate-precheck
- IMPORT-06 restart snapshots
- IMPORT-07 repair/rebind
- BACKEND-01 unresolved business-object binding guard before production `PreUploadCheck`

## Scope restored in this PR slice
- Local report-first direction docs.
- Local FPV report draft shell.
- Media selection/capture inside report draft.
- Duplicate-safe attachment behavior inside draft.
- Local report draft queue action.

## Explicitly not included
- production `PreUploadCheck`
- `UploadReceipt`
- backend create-report
- approved concrete `businessObjectKey` source
- auth/session/device binding
- direct upload runtime
- download runtime
- production reports engine
- report field editing beyond the local baseline
- report validation
- report persistence
- lookup/filter/profile UX
- incident creation
- App.UI.Shared changes
- shared DTO/contracts changes
- App.Api/backend changes
- Modules changes
- DB migrations
- workflows/deploy changes

## BusinessObject decision status
- Production direction is Option 1: Android must obtain `businessObjectKey` from a backend-controlled report/business-object binding source before `PreUploadCheck`.
- The concrete backend source/endpoint/contract remains undocumented.
- Local report draft ids are not `businessObjectKey`.
- Production `PreUploadCheck` runtime remains blocked.
- This slice is Android-local report/business-object UX replay only.

## Validation plan
- `dotnet test .\tests\Unit\App.Mobile.Android.Foundation.Tests\App.Mobile.Android.Foundation.Tests.csproj -c Debug`
- `dotnet build .\src\App.Mobile.Android\App.Mobile.Android.csproj -f net10.0-android -m:1`
- `dotnet format whitespace .\AnalyticsAutomation-Core.sln --verify-no-changes --no-restore`
- `git diff --check`

## Manual steps pending
- Physical Android check for local report baseline:
  - `Полеты` route/list works.
  - create FPV draft works.
  - report draft opens.
  - media can be selected/captured inside draft.
  - duplicate same-video attachment is blocked.
  - draft can be placed into local queue.
  - no fake `businessObjectKey` is shown.
  - production `PreUploadCheck` remains blocked.

## Next code step
- Owner decision dependent after this baseline:
  - continue local report UX replay, or
  - wait for backend concrete `businessObjectKey` source.
