# Coder 3 Mobile Foundation Reconciliation

## Purpose
Track the controlled mobile foundation replay into the canonical repository without importing broad local-only work.

## Canonical state after PR #96
The canonical repository contains the first three restored mobile foundation slices:
- `App.Mobile.Android.csproj` targets `net10.0-android`.
- `App.Mobile.Android` references `App.UI.Shared` as already present in main.
- Shell, navigation, Russian visible baseline, and local shell-state stubs are present from MOB-CANON-IMPORT-01 / PR #67.
- Android native video picker and native camera capture baseline are present from MOB-CANON-IMPORT-02 / PR #95.
- Local selected-media descriptor/cache and the selected local media card are present from MOB-CANON-IMPORT-02 / PR #95.
- Local in-memory outbox foundation, queue rendering, and retry/remove local actions are present from MOB-CANON-IMPORT-03 / PR #96.
- `App.UI.Shared` is intentionally unchanged by the mobile import slices after PR #67.

## Current PR-ready slice
`MOB-CANON-IMPORT-04` is replayed from fresh `main` at `1a63bf2`.

This slice restores only:
- Selected-media to outbox local handoff.
- LocalSelectedMediaStoreEntry.
- PendingSyncItemLocalMediaDraft.
- TakeCurrentAsync on selected-media store.
- Selected-media-aware outbox enqueue.
- Queue media-linked draft details.
- Unit tests for selected-media handoff and outbox behavior.

## Explicitly deferred
- duplicate-precheck
- restart-resilience snapshots
- repair/rebind
- report draft shell
- lookup/filter/report/profile UX
- backend S1 adapters
- PreUploadCheck
- UploadReceipt
- direct upload runtime
- sync
- download
- shared DTO/contracts changes
- App.UI.Shared changes
- SQLite/local DB persistence

## Gap matrix
| Capability | Exists in canonical main | Restored by IMPORT-04 branch | Later slice needed | Notes |
| --- | --- | --- | --- | --- |
| MAUI shell | yes | no | no | Present from PR #67 |
| mobile navigation/menu | yes | no | no | Present from PR #67 |
| native video picker | yes | no | no | Present from PR #95 |
| native camera capture | yes | no | no | Present from PR #95 |
| selected media cache | yes | no | no | Present from PR #95 |
| local outbox foundation | yes | no | no | Present from PR #96 |
| selected media to outbox handoff | no | yes | no | In-memory local handoff only |
| local duplicate-precheck | no | no | yes | Later import slice |
| snapshot/restart-resilience | no | no | yes | Later import slice |
| repair/rebind | no | no | yes | Later import slice |
| mobile foundation unit tests | partial | yes | later expansion possible | Adds selected-media handoff coverage |

## Recommended reconciliation strategy
Continue with small PR-ready import slices from fresh `main`.

The current branch is only for validation of MOB-CANON-IMPORT-04:
- run App.Mobile.Android foundation unit tests
- run Android build
- run whitespace format check
- physical Android phone check passed for selected-media handoff from Upload to Queue

Do not start MOB-CANON-IMPORT-05, backend/S1 integration, duplicate-precheck, restart snapshots, repair/rebind, report draft, or report UX in this branch.

## Not allowed
- no blind copy over canonical `App.Mobile.Android`
- no overwriting canonical solution with old solution
- no backend changes
- no shared DTO/contracts changes
- no App.UI.Shared changes in this PR slice
- no Sprint 1 upload code in this step
