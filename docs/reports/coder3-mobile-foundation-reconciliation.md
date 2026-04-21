# Coder 3 Mobile Foundation Reconciliation

## Purpose
Compare the canonical mobile project against the previously built mobile foundation without importing code blindly into the canonical repository.

## Canonical App.Mobile.Android current state
The canonical repository now contains the first two restored mobile foundation slices:
- `App.Mobile.Android.csproj` still targets `net10.0-android`
- `App.Mobile.Android` still references `App.UI.Shared`
- shell, navigation, Russian visible baseline, and local shell-state stubs were restored in `MOB-CANON-IMPORT-01`
- Android native video picker and native camera capture are restored in `MOB-CANON-IMPORT-02`
- local selected-media descriptor/cache and the selected local media card on `Upload` are restored in `MOB-CANON-IMPORT-02`
- `App.UI.Shared` is intentionally unchanged in `MOB-CANON-IMPORT-02`
- outbox, selected-media handoff, duplicate-precheck, restart-resilience, and repair/rebind are still absent in canonical `App.Mobile.Android`
- the mobile foundation unit test project exists and now includes selected-media store coverage

## Prior mobile foundation available for reuse
Previous source path is available:
- `C:\Users\yarad\source\repos\AndroidA_git_wrong_repo_backup_20260419_151951`

Observed reusable mobile foundation from that source:
- native picker/camera foundation
- selected-media cache
- outbox foundation
- duplicate-precheck
- restart-resilience snapshot seam
- repair/rebind seam
- Russian UI baseline
- local shell state/access/flag stubs
- App.UI.Shared placeholder/shared baseline used by mobile
- unit test project present under `tests/Unit/App.Mobile.Android.Foundation.Tests`
- prior report trail indicates unit tests passed `48/48`
- prior docs/task-cards/reports exist and can inform a controlled import plan

## Gap matrix
| Capability | Exists in canonical | Exists in prior foundation | Import/reconcile needed | Risk | Notes |
| --- | --- | --- | --- | --- | --- |
| MAUI shell | yes | yes | no | low | canonical already had the base shell scaffold |
| App.UI.Shared hookup | yes | yes | no | low | canonical already had the project reference |
| Russian UI baseline | yes after MOB-CANON-IMPORT-01 | yes | more later | low | current active shell/menu/pages are Russian-first |
| mobile navigation/menu | yes after MOB-CANON-IMPORT-01 | yes | more later | low | template `Counter` and `Weather` do not appear in active mobile menu |
| shell state/access/flag stubs | yes after MOB-CANON-IMPORT-01 | yes | more later | low | local shell mode/options/access/flag seams are already present |
| native video picker | yes after MOB-CANON-IMPORT-02 | yes | more later | medium | local Android native gallery video selection is restored |
| native camera capture | yes after MOB-CANON-IMPORT-02 | yes | more later | medium | runtime support still depends on device capability |
| selected media cache | yes after MOB-CANON-IMPORT-02 | yes | more later | low | in-memory local selected-media descriptor/cache is restored |
| selected media to outbox handoff | no | yes | yes | medium | belongs to a later import slice |
| local duplicate-precheck | no | yes | yes | medium | belongs to a later import slice |
| snapshot/restart-resilience | no | yes | yes | medium | belongs to a later import slice |
| repair/rebind | no | yes | yes | medium | belongs to a later import slice |
| mobile foundation unit tests | yes after MOB-CANON-IMPORT-02 | yes | more later | low | canonical repo now covers shell/navigation/state plus selected-media store |
| docs/task-cards/reports | partial after MOB-CANON-IMPORT-02 | yes | yes | low | import trail is being restored slice by slice |

## Recommended reconciliation strategy
Decision:
- split import into multiple smaller PRs

Current implementation status:
- `MOB-CANON-IMPORT-01` is runtime-verified on physical Android and PR `#67` is ready for review
- `MOB-CANON-IMPORT-02` is the current stacked reconciliation slice
- the current slice restores Android native video picker/camera, local selected-media descriptor/cache, the `Upload` selected-media card, and minimal selected-media store tests
- local validation for `MOB-CANON-IMPORT-02` currently passes: `14/14` tests and Android build success
- `MOB-CANON-IMPORT-02` is runtime-verified on physical Android for the visible `Upload` screen and native video picker flow
- the import remains intentionally split and is not a blind overwrite of canonical `App.Mobile.Android` or `App.UI.Shared`

Why:
- the canonical repo already contains real mobile project placeholders and real Sprint 1 handoff docs
- blindly overwriting canonical `App.Mobile.Android` or `App.UI.Shared` from the backup source would be unsafe
- the missing foundation is broad enough that a single giant import PR would be hard to review and risky to reconcile
- the safest path is to bring in the missing mobile foundation in ordered slices, validate each slice in the canonical repo, and only start Sprint 1 backend integration after the mobile foundation baseline is restored

Current stacked slice:
- `MOB-CANON-IMPORT-02 - Android media picker/camera + selected-media cache reconciliation`

Next import slice after `MOB-CANON-IMPORT-02` manual closure:
- `MOB-CANON-IMPORT-03 - local outbox foundation reconciliation`

Remaining later slices:
- selected-media to outbox handoff
- duplicate-precheck
- restart-resilience
- repair/rebind

## Not allowed
- no blind robocopy over canonical `App.Mobile.Android`
- no overwriting canonical solution with old solution
- no backend changes
- no shared DTO/contracts changes
- no App.UI.Shared ownership expansion without coder 2 review
- no Sprint 1 upload code in this step
