# Coder 3 Mobile Foundation Reconciliation

## Purpose
Compare the canonical mobile project against the previously built mobile foundation without importing code blindly into the canonical repository.

## Canonical App.Mobile.Android current state
The canonical repository started from a template-level MAUI Blazor Hybrid mobile baseline:
- `App.Mobile.Android.csproj` targets `net10.0-android`
- `App.Mobile.Android` already references `App.UI.Shared`
- `MauiProgram.cs` was default MAUI + Blazor WebView setup before reconciliation
- `MainLayout.razor`, `NavMenu.razor`, `Home.razor`, `Counter.razor`, `Weather.razor`, and `NotFound.razor` were still template content
- the visible navigation was still the template `Home`, `Counter`, `Weather` menu
- no local Android media picker/camera foundation was present
- no selected-media cache, outbox, duplicate-precheck, restart-resilience, or repair/rebind seams were present
- `App.UI.Shared` was also still template baseline
- no mobile foundation unit test project was present in the canonical repo

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
| Russian UI baseline | partial after MOB-CANON-IMPORT-01 | yes | more later | low | current active shell/menu/pages are Russian-first now |
| mobile navigation/menu | yes after MOB-CANON-IMPORT-01 | yes | more later | low | template `Counter` and `Weather` no longer appear in active mobile menu |
| shell state/access/flag stubs | yes after MOB-CANON-IMPORT-01 | yes | more later | low | current slice restored local shell mode/options/access/flag seams |
| native video picker | no | yes | yes | medium | belongs to later import slice |
| native camera capture | no | yes | yes | medium | belongs to later import slice |
| selected media cache | no | yes | yes | medium | belongs to later import slice |
| selected media to outbox handoff | no | yes | yes | medium | belongs to later import slice |
| local duplicate-precheck | no | yes | yes | medium | belongs to later import slice |
| snapshot/restart-resilience | no | yes | yes | medium | belongs to later import slice |
| repair/rebind | no | yes | yes | medium | belongs to later import slice |
| mobile foundation unit tests | yes after MOB-CANON-IMPORT-01 | yes | more later | low | canonical repo now has minimal shell/navigation/state tests |
| docs/task-cards/reports | partial after MOB-CANON-IMPORT-01 | yes | yes | low | import trail is being restored slice by slice |

## Recommended reconciliation strategy
Decision:
- split import into multiple smaller PRs

Current implementation status:
- `MOB-CANON-IMPORT-01` is the first active reconciliation slice
- `MOB-CANON-IMPORT-01` is runtime-verified on physical Android
- the current slice restores shell, navigation, Russian visible baseline, local shell-state stubs, narrow shared placeholders, and minimal unit tests
- the import is intentionally split and is not a blind overwrite of canonical `App.Mobile.Android` or `App.UI.Shared`

Why:
- the canonical repo already contains real mobile project placeholders and real Sprint 1 handoff docs
- blindly overwriting canonical `App.Mobile.Android` or `App.UI.Shared` from the backup source would be unsafe
- the missing foundation is broad enough that a single giant import PR would be hard to review and risky to reconcile
- the safest path is to bring in the missing mobile foundation in ordered slices, validate each slice in the canonical repo, and only start Sprint 1 backend integration after the mobile foundation baseline is restored

Recommended next import slice:
- `MOB-CANON-IMPORT-02 - Android media picker/camera + selected-media cache reconciliation`

## Not allowed
- no blind robocopy over canonical `App.Mobile.Android`
- no overwriting canonical solution with old solution
- no backend changes
- no shared DTO/contracts changes
- no App.UI.Shared ownership expansion without coder 2 review
- no Sprint 1 upload code in this step
