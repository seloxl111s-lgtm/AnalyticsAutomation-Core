namespace App.Mobile.Android.DraftRepair;

internal enum LocalMediaDraftRepairStatus
{
    NoCurrentSelection = 0,
    SelectionHasNoLiveHandle = 1,
    NoRepairableDraft = 2,
    DraftAlreadyHasLiveHandle = 3,
    SelectionDoesNotMatchDraft = 4,
    ReadyToRepair = 5
}