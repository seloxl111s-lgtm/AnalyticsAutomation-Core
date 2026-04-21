# Coder 3 Report-First Mobile Plan

## Why this correction exists
- previous import slices focused on media and outbox foundation
- the owner clarified that the primary user flow is report or flight creation
- video is attached to a report, not the top-level user object
- reference app evidence confirms local report first, media attachment second, server id later, and upload lifecycle after report sync

## Reference behavior captured
- local report or flight is created first
- video is added to the local report
- video is copied or stored separately in the reference app
- video has a separate upload lifecycle
- server report id appears later
- video upload binds to server report id later
- observed fields:
  - `device_type`
  - `serial_number`
  - `delivery_start`
  - `delivery_time`
  - `distance`
  - `target_type`
  - `reason`
  - `comment`
  - `radio_frequency`
  - `video_frequency`
  - `test_flight`

## Correct mobile object model direction
Local-only foundation names, not shared DTO/contracts:
- `MobileReportDraft`
- `MobileReportDraftStatus`
- `MobileReportAttachment`
- `MobileReportAttachmentKind`
- `MobileReportDraftFieldValue`
- `IMobileReportDraftStore`
- `InMemoryMobileReportDraftStore`
- `StubMobileReportLookupProvider`

Direction:
- `MobileReportDraft` is the parent
- video, photo, and log files are attachments
- selected-media cache feeds attachment creation
- an outbox item should eventually reference `reportDraftId` and `attachmentId`
- `businessObjectKey` must later come from a backend or report contract, not be invented locally

## Existing foundation reinterpretation
- `AndroidNativeMediaService` = attachment source provider
- `IMobileSelectedMediaStore` = temporary selected attachment cache
- `IMobileOutboxService` = local pending attachment or report action queue
- duplicate-precheck = local attachment duplicate warning only
- snapshot stores = local draft and attachment restart-resilience foundation
- repair/rebind = restored attachment local-file access repair

## Proposed UX direction
- bottom navigation should eventually move toward a `Полеты` / `Профиль` style
- the first report screen should be a local `Полеты` / `Отчеты` list
- the create action should expose `Создать отчет FPV`
- the report draft form should be sectioned
- media blocks should live inside the report draft:
  - video
  - ready-drone photo
  - log file
- lookup-heavy fields require searchable selector surfaces
- some selectors require confirm or apply behavior

## What stays mobile-local for now
- report draft shell
- local draft state
- local lookup stub provider
- selector components
- report form sections
- attachment blocks
- Android media binding
- outbox and draft repair and restart behavior

## What must wait for coder 1
- create report or draft contract
- `businessObjectKey` source
- report server id or remote id semantics
- validation ownership
- lookup or reference catalog endpoints
- catalog versioning, freshness, and offline rules
- exact upload binding between report and `UploadReceipt`
- auth, session, device, and group scope

## What must wait for coder 2
- promotion of generic form controls to `App.UI.Shared`
- promotion of lookup selector UI to `App.UI.Shared`
- shared visual language for forms
- shared status and error components
- web compatibility of report form components

## What is not allowed
- no hardcoded final lookup dictionaries
- no backend contract invention
- no shared DTO/contracts changes
- no `App.UI.Shared` expansion without coder 2 review
- no production upload flow from report draft yet
- no direct incident, fraud, or worker logic in mobile
- no React Native or Expo copy

## Safe next code step
Recommend:
- `MOB-CANON-REPORT-01 - Local FPV report draft shell in App.Mobile.Android`

Scope:
- `App.Mobile.Android` only
- local route or list for report drafts
- create local FPV draft
- local sectioned draft screen
- local in-memory draft store
- stub lookup provider
- attach the currently selected video to the draft as local attachment metadata
- no backend save
- no `PreUploadCheck`
- no `UploadReceipt`
- no `App.UI.Shared` changes
