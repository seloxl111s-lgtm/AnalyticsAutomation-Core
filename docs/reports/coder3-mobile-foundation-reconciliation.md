# Coder 3 Mobile Foundation Reconciliation

## Purpose
Track the controlled mobile foundation replay into the canonical repository without importing broad local-only work.

## Canonical App.Mobile.Android current state
The canonical repository contains the first five merged mobile foundation slices and the current PR-ready IMPORT-06 replay:
- `App.Mobile.Android.csproj` targets `net10.0-android`.
- `App.Mobile.Android` references `App.UI.Shared` as already present in main.
- Shell, navigation, Russian visible baseline, and local shell-state stubs are present from MOB-CANON-IMPORT-01 / PR #67.
- Android native video picker and native camera capture baseline are present from MOB-CANON-IMPORT-02 / PR #95.
- Local selected-media descriptor/cache and the selected local media card are present from MOB-CANON-IMPORT-02 / PR #95.
- Local in-memory outbox foundation, queue rendering, and retry/remove local actions are present from MOB-CANON-IMPORT-03 / PR #96.
- Selected-media to outbox draft handoff and media-linked queue details are present from MOB-CANON-IMPORT-04 / PR #97.
- Local duplicate-precheck and duplicate-aware handoff behavior are present from MOB-CANON-IMPORT-05 / PR #98.
- `App.UI.Shared` is intentionally unchanged by this IMPORT-06 slice.

## Current PR-ready slice
`MOB-CANON-IMPORT-06` is replayed from fresh `main` at `6b22b69`.

This slice restores only:
- Selected-media JSON metadata snapshot store.
- Outbox JSON metadata snapshot store.
- Restored selected-media metadata-only state.
- Restored queue draft metadata-only state.
- Reselect-required warning for metadata-only selected media.
- Restart-resilience unit tests.

## Explicitly deferred
- repair/rebind
- report draft shell
- lookup/filter/report/profile UX
- backend S1 adapters
- PreUploadCheck
- UploadReceipt
- direct upload runtime
- sync
- download
- final offline cache policy
- shared DTO/contracts changes
- App.UI.Shared changes
- SQLite/local DB persistence beyond JSON metadata snapshots
- stream persistence
- file copy
- FullPath-based design

## Gap matrix
| Capability | Exists in canonical main | Restored by IMPORT-06 branch | Later slice needed | Notes |
| --- | --- | --- | --- | --- |
| MAUI shell | yes | no | no | Present from PR #67 |
| mobile navigation/menu | yes | no | no | Present from PR #67 |
| native video picker | yes | no | no | Present from PR #95 |
| native camera capture | yes | no | no | Present from PR #95 |
| selected media cache | yes | no | no | Present from PR #95 |
| local outbox foundation | yes | no | no | Present from PR #96 |
| selected media to outbox handoff | yes | no | no | Present from PR #97 |
| local duplicate-precheck | yes | no | no | Present from PR #98 |
| restart-resilience JSON metadata snapshots | no | yes | no | Current PR-ready slice |
| repair/rebind | no | no | yes | Later import slice |
| mobile foundation unit tests | partial | yes | later expansion possible | Adds restart-resilience snapshot coverage |

## Recommended reconciliation strategy
Continue with small PR-ready import slices from fresh `main`.

The current branch is only for validation of MOB-CANON-IMPORT-06:
- run App.Mobile.Android foundation unit tests
- run Android build
- run whitespace format check
- physical Android full-restart check is pending for restored metadata-only state

Do not start MOB-CANON-IMPORT-07, backend/S1 integration, repair/rebind, report draft, or report UX in this branch.

## Not allowed
- no blind copy over canonical `App.Mobile.Android`
- no overwriting canonical solution with old solution
- no backend changes
- no shared DTO/contracts changes
- no App.UI.Shared changes in this PR slice
- no Sprint 1 upload code in this step
