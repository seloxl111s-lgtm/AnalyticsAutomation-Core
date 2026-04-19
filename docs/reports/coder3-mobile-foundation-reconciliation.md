# Coder 3 Mobile Foundation Reconciliation

## Purpose
Compare the canonical mobile project against the previously built mobile foundation without importing code in this step.

## Canonical App.Mobile.Android current state
The canonical repository currently contains a template-level MAUI Blazor Hybrid mobile baseline:
- `App.Mobile.Android.csproj` targets `net10.0-android`
- `App.Mobile.Android` already references `App.UI.Shared`
- `MauiProgram.cs` is still default MAUI + Blazor WebView setup without local mobile foundation services
- `MainLayout.razor`, `NavMenu.razor`, `Home.razor`, `Counter.razor`, `Weather.razor`, and `NotFound.razor` are still template content
- the visible navigation is still the template `Home`, `Counter`, `Weather` menu
- no local Android media picker/camera foundation is present
- no selected-media cache, outbox, duplicate-precheck, restart-resilience, or repair/rebind seams are present
- `App.UI.Shared` is also still template baseline
- no mobile foundation unit test project is present in the canonical repo

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
| MAUI shell | yes | yes | no | low | canonical already has the base shell scaffold |
| App.UI.Shared hookup | yes | yes | no | low | canonical already has a project reference, but shared placeholder content still differs |
| Russian UI baseline | no | yes | yes | low | visible mobile text is still template English in canonical |
| mobile navigation/menu | template only | yes | yes | medium | canonical nav is still `Home/Counter/Weather` template |
| shell state/access/flag stubs | no | yes | yes | medium | local shell seams exist only in prior foundation |
| native video picker | no | yes | yes | medium | canonical has no `IMobileMediaService` or Android native binding |
| native camera capture | no | yes | yes | medium | canonical has no camera capture seam or service |
| selected media cache | no | yes | yes | medium | prior foundation has in-memory selected media store and descriptor models |
| selected media to outbox handoff | no | yes | yes | medium | canonical has no handoff logic or media-linked draft model |
| local duplicate-precheck | no | yes | yes | medium | prior foundation has explicit local duplicate-precheck service and models |
| snapshot/restart-resilience | no | yes | yes | medium | prior foundation has JSON snapshot stores for selected media and outbox metadata |
| repair/rebind | no | yes | yes | medium | prior foundation has local repair service for restored metadata-only drafts |
| mobile foundation unit tests | no | yes | yes | medium | canonical solution has integration tests only; no mobile unit test gate |
| docs/task-cards/reports | no | yes | yes | low | canonical repo does not currently contain the prior mobile planning/report trail |

## Recommended reconciliation strategy
Decision:
- split import into multiple smaller PRs

Why:
- the canonical repo already contains real mobile project placeholders and real Sprint 1 handoff docs
- blindly overwriting canonical `App.Mobile.Android` or `App.UI.Shared` from the backup source would be unsafe
- the missing foundation is broad enough that a single giant import PR would be hard to review and risky to reconcile
- the safest path is to bring in the missing mobile foundation in ordered slices, validate each slice in the canonical repo, and only start Sprint 1 backend integration after the mobile foundation baseline is restored

Recommended first reconciliation step:
- `MOB-CANON-IMPORT-01 — reconcile/import missing mobile foundation into canonical repo first`

## Not allowed
- no blind robocopy over canonical `App.Mobile.Android`
- no overwriting canonical solution with old solution
- no backend changes
- no shared DTO/contracts changes
- no App.UI.Shared ownership expansion without coder 2 review
- no Sprint 1 upload code in this docs-only step
