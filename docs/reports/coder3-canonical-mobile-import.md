# Coder 3 Canonical Mobile Import

## Current step
- MOB-CANON-IMPORT-02 PR-ready replay from fresh main.

## Base and coordination state
- Base main SHA at task start: `cb0b960`
- PR #67 is merged into `main`.
- Coordination log source: GitHub issue `#89`, not a repo file.
- Latest coordination entries reviewed:
  - `#90` S2-37 Admin Review API AuthZ Scope
  - `#91` S2-39 Deploy Trigger Reconciliation
  - `#92` S2-40 Coordination Readiness and ChatGPT Sync Readiness
- Handoff/task cards reviewed:
  - `docs/handoffs/s1-11-coder3-android-integration.md`
  - `docs/handoffs/s1-11-backend-integration-note.md`
  - `docs/handoffs/s1-12-sprint1-quality-gate.md`
  - `docs/task-cards/MOB-CANON-IMPORT-01.txt`
  - `docs/task-cards/MOB-CANON-IMPORT-02.txt`

## Scope restored in this PR slice
- Android native video picker.
- Android native camera capture baseline.
- Selected-media descriptor/cache.
- Upload media capability card.
- Selected local media card.
- Clear local selection action.
- Minimal App.Mobile.Android foundation tests for the selected-media store.

## Explicitly not included
- outbox
- selected-media to outbox handoff
- duplicate-precheck
- restart snapshots
- repair/rebind
- report draft
- backend S1 adapters
- PreUploadCheck
- UploadReceipt
- direct upload runtime
- App.UI.Shared changes
- shared DTO/contracts changes

## Source used
- IMPORT-02 source commit: `9ea62e7 feat(mobile): restore Android media picker and selected media cache`
- PR-ready replay branch base: fresh `main` at `cb0b960`
- canonical current project: `C:\Users\yarad\source\repos\AndroidA_core`

## Validation plan
- `dotnet test .\tests\Unit\App.Mobile.Android.Foundation.Tests\App.Mobile.Android.Foundation.Tests.csproj -c Debug`
- `dotnet build .\src\App.Mobile.Android\App.Mobile.Android.csproj -f net10.0-android -m:1`

## Manual steps pending
- Physical Android runtime check for media picker/camera/selected-media cache on this fresh-main branch.
- Confirm native video picker opens.
- Confirm native camera capture opens or shows a safe device capability error.
- Confirm selected local media card renders after pick/capture.
- Confirm clear local selection action works.

## Waiting for coder 1
- No immediate blocker for this import slice.
- Backend/S1 integration is not started in this PR slice.

## Waiting for coder 2
- No App.UI.Shared changes are included in this PR slice.

## Next code step
- Do not start MOB-CANON-IMPORT-03 until this PR-ready branch has phone-check evidence and owner approval.
