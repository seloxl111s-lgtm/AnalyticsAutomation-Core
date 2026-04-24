# Coder 3 Mobile Foundation Reconciliation

## Purpose
Track the controlled mobile foundation replay into the canonical repository without importing broad local-only work.

## Canonical state after PR #67
The canonical repository contains the MOB-CANON-IMPORT-01 shell/navigation/RU baseline from PR #67:
- `App.Mobile.Android.csproj` targets `net10.0-android`.
- `App.Mobile.Android` references `App.UI.Shared` as already present in main.
- Shell, navigation, Russian visible baseline, and local shell-state stubs are present.
- PR #67 is merged into `main`.

## Current PR-ready slice
`MOB-CANON-IMPORT-02` is replayed from fresh `main` at `cb0b960`.

This slice restores only:
- Android native video picker.
- Android native camera capture baseline.
- Local selected-media descriptor/cache.
- Upload media capability card.
- Selected local media card.
- Clear local selection action.
- App.Mobile.Android foundation tests for the selected-media store.

## Explicitly deferred
- outbox foundation
- selected-media to outbox handoff
- duplicate-precheck
- restart-resilience snapshots
- repair/rebind
- report draft shell
- lookup/filter/report UX
- backend S1 adapters
- PreUploadCheck
- UploadReceipt
- direct upload runtime
- shared DTO/contracts changes
- App.UI.Shared changes

## Gap matrix
| Capability | Exists in canonical main | Restored by IMPORT-02 branch | Later slice needed | Notes |
| --- | --- | --- | --- | --- |
| MAUI shell | yes | no | no | Present from PR #67 |
| mobile navigation/menu | yes | no | no | Present from PR #67 |
| Russian visible baseline | yes | no | later polish possible | Present from PR #67 |
| shell state/access/flag stubs | yes | no | later polish possible | Present from PR #67 |
| native video picker | no | yes | no | Local device-only selection, no upload |
| native camera capture | no | yes | no | Device support still requires phone check |
| selected media cache | no | yes | no | In-memory local descriptor/cache |
| selected media to outbox handoff | no | no | yes | IMPORT-03+ |
| local duplicate-precheck | no | no | yes | Later import slice |
| snapshot/restart-resilience | no | no | yes | Later import slice |
| repair/rebind | no | no | yes | Later import slice |
| mobile foundation unit tests | partial | yes | later expansion possible | Adds selected-media store coverage |

## Recommended reconciliation strategy
Continue with small PR-ready import slices from fresh `main`.

The next action for this branch is validation only:
- run App.Mobile.Android foundation unit tests
- run Android build
- perform physical Android phone check for picker/camera/selected-media cache

Do not start MOB-CANON-IMPORT-03, backend/S1 integration, outbox, duplicate-precheck, restart snapshots, repair/rebind, or report UX in this branch.

## Not allowed
- no blind copy over canonical `App.Mobile.Android`
- no overwriting canonical solution with old solution
- no backend changes
- no shared DTO/contracts changes
- no App.UI.Shared changes in this PR slice
- no Sprint 1 upload code in this step
