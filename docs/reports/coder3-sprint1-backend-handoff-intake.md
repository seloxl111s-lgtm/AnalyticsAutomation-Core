# Coder 3 Sprint 1 Backend Handoff Intake

## Inputs used
- `docs/handoffs/s1-11-coder3-android-integration.md`
- `docs/handoffs/s1-11-backend-integration-note.md`
- `docs/handoffs/s1-12-sprint1-quality-gate.md`
- `docs/handoffs/s1-11-coder2-web-integration.md`

## Handoff status summary
- Backend integration note is present and documents Sprint 1 control-plane contracts and status vocabularies.
- Android integration handoff is present and documents the approved Android upload and download control-plane flow.
- Sprint 1 quality gate is present with `PASS_FOR_BACKEND_HANDOFF`.
- The quality gate explicitly says GO for coder 3 Android integration against documented contracts.
- The same quality gate also says `NOT_A_PRODUCTION_RELEASE`, NO-GO for inventing new client statuses, and NO-GO for changing shared contracts without new coordination.

## Endpoint map for Android
| Area | Method/path | Purpose | Documented request fields | Documented response/status vocabulary | Current Android usage | Forbidden assumptions |
| --- | --- | --- | --- | --- | --- | --- |
| auth/sign-in | not documented in inspected handoff docs | not documented in inspected handoff docs | not documented in inspected handoff docs | not documented in inspected handoff docs | not implemented in canonical Android code at intake time | do not invent auth endpoint shape or payloads |
| auth/refresh | not documented in inspected handoff docs | not documented in inspected handoff docs | not documented in inspected handoff docs | not documented in inspected handoff docs | not implemented in canonical Android code at intake time | do not invent refresh endpoint shape or token rules |
| devices/register | not documented in inspected handoff docs | not documented in inspected handoff docs | not documented in inspected handoff docs | not documented in inspected handoff docs | not implemented in canonical Android code at intake time | do not invent device registration request/response contracts |
| group tree | not documented in inspected handoff docs | not documented in inspected handoff docs | not documented in inspected handoff docs | not documented in inspected handoff docs | not implemented in canonical Android code at intake time | do not invent group tree endpoint shape or field vocabulary |
| platform foundation | not documented in inspected handoff docs | not documented in inspected handoff docs | not documented in inspected handoff docs | not documented in inspected handoff docs | not implemented in canonical Android code at intake time | do not invent bootstrap/current-user/platform payloads |
| observability | `GET /health/live` | stage liveness check | not documented in inspected handoff docs | health endpoint exists; response body is not documented in inspected handoff docs | not implemented in canonical Android code at intake time | do not invent extra health payload fields |
| observability | `GET /health/ready` | stage readiness check | not documented in inspected handoff docs | health endpoint exists; response body is not documented in inspected handoff docs | not implemented in canonical Android code at intake time | do not invent extra readiness payload fields |
| site gateway | `GET /api/website-integration/contracts` | inspect website integration contracts/stub baseline | not documented in inspected handoff docs | not documented in inspected handoff docs | not implemented in canonical Android code at intake time | do not treat preview or stub-facing gateway routes as production mobile upload runtime by default |
| site gateway | `POST /api/website-integration/upload-receipts/preview` | preview upload-receipt integration behavior | not documented in inspected handoff docs | not documented in inspected handoff docs | not implemented in canonical Android code at intake time | do not use preview-only endpoint as production upload runtime |
| site gateway | `POST /api/website-integration/download-intents/preview` | preview download-intent integration behavior | not documented in inspected handoff docs | not documented in inspected handoff docs | not implemented in canonical Android code at intake time | do not use preview-only endpoint as production download runtime |
| site gateway | `POST /api/website-integration/reconcile/preview` | preview reconcile behavior | not documented in inspected handoff docs | not documented in inspected handoff docs | not implemented in canonical Android code at intake time | do not use preview-only reconcile behavior as final mobile sync contract |
| pre-upload-check | `POST /api/video-upload/pre-upload-check` | server-side upload pre-check before direct site upload | `userId`, `deviceId`, `groupNodeId`, `businessObjectKey`, `fileName`, `contentType`, `sizeBytes`, `byteSha256` | decisions: `ALLOW`, `BLOCK_HARD_DUPLICATE`, `ALLOW_WITH_REVIEW`, `BLOCK_POSSIBLE_FALSIFICATION` | not implemented in canonical Android code at intake time; documented target for Sprint 1 upload flow | do not invent new decision values; do not invent real site provider payload |
| upload-receipt | `POST /api/video-upload/receipts` | report final upload receipt after direct site upload | `clientReceiptKey`, `userId`, `deviceId`, `groupNodeId`, `businessObjectKey`, `externalVideoId`, `fileName`, `contentType`, `sizeBytes`, `byteSha256`, `uploadedAtUtc` | statuses: `accepted`, `duplicate_ignored`, `rejected` | not implemented in canonical Android code at intake time; documented target after direct site upload | do not invent new receipt statuses; do not assume undocumented idempotency semantics beyond documented receipt fields/statuses |
| download intent | `POST /api/video-download/intents` | create server-issued download intent before direct site download | not documented in inspected handoff docs | intent statuses: `created`, `consumed`, `expired`, `rejected`; flow documents returned `DownloadUrl`, but exact payload shape is not documented in inspected handoff docs | not implemented in canonical Android code at intake time | do not create download receipt without a server-issued intent; do not invent exact `DownloadUrl` payload shape |
| download intent | `GET /api/video-download/intents/{downloadIntentId}` | read download intent state | not documented in inspected handoff docs | intent statuses: `created`, `consumed`, `expired`, `rejected` | not implemented in canonical Android code at intake time | do not invent extra download intent statuses |
| download receipt | `POST /api/video-download/receipts` | report download receipt after direct site download | not documented in inspected handoff docs | receipt statuses: `accepted`, `duplicate_ignored` | not implemented in canonical Android code at intake time | do not create receipt without intent; do not invent extra receipt statuses |
| download receipt | `GET /api/video-download/receipts/{downloadReceiptId}` | read download receipt state | not documented in inspected handoff docs | receipt statuses: `accepted`, `duplicate_ignored` | not implemented in canonical Android code at intake time | do not invent extra receipt statuses |
| download status vocabulary | `GET /api/video-download/status-vocabulary` | read current download status vocabulary | not documented in inspected handoff docs | download intent statuses: `created`, `consumed`, `expired`, `rejected`; download receipt statuses: `accepted`, `duplicate_ignored` | not implemented in canonical Android code at intake time | do not invent statuses beyond documented vocabularies |

## Upload flow map
Documented upload flow:
1. Client calculates local file metadata.
2. Client calls `POST /api/video-upload/pre-upload-check`.
3. If the decision allows upload, client uploads bytes directly to the site.
4. Client calls `POST /api/video-upload/receipts`.
5. Backend writes audit data and can queue `AnalyzeUploadedVideo`.
6. Duplicate or fraud incidents can be assigned later on the server side.

Documented PreUploadCheck metadata:
- `userId`
- `deviceId`
- `groupNodeId`
- `businessObjectKey`
- `fileName`
- `contentType`
- `sizeBytes`
- `byteSha256`

Documented UploadReceipt metadata:
- `clientReceiptKey`
- `userId`
- `deviceId`
- `groupNodeId`
- `businessObjectKey`
- `externalVideoId`
- `fileName`
- `contentType`
- `sizeBytes`
- `byteSha256`
- `uploadedAtUtc`

Documented decision and status vocabulary:
- PreUploadCheck: `ALLOW`, `BLOCK_HARD_DUPLICATE`, `ALLOW_WITH_REVIEW`, `BLOCK_POSSIBLE_FALSIFICATION`
- UploadReceipt: `accepted`, `duplicate_ignored`, `rejected`

Documented vs missing:
- `businessObjectKey` is documented as a required field for both PreUploadCheck and UploadReceipt.
- direct site upload is explicitly documented as the approved byte-transfer path.
- receipt status vocabulary is documented.
- exact UploadReceipt idempotency behavior is not fully documented in the inspected handoff docs, even though `clientReceiptKey` is present.
- report draft creation and create-report contract are not documented in the inspected handoff docs.

Blocker for full end-to-end upload:
- if Android must create or fetch the business object/report draft before PreUploadCheck, that contract is not documented in the inspected handoff docs and remains a blocker for a full end-to-end user flow.

## Offline upload map
Documented offline constraints:
- last active account only
- limited upload-related functionality only
- local outbox and local dedupe cache only
- absolute duplicate/fraud block is not guaranteed offline
- on reconnect, client sends UploadReceipt/sync payload
- server performs late duplicate/fraud detection after sync

Not documented in inspected handoff docs:
- final offline cache policy
- exact offline sync retry window
- exact offline storage retention rules

## Download flow map
Documented download flow:
1. Client calls `POST /api/video-download/intents`.
2. API returns `DownloadUrl`.
3. Client downloads bytes directly from the site.
4. Client calls `POST /api/video-download/receipts`.

Documented constraints:
- download intent creation is online-only
- do not create `DownloadReceipt` without a server-issued intent
- no offline download baseline is documented for this stage

Documented status vocabulary:
- DownloadIntent: `created`, `consumed`, `expired`, `rejected`
- DownloadReceipt: `accepted`, `duplicate_ignored`

Not documented in inspected handoff docs:
- exact request payload for `CreateDownloadIntent`
- exact response payload shape that contains `DownloadUrl`
- exact receipt request fields for download receipts

## What Android must not implement
- no worker job launching
- no direct incident creation
- no fraud/deep-analysis payload invention
- no new statuses
- no production upload runtime on preview-only endpoints
- no shared DTO/contracts changes

## Questions / blockers for coder 1
- stage base URL/environment source is still owner-provided according to the backend integration note
- auth/sign-in contract is not documented in the inspected handoff docs
- auth/refresh contract is not documented in the inspected handoff docs
- devices/register contract is not documented in the inspected handoff docs
- group tree endpoint and payload contract are not documented in the inspected handoff docs
- platform foundation/bootstrap/current-user payload is not documented in the inspected handoff docs
- exact `DownloadUrl` response shape is not documented in the inspected handoff docs
- report draft/create report contract is not documented in the inspected handoff docs
- UploadReceipt idempotency behavior beyond documented fields/statuses is not fully documented in the inspected handoff docs

## First safe S1 code step recommendation
Recommended next step:
- `MOB-CANON-IMPORT-01 — reconcile/import missing mobile foundation into canonical repo first`

Reason:
- the canonical `App.Mobile.Android` and `App.UI.Shared` projects are still template-level baseline
- the richer mobile foundation exists only in the prior backup source
- starting backend integration code directly in the canonical template would duplicate already-built foundation work and widen risk before reconciliation
