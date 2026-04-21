# Coder 3 Canonical Mobile Import

## Current step
- MOB-CANON-IMPORT-07 PR-ready replay from fresh main.

## Base and coordination state
- Base main SHA at task start: `b5d0be7`
- PR #99 is merged into `main`.
- Coordination log source: GitHub issue `#89`, not a repo file.
- Latest coordination entries reviewed:
  - `#95` feat(mobile): restore Android media picker and selected media cache
  - `#96` feat(mobile): restore local outbox foundation
  - `#97` feat(mobile): connect selected media to local outbox draft
  - `#98` feat(mobile): restore local duplicate precheck
  - `#99` feat(mobile): restore restart-resilient local snapshots
- Handoff/task cards reviewed:
  - `docs/handoffs/s1-11-coder3-android-integration.md`
  - `docs/handoffs/s1-11-backend-integration-note.md`
  - `docs/handoffs/s1-12-sprint1-quality-gate.md`
  - `docs/task-cards/MOB-CANON-IMPORT-06.txt`
  - `docs/task-cards/MOB-CANON-IMPORT-07.txt`

## Scope restored in this PR slice
- Local repair/rebind models.
- `ILocalMediaDraftRepairService`.
- `LocalCurrentSelectionDraftRepairService`.
- `RepairLocalMediaDraftAsync` on local outbox service.
- Queue repair button for restored metadata-only drafts.
- Upload note for restored metadata-only selection.
- Unit tests for repair service and outbox repair behavior.

## Explicitly not included
- report draft
- backend S1 adapters
- PreUploadCheck
- UploadReceipt
- sync
- download
- final offline cache policy
- lookup/filter/profile UX
- incident creation
- direct upload runtime
- App.UI.Shared changes
- shared DTO/contracts changes
- SQLite/local DB
- persistence redesign
- FullPath-based design
- hashes
- ffprobe
- final dedupe logic
- worker/deep-analysis logic

## Source used
- IMPORT-07 source commit: `5c7c11e feat(mobile): restore local media draft repair`
- PR-ready replay branch base: fresh `main` at `b5d0be7`
- canonical current project: `C:\Users\yarad\source\repos\AndroidA_core`

## Validation plan
- `dotnet test .\tests\Unit\App.Mobile.Android.Foundation.Tests\App.Mobile.Android.Foundation.Tests.csproj -c Debug`
- `dotnet build .\src\App.Mobile.Android\App.Mobile.Android.csproj -f net10.0-android -m:1`
- `dotnet format whitespace .\AnalyticsAutomation-Core.sln --verify-no-changes --no-restore`

## Physical Android runtime check
- Result: pending.
- Runtime status: not_done.
- Manual check must verify repairing a restored queue draft from the current selected media.

## Manual steps pending
- Physical Android runtime check for repairing a restored queue draft from the current selected media.
- Confirm mismatch is blocked with Russian warning.
- Confirm successful repair clears current selected media.
- Confirm metadata-only note disappears after successful repair.
- Confirm retry/remove still work.

## Waiting for coder 1
- No immediate blocker for this import slice.
- Backend/S1 integration is not started in this PR slice.

## Waiting for coder 2
- No App.UI.Shared changes are included in this PR slice.

## Next code step
- After this PR merges: report-first PR slices or backend integration planning only after explicit sync/coordination decision.
- Do not start report, UX, profile, backend, or worker work in this branch.
