# Coder 3 Android Backend Readiness

## Repository and sync state
- Repository path: `C:\Users\yarad\source\repos\AndroidA_core`
- Current main SHA: `e4dd243`
- Latest main commits reviewed:
  - `e4dd243` feat(mobile): restore local media draft repair (#100)
  - `f22a4b8` S2-47 Desktop UI Technology Owner Decision
  - `787805f` S2-46 Desktop UI Technology Decision Task
  - `921c9c2` S2-45 Replace stale S2-18 Web prompt with desktop-client prompt
  - `9c623fb` Desktop Client Form Architecture Update
  - `b5d0be7` feat(mobile): restore restart-resilient local snapshots (#99)
  - `6b22b69` feat(mobile): restore local duplicate precheck (#98)
  - `fb4d589` feat(mobile): connect selected media to local outbox draft (#97)
  - `1a63bf2` feat(mobile): restore local outbox foundation (#96)
  - `0015c7d` feat(mobile): restore Android media picker and selected media cache (#95)
- Coordination log entries reviewed:
  - GitHub issue #89: PR #96 manual coordination replay merged as `1a63bf2`.
  - GitHub issue #89: PR #97 manual selected-media handoff replay merged as `fb4d589`.
  - GitHub issue #89: PR #98 manual duplicate-precheck replay merged as `6b22b69`.
  - GitHub issue #89: PR #99 manual restart-snapshot replay merged as `b5d0be7`.
  - GitHub issue #89: PR #101 desktop client form architecture update merged as `9c623fb`.
  - GitHub issue #89: PR #102 stale S2-18 Web prompt replaced with desktop-client prompt as `921c9c2`.
  - GitHub issue #89: PR #103 desktop UI technology decision task merged as `787805f`.
  - GitHub issue #89: PR #104 desktop UI technology owner decision merged as `f22a4b8`.
- Handoff/task cards reviewed:
  - `docs/ops/korobochka-chatgpt-context.md`
  - `docs/handoffs/s1-11-coder3-android-integration.md`
  - `docs/handoffs/s1-11-backend-integration-note.md`
  - `docs/handoffs/s1-12-sprint1-quality-gate.md`
  - `docs/task-cards/MOB-CANON-IMPORT-01.txt`
  - `docs/task-cards/MOB-CANON-IMPORT-02.txt`
  - `docs/task-cards/MOB-CANON-IMPORT-03.txt`
  - `docs/task-cards/MOB-CANON-IMPORT-04.txt`
  - `docs/task-cards/MOB-CANON-IMPORT-05.txt`
  - `docs/task-cards/MOB-CANON-IMPORT-06.txt`
  - `docs/task-cards/MOB-CANON-IMPORT-07.txt`
  - `docs/reports/coder3-canonical-mobile-import.md`
  - `docs/reports/coder3-mobile-foundation-reconciliation.md`
  - `docs/task-cards/desktop-client-form-architecture-update.txt`
  - `docs/handoffs/S2_18_DESKTOP_CLIENT_UPLOAD_CONTROL_PLANE_PROMPT.md`
  - `docs/task-cards/C2-S2-18_desktop-client-upload-control-plane-integration.txt`

## Android foundation now in main
| Slice | PR | Main status | Capability | Runtime status known from previous reports | Notes |
| --- | --- | --- | --- | --- | --- |
| IMPORT-01 shell/navigation | #67 | merged in main | Android shell, navigation, Russian visible baseline, local shell-state stubs | baseline accepted before IMPORT-02..07 replay | Earlier foundation already present before the recent import chain. |
| IMPORT-02 media picker/cache | #95 | merged in main | Native video picker, native camera capture, selected-media descriptor/cache, selected media card | phone check previously passed | No App.UI.Shared change in replay slice. |
| IMPORT-03 local outbox | #96 | merged in main | Local in-memory outbox foundation, queue rendering, retry/remove local actions | phone check previously passed | No backend persistence or upload runtime. |
| IMPORT-04 selected media -> outbox draft | #97 | merged in main | Selected-media handoff into local outbox draft | import04 phone ok | Local handoff only. |
| IMPORT-05 local duplicate-precheck | #98 | merged in main | Local duplicate precheck against current selected media and outbox drafts | import05 phone ok | Local advisory duplicate behavior only. |
| IMPORT-06 restart snapshots | #99 | merged in main | JSON metadata snapshots and metadata-only restored state | import06 phone ok | Metadata snapshots only; no stream persistence or file copy. |
| IMPORT-07 repair/rebind | #100 | merged in main | Local repair/rebind for restored metadata-only drafts | import07 phone ok | Local repair only; no backend integration. |

## Current architecture boundary
- Mobile is an Android APK on .NET MAUI Blazor Hybrid.
- Server remains the control plane.
- Video bytes do not pass through `App.Api`.
- Direct upload/download remains client <-> site.
- Local outbox/offline foundation is advisory/local until backend sync contracts are bound.
- `App.UI.Shared` was not touched in IMPORT-02..07.
- Shared DTO/contracts remain unchanged by IMPORT-02..07.

## New cross-client context
- The primary desktop client is now a standalone desktop application.
- `App.Web` is not the primary desktop client.
- `App.Web` remains a non-primary web/admin/diagnostic/support surface.
- The S2-18 Web/PWA prompt is stale for primary desktop implementation.
- S2-18 Android remains broadly aligned with the mobile baseline.
- Do not implement desktop work from the Android role.

## Backend integration map for Android
| Area | Documented endpoint/path | Documented request/response fields | Android readiness | Blocker if not documented | Forbidden assumptions |
| --- | --- | --- | --- | --- | --- |
| auth/sign-in | No Android-specific endpoint found in reviewed S1 handoff docs. | No request/response fields found in reviewed S1 handoff docs. | Not ready beyond a future auth/session client seam. | Approved auth endpoint and payload for Android. | Do not invent auth routes, tokens, storage, or session binding. |
| auth/refresh | No refresh endpoint found in reviewed S1 handoff docs. | No refresh fields found in reviewed S1 handoff docs. | Not ready beyond a future refresh seam. | Approved refresh endpoint, token lifecycle, and failure behavior. | Do not invent refresh semantics. |
| group tree | No Android group tree endpoint found in reviewed S1 handoff docs. `groupNodeId` is required by upload metadata. | `groupNodeId` appears as a required upload metadata field. | Needs client seam after endpoint and source are approved. | Approved group tree endpoint and selected group source. | Do not derive group scope locally without approved source. |
| devices/register | No device registration endpoint found in reviewed S1 handoff docs. `deviceId` is required by upload metadata. | `deviceId` appears as a required upload metadata field. | Needs client seam after endpoint and device identity policy are approved. | Approved device registration endpoint and identifier policy. | Do not invent device binding or auth/session/device coupling. |
| health live/ready | `GET /health/live`, `GET /health/ready`. | No body fields required in reviewed docs. | Ready for a small health client in a backend adapter foundation slice. | None for basic health ping. | Do not treat health as auth/session readiness. |
| pre-upload-check | `POST /api/video-upload/pre-upload-check`. | Minimal fields: `userId`, `deviceId`, `groupNodeId`, `businessObjectKey`, `fileName`, `contentType`, `sizeBytes`, `byteSha256`. Decisions: `ALLOW`, `BLOCK_HARD_DUPLICATE`, `ALLOW_WITH_REVIEW`, `BLOCK_POSSIBLE_FALSIFICATION`. | Android can provide local media metadata/outbox context, but online binding is not ready. | Approved auth/user/group/device scope, `businessObjectKey` source, hashing policy, and direct-site adapter boundary. | Do not add `PreUploadCheck` client code until missing sources are explicit. |
| upload-receipt | `POST /api/video-upload/receipts`. | Minimal fields: `clientReceiptKey`, `userId`, `deviceId`, `groupNodeId`, `businessObjectKey`, `externalVideoId`, `fileName`, `contentType`, `sizeBytes`, `byteSha256`, `uploadedAtUtc`. Statuses: `accepted`, `duplicate_ignored`, `rejected`. | Not ready for runtime; receipt belongs after direct-site success or approved stub success. | Approved direct-site result model, `externalVideoId`, `businessObjectKey`, and online auth scope. | Do not emit `UploadReceipt` before direct-site boundary succeeds or an approved stub reports success. |
| direct site upload boundary | Handoffs state bytes upload directly client <-> site; `App.Api` is not a media proxy. Real provider payload is deferred. | No final provider payload documented. | Needs adapter boundary design before runtime. | Approved provider/stub contract and success/failure mapping. | Do not send video bytes through `App.Api`; do not invent provider payload. |
| download intent | `POST /api/video-download/intents`; backend docs also mention download intent status vocabulary. | Reviewed docs describe server-issued intent and direct `DownloadUrl` use. | Not ready for Android runtime in this checkpoint. | Approved Android download UX, auth scope, intent request shape, and stale-intent handling. | Do not download without server intent. |
| download receipt | `POST /api/video-download/receipts`. | Reviewed docs require receipt after server-issued intent/direct download flow. | Not ready for Android runtime in this checkpoint. | Approved receipt payload and retry/reconcile behavior. | Do not send a `DownloadReceipt` without a server intent and direct download outcome. |

## Upload control-plane readiness
- Android can now provide local media metadata and local outbox state.
- Android still needs online control-plane binding:
  - auth/session
  - current user/group/device scope
  - `businessObjectKey`
  - `PreUploadCheck`
  - direct site upload adapter boundary
  - `UploadReceipt`
- `UploadReceipt` must happen only after the direct-site boundary succeeds or an approved stub reports success.
- Video bytes must not go through `App.Api`.

## Critical blocker: businessObjectKey / create-report source
The reviewed handoff docs document `businessObjectKey` as a required upload metadata field, but they do not document the approved Android source for `businessObjectKey`, report draft, or business object binding before `PreUploadCheck`.

This is a blocker. Do not invent it.

Coordination question to coder 1:
"What is the approved Android source for businessObjectKey / report draft / business object binding before PreUploadCheck?"

## Report-first local UX decision
- Local report-first UX exists in old stacked work but is not yet replayed into main.
- Report-first UX may be useful as local mobile context.
- However, the first production slice remains upload control-plane readiness, not the reports engine.
- Report UX must remain local-only unless backend create-report/businessObjectKey contract is approved.
- Do not start production reports flow now.

## Next safe options
Option A:
MOB-CANON-BACKEND-01 - Android backend adapter foundation

Scope:
- endpoint/options model
- typed local API adapter interfaces
- health client
- auth/session client seam
- device/group client seam
- no upload runtime until businessObjectKey/source and direct-site boundary are explicit

Allowed only if:
- businessObjectKey/create-report/source is documented or explicitly scoped as unresolved adapter placeholder.

Option B:
MOB-CANON-REPORT-00/01 replay

Scope:
- local Android report draft shell only
- no backend save
- no businessObjectKey
- no production reports engine
- no PreUploadCheck/UploadReceipt

Allowed only if:
- owner accepts local report UX as non-production context while upload control-plane remains production priority.

Option C:
Coordination blocker

Scope:
- ask coder 1/owner for businessObjectKey/create-report decision before any backend code.

## Recommendation
Recommend Option C first, then Option B only if the owner wants local UX replay.

Reason:
- `businessObjectKey` source is not documented for Android.
- Production upload control-plane code would require inventing business object/report binding, which is forbidden.
- Production report flow should not start before upload control-plane decisions are explicit.

## Not allowed
- no backend code in this checkpoint
- no PreUploadCheck client code yet
- no UploadReceipt client code yet
- no App.UI.Shared changes
- no shared DTO/contracts changes
- no App.Api changes
- no desktop code from Android role
- no direct upload runtime without adapter boundary
- no secrets/tokens in docs
