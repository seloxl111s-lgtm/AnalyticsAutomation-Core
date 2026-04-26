# Coder 3 Mobile Foundation Reconciliation

## Purpose
Track the controlled mobile foundation replay into the canonical repository without importing broad local-only work.

## Canonical App.Mobile.Android current state
The canonical repository contains the first six merged mobile foundation slices and the current PR-ready IMPORT-07 replay:
- `App.Mobile.Android.csproj` targets `net10.0-android`.
- `App.Mobile.Android` references `App.UI.Shared` as already present in main.
- Shell, navigation, Russian visible baseline, and local shell-state stubs are present from MOB-CANON-IMPORT-01 / PR #67.
- Android native video picker and native camera capture baseline are present from MOB-CANON-IMPORT-02 / PR #95.
- Local selected-media descriptor/cache and the selected local media card are present from MOB-CANON-IMPORT-02 / PR #95.
- Local in-memory outbox foundation, queue rendering, and retry/remove local actions are present from MOB-CANON-IMPORT-03 / PR #96.
- Selected-media to outbox draft handoff and media-linked queue details are present from MOB-CANON-IMPORT-04 / PR #97.
- Local duplicate-precheck and duplicate-aware handoff behavior are present from MOB-CANON-IMPORT-05 / PR #98.
- Restart-resilience JSON metadata snapshots are present from MOB-CANON-IMPORT-06 / PR #99.
- `App.UI.Shared` is intentionally unchanged by this IMPORT-07 slice.

## Current PR-ready slice
`MOB-CANON-IMPORT-07` is replayed from fresh `main` at `b5d0be7`.

This slice restores only:
- Local repair/rebind models.
- `ILocalMediaDraftRepairService`.
- `LocalCurrentSelectionDraftRepairService`.
- `RepairLocalMediaDraftAsync` on local outbox service.
- Queue repair button for restored metadata-only drafts.
- Upload note for restored metadata-only selection.
- Unit tests for repair service and outbox repair behavior.

## Explicitly deferred
- report draft shell
- lookup/filter/report/profile UX
- backend S1 adapters
- PreUploadCheck
- UploadReceipt
- direct upload runtime
- sync
- download
- final offline cache policy
- incident creation
- fraud/worker logic
- shared DTO/contracts changes
- App.UI.Shared changes
- SQLite/local DB
- persistence redesign
- FullPath-based design
- hashes
- ffprobe

## Gap matrix
| Capability | Exists in canonical main | Restored by IMPORT-07 branch | Later slice needed | Notes |
| --- | --- | --- | --- | --- |
| MAUI shell | yes | no | no | Present from PR #67 |
| mobile navigation/menu | yes | no | no | Present from PR #67 |
| native video picker | yes | no | no | Present from PR #95 |
| native camera capture | yes | no | no | Present from PR #95 |
| selected media cache | yes | no | no | Present from PR #95 |
| local outbox foundation | yes | no | no | Present from PR #96 |
| selected media to outbox handoff | yes | no | no | Present from PR #97 |
| local duplicate-precheck | yes | no | no | Present from PR #98 |
| restart-resilience JSON metadata snapshots | yes | no | no | Present from PR #99 |
| local repair/rebind | no | yes | no | Current PR-ready slice |
| report/UX/backend integration | no | no | yes | Requires explicit coordination decision |
| mobile foundation unit tests | partial | yes | later expansion possible | Adds local repair/rebind coverage |

## Recommended reconciliation strategy
Continue with small PR-ready import slices from fresh `main`.

The current branch is only for validation of MOB-CANON-IMPORT-07:
- run App.Mobile.Android foundation unit tests
- run Android build
- run whitespace format check
- physical Android repair/rebind check passed

Do not start report, UX, profile, backend/S1 integration, incident creation, fraud, or worker logic in this branch.

## Not allowed
- no blind copy over canonical `App.Mobile.Android`
- no overwriting canonical solution with old solution
- no backend changes
- no shared DTO/contracts changes
- no App.UI.Shared changes in this PR slice
- no Sprint 1 upload code in this step
