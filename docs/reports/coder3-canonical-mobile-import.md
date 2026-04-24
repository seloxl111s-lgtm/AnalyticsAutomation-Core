# Coder 3 Canonical Mobile Import

## Current step
- MOB-CANON-IMPORT-04 PR-ready replay from fresh main.

## Base and coordination state
- Base main SHA at task start: `1a63bf2`
- PR #96 is merged into `main`.
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
  - `docs/task-cards/MOB-CANON-IMPORT-03.txt`
  - `docs/task-cards/MOB-CANON-IMPORT-04.txt`

## Scope restored in this PR slice
- Selected-media to outbox local handoff.
- LocalSelectedMediaStoreEntry.
- PendingSyncItemLocalMediaDraft.
- TakeCurrentAsync on selected-media store.
- Selected-media-aware outbox enqueue.
- Queue media-linked draft details.
- Unit tests for selected-media handoff and outbox behavior.

## Explicitly not included
- duplicate-precheck
- restart-resilience snapshots
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
- IMPORT-04 source commit: `5f0459a feat(mobile): connect selected media to local outbox draft`
- PR-ready replay branch base: fresh `main` at `1a63bf2`
- canonical current project: `C:\Users\yarad\source\repos\AndroidA_core`

## Validation plan
- `dotnet test .\tests\Unit\App.Mobile.Android.Foundation.Tests\App.Mobile.Android.Foundation.Tests.csproj -c Debug`
- `dotnet build .\src\App.Mobile.Android\App.Mobile.Android.csproj -f net10.0-android -m:1`
- `dotnet format whitespace .\AnalyticsAutomation-Core.sln --verify-no-changes --no-restore`

## Physical Android runtime check
- Result: passed.
- Runtime status: import04 phone ok.
- Selected media can be transferred from Upload to Queue.
- Selected local media card clears after successful handoff.
- Queue renders media-linked item details.
- No selected media warning works.
- Retry/remove still work.

## Manual steps pending
- none for IMPORT-04.

## Waiting for coder 1
- No immediate blocker for this import slice.
- Backend/S1 integration is not started in this PR slice.

## Waiting for coder 2
- No App.UI.Shared changes are included in this PR slice.

## Next code step
- After this PR merges: MOB-CANON-IMPORT-05 local duplicate-precheck PR slice.
- Do not start MOB-CANON-IMPORT-05 in this branch.
