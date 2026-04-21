# Coder 3 Mobile Foundation Reconciliation

## Purpose
Track the controlled mobile foundation replay into the canonical repository without importing broad local-only work.

## Canonical App.Mobile.Android current state
The canonical repository contains the first four merged mobile foundation slices and the current PR-ready IMPORT-05 replay:
- `App.Mobile.Android.csproj` targets `net10.0-android`.
- `App.Mobile.Android` references `App.UI.Shared` as already present in main.
- Shell, navigation, Russian visible baseline, and local shell-state stubs are present from MOB-CANON-IMPORT-01 / PR #67.
- Android native video picker and native camera capture baseline are present from MOB-CANON-IMPORT-02 / PR #95.
- Local selected-media descriptor/cache and the selected local media card are present from MOB-CANON-IMPORT-02 / PR #95.
- Local in-memory outbox foundation, queue rendering, and retry/remove local actions are present from MOB-CANON-IMPORT-03 / PR #96.
- Selected-media to outbox draft handoff and media-linked queue details are present from MOB-CANON-IMPORT-04 / PR #97.
- `App.UI.Shared` is intentionally unchanged by this IMPORT-05 slice.

## Current PR-ready slice
`MOB-CANON-IMPORT-05` is replayed from fresh `main` at `fb4d589`.

This slice restores only:
- Local duplicate-precheck models.
- `ILocalDuplicatePrecheckService`.
- `LocalOutboxDuplicatePrecheckService`.
- Duplicate-aware selected-media handoff.
- Upload local precheck card.
- Unit tests for local duplicate-precheck and outbox duplicate behavior.

## Explicitly deferred
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
- final dedupe/fraud logic
- shared DTO/contracts changes
- App.UI.Shared changes
- SQLite/local DB persistence

## Gap matrix
| Capability | Exists in canonical main | Restored by IMPORT-05 branch | Later slice needed | Notes |
| --- | --- | --- | --- | --- |
| MAUI shell | yes | no | no | Present from PR #67 |
| mobile navigation/menu | yes | no | no | Present from PR #67 |
| native video picker | yes | no | no | Present from PR #95 |
| native camera capture | yes | no | no | Present from PR #95 |
| selected media cache | yes | no | no | Present from PR #95 |
| local outbox foundation | yes | no | no | Present from PR #96 |
| selected media to outbox handoff | yes | no | no | Present from PR #97 |
| local duplicate-precheck | no | yes | no | Current PR-ready slice |
| snapshot/restart-resilience | no | no | yes | Later import slice |
| repair/rebind | no | no | yes | Later import slice |
| mobile foundation unit tests | partial | yes | later expansion possible | Adds local duplicate-precheck coverage |

## Recommended reconciliation strategy
Continue with small PR-ready import slices from fresh `main`.

The current branch is only for validation of MOB-CANON-IMPORT-05:
- run App.Mobile.Android foundation unit tests
- run Android build
- run whitespace format check
- physical Android phone check is pending for local duplicate-precheck behavior

Do not start MOB-CANON-IMPORT-06, backend/S1 integration, restart snapshots, repair/rebind, report draft, or report UX in this branch.

## Not allowed
- no blind copy over canonical `App.Mobile.Android`
- no overwriting canonical solution with old solution
- no backend changes
- no shared DTO/contracts changes
- no App.UI.Shared changes in this PR slice
- no Sprint 1 upload code in this step
