# Coder 3 Canonical Mobile Import

## Current step
- MOB-CANON-IMPORT-06 PR-ready replay from fresh main.

## Base and coordination state
- Base main SHA at task start: `6b22b69`
- PR #98 is merged into `main`.
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
  - `docs/task-cards/MOB-CANON-IMPORT-05.txt`
  - `docs/task-cards/MOB-CANON-IMPORT-06.txt`

## Scope restored in this PR slice
- Selected-media JSON metadata snapshot store.
- Outbox JSON metadata snapshot store.
- Restored selected-media metadata-only state.
- Restored queue draft metadata-only state.
- Reselect-required warning for metadata-only selected media.
- Restart-resilience unit tests.

## Explicitly not included
- repair/rebind
- report draft
- backend S1 adapters
- PreUploadCheck
- UploadReceipt
- sync
- download
- final offline cache policy
- direct upload runtime
- App.UI.Shared changes
- shared DTO/contracts changes
- SQLite/local DB beyond JSON metadata snapshots
- stream persistence
- file copy
- FullPath-based design

## Source used
- IMPORT-06 source commit: `16c1b92 feat(mobile): restore restart-resilient local snapshots`
- PR-ready replay branch base: fresh `main` at `6b22b69`
- canonical current project: `C:\Users\yarad\source\repos\AndroidA_core`

## Validation plan
- `dotnet test .\tests\Unit\App.Mobile.Android.Foundation.Tests\App.Mobile.Android.Foundation.Tests.csproj -c Debug`
- `dotnet build .\src\App.Mobile.Android\App.Mobile.Android.csproj -f net10.0-android -m:1`
- `dotnet format whitespace .\AnalyticsAutomation-Core.sln --verify-no-changes --no-restore`

## Physical Android runtime check
- Result: pending.
- Runtime status: not_done.
- Manual check must verify restored selected media card, restored queue items, and reselect-required warning after a full restart.

## Manual steps pending
- Physical Android full-restart check for restored selected media card.
- Physical Android full-restart check for restored queue items.
- Confirm restored metadata-only state is explicit.
- Confirm restored selected media cannot be handed off without reselecting the file.
- Confirm reselect-required warning works.
- Confirm retry/remove still work.

## Waiting for coder 1
- No immediate blocker for this import slice.
- Backend/S1 integration is not started in this PR slice.

## Waiting for coder 2
- No App.UI.Shared changes are included in this PR slice.

## Next code step
- After this PR merges: MOB-CANON-IMPORT-07 local repair/rebind PR slice.
- Do not start MOB-CANON-IMPORT-07 in this branch.
