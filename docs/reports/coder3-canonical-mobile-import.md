# Coder 3 Canonical Mobile Import

## Current step
- MOB-CANON-IMPORT-05 PR-ready replay from fresh main.

## Base and coordination state
- Base main SHA at task start: `fb4d589`
- PR #97 is merged into `main`.
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
  - `docs/task-cards/MOB-CANON-IMPORT-04.txt`
  - `docs/task-cards/MOB-CANON-IMPORT-05.txt`

## Scope restored in this PR slice
- Local duplicate-precheck models.
- `ILocalDuplicatePrecheckService`.
- `LocalOutboxDuplicatePrecheckService`.
- Duplicate-aware selected-media handoff.
- Upload local precheck card.
- Unit tests for local duplicate-precheck and outbox duplicate behavior.

## Explicitly not included
- restart-resilience snapshots
- repair/rebind
- report draft
- backend S1 adapters
- PreUploadCheck
- UploadReceipt
- sync
- download
- final dedupe/fraud logic
- direct upload runtime
- App.UI.Shared changes
- shared DTO/contracts changes
- SQLite/local DB
- persistence

## Source used
- IMPORT-05 source commit: `0020870 feat(mobile): restore local duplicate precheck`
- PR-ready replay branch base: fresh `main` at `fb4d589`
- canonical current project: `C:\Users\yarad\source\repos\AndroidA_core`

## Validation plan
- `dotnet test .\tests\Unit\App.Mobile.Android.Foundation.Tests\App.Mobile.Android.Foundation.Tests.csproj -c Debug`
- `dotnet build .\src\App.Mobile.Android\App.Mobile.Android.csproj -f net10.0-android -m:1`
- `dotnet format whitespace .\AnalyticsAutomation-Core.sln --verify-no-changes --no-restore`

## Physical Android runtime check
- Result: pending.
- Runtime status: not_done.
- Manual check must verify local duplicate-precheck warning and preserved non-duplicate handoff.

## Manual steps pending
- Physical Android runtime check for local duplicate-precheck warning and preserved non-duplicate handoff.
- Confirm duplicate current selection is blocked.
- Confirm duplicate rejection does not clear current selected media.
- Confirm duplicate rejection does not add a second queue item.
- Confirm non-duplicate handoff still works.
- Confirm retry/remove still work.

## Waiting for coder 1
- No immediate blocker for this import slice.
- Backend/S1 integration is not started in this PR slice.

## Waiting for coder 2
- No App.UI.Shared changes are included in this PR slice.

## Next code step
- After this PR merges: MOB-CANON-IMPORT-06 restart-resilience snapshot PR slice.
- Do not start MOB-CANON-IMPORT-06 in this branch.
