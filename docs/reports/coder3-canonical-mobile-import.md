# Coder 3 Canonical Mobile Import

## Current step
- MOB-CANON-IMPORT-03 PR-ready replay from fresh main.

## Base and coordination state
- Base main SHA at task start: `0015c7d`
- PR #95 is merged into `main`.
- Coordination log source: GitHub issue `#89`, not a repo file.
- Latest coordination entries reviewed:
  - `#90` S2-37 Admin Review API AuthZ Scope
  - `#91` S2-39 Deploy Trigger Reconciliation
  - `#92` S2-40 Coordination Readiness and ChatGPT Sync Readiness
  - `#94` S2-42 Formalize Manual-only Korobochka Deploy Policy
- Handoff/task cards reviewed:
  - `docs/handoffs/s1-11-coder3-android-integration.md`
  - `docs/handoffs/s1-11-backend-integration-note.md`
  - `docs/handoffs/s1-12-sprint1-quality-gate.md`
  - `docs/task-cards/MOB-CANON-IMPORT-02.txt`
  - `docs/task-cards/MOB-CANON-IMPORT-03.txt`

## Scope restored in this PR slice
- Local in-memory outbox foundation.
- PendingSyncItem local models.
- IMobileOutboxService.
- StubMobileOutboxService.
- Upload local enqueue stub action.
- Queue local item rendering.
- Retry/remove local actions.
- Minimal App.Mobile.Android foundation tests for the local outbox service.

## Explicitly not included
- selected-media to outbox handoff
- duplicate-precheck
- restart snapshots
- repair/rebind
- report draft
- backend S1 adapters
- PreUploadCheck
- UploadReceipt
- sync
- download
- direct upload runtime
- App.UI.Shared changes
- shared DTO/contracts changes
- SQLite/local DB
- persistence

## Source used
- IMPORT-03 source commit: `ed37abf feat(mobile): restore local outbox foundation`
- PR-ready replay branch base: fresh `main` at `0015c7d`
- canonical current project: `C:\Users\yarad\source\repos\AndroidA_core`

## Validation plan
- `dotnet test .\tests\Unit\App.Mobile.Android.Foundation.Tests\App.Mobile.Android.Foundation.Tests.csproj -c Debug`
- `dotnet build .\src\App.Mobile.Android\App.Mobile.Android.csproj -f net10.0-android -m:1`
- `dotnet format whitespace .\AnalyticsAutomation-Core.sln --verify-no-changes --no-restore`

## Manual steps pending
- Physical Android runtime check for Upload enqueue action and Queue retry/remove behavior.
- Confirm Upload route can enqueue a local stub outbox item.
- Confirm Queue route renders local outbox items.
- Confirm retry local action updates local state.
- Confirm remove local action removes the local item.

## Waiting for coder 1
- No immediate blocker for this import slice.
- Backend/S1 integration is not started in this PR slice.

## Waiting for coder 2
- No App.UI.Shared changes are included in this PR slice.

## Next code step
- Do not start MOB-CANON-IMPORT-04 until this PR-ready branch has phone-check evidence and owner approval.
