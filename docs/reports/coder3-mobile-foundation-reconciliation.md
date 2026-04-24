# Coder 3 Mobile Foundation Reconciliation

## Purpose
Track the controlled mobile foundation replay into the canonical repository without importing broad local-only work.

## Canonical state after PR #95
The canonical repository contains the first two restored mobile foundation slices:
- `App.Mobile.Android.csproj` targets `net10.0-android`.
- `App.Mobile.Android` references `App.UI.Shared` as already present in main.
- Shell, navigation, Russian visible baseline, and local shell-state stubs are present from MOB-CANON-IMPORT-01 / PR #67.
- Android native video picker and native camera capture baseline are present from MOB-CANON-IMPORT-02 / PR #95.
- Local selected-media descriptor/cache and the selected local media card are present from MOB-CANON-IMPORT-02 / PR #95.
- `App.UI.Shared` is intentionally unchanged by the mobile import slices after PR #67.

## Current PR-ready slice
`MOB-CANON-IMPORT-03` is replayed from fresh `main` at `0015c7d`.

This slice restores only:
- Local in-memory outbox foundation.
- PendingSyncItem local models.
- IMobileOutboxService.
- StubMobileOutboxService.
- Upload local enqueue stub action.
- Queue local item rendering.
- Retry/remove local actions.
- App.Mobile.Android foundation tests for the local outbox service.

## Explicitly deferred
- selected-media to outbox handoff
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
| Capability | Exists in canonical main | Restored by IMPORT-03 branch | Later slice needed | Notes |
| --- | --- | --- | --- | --- |
| MAUI shell | yes | no | no | Present from PR #67 |
| mobile navigation/menu | yes | no | no | Present from PR #67 |
| Russian visible baseline | yes | no | later polish possible | Present from PR #67 |
| native video picker | yes | no | no | Present from PR #95 |
| native camera capture | yes | no | no | Present from PR #95 |
| selected media cache | yes | no | no | Present from PR #95 |
| local outbox foundation | no | yes | no | In-memory local queue only |
| selected media to outbox handoff | no | no | yes | Later import slice |
| local duplicate-precheck | no | no | yes | Later import slice |
| snapshot/restart-resilience | no | no | yes | Later import slice |
| repair/rebind | no | no | yes | Later import slice |
| mobile foundation unit tests | partial | yes | later expansion possible | Adds outbox service coverage |

## Recommended reconciliation strategy
Continue with small PR-ready import slices from fresh `main`.

The current branch is only for validation of MOB-CANON-IMPORT-03:
- run App.Mobile.Android foundation unit tests
- run Android build
- run whitespace format check
- physical Android phone check passed for Upload enqueue and Queue retry/remove behavior

Do not start MOB-CANON-IMPORT-04, backend/S1 integration, selected-media handoff, duplicate-precheck, restart snapshots, repair/rebind, or report UX in this branch.

## Not allowed
- no blind copy over canonical `App.Mobile.Android`
- no overwriting canonical solution with old solution
- no backend changes
- no shared DTO/contracts changes
- no App.UI.Shared changes in this PR slice
- no Sprint 1 upload code in this step
