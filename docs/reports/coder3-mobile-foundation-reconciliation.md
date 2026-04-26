# Coder 3 Mobile Foundation Reconciliation

## Purpose
Track the controlled mobile foundation replay into the canonical repository without importing broad local-only work.

## Canonical App.Mobile.Android current state
The canonical repository contains the complete IMPORT-01..07 Android media/outbox foundation chain in `main`:
- `App.Mobile.Android.csproj` targets `net10.0-android`.
- `App.Mobile.Android` references `App.UI.Shared` as already present in main.
- Shell, navigation, Russian visible baseline, and local shell-state stubs are present from MOB-CANON-IMPORT-01 / PR #67.
- Android native video picker and native camera capture baseline are present from MOB-CANON-IMPORT-02 / PR #95.
- Local selected-media descriptor/cache and the selected local media card are present from MOB-CANON-IMPORT-02 / PR #95.
- Local in-memory outbox foundation, queue rendering, and retry/remove local actions are present from MOB-CANON-IMPORT-03 / PR #96.
- Selected-media to outbox draft handoff and media-linked queue details are present from MOB-CANON-IMPORT-04 / PR #97.
- Local duplicate-precheck and duplicate-aware handoff behavior are present from MOB-CANON-IMPORT-05 / PR #98.
- Restart-resilience JSON metadata snapshots are present from MOB-CANON-IMPORT-06 / PR #99.
- Local repair/rebind is present from MOB-CANON-IMPORT-07 / PR #100.
- `App.UI.Shared` was intentionally unchanged by IMPORT-02..07.

## Current readiness checkpoint
`MOB-CANON-BACKEND-00` is a docs-only readiness checkpoint from fresh `main` at `e4dd243`.

Android media/outbox foundation reconciliation is complete in main through IMPORT-07.
Report/UX/backend work is not yet replayed from old stacked work.
The next decision is backend adapter foundation versus local report UX replay.

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
| local repair/rebind | yes | no | no | Present from PR #100 |
| report/UX/backend integration | no | no | yes | Requires explicit coordination decision |
| mobile foundation unit tests | yes | no | later expansion possible | Foundation coverage merged through IMPORT-07 |

## Recommended reconciliation strategy
Continue with small PR-ready slices from fresh `main`, but choose the next direction explicitly.

Backend adapter foundation should wait for the approved Android source of `businessObjectKey` / report draft / business object binding before any `PreUploadCheck` runtime.

Local report UX replay remains possible only as local/non-production context if the owner accepts that it does not provide backend save, `businessObjectKey`, production reports engine, `PreUploadCheck`, or `UploadReceipt`.

Do not start report, UX, profile, backend/S1 integration, incident creation, fraud, or worker logic in this branch.

## Not allowed
- no blind copy over canonical `App.Mobile.Android`
- no overwriting canonical solution with old solution
- no backend changes
- no shared DTO/contracts changes
- no App.UI.Shared changes in this PR slice
- no Sprint 1 upload code in this step
