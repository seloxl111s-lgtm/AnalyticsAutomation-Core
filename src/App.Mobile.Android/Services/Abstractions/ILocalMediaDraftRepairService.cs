namespace App.Mobile.Android.Services.Abstractions;

internal interface ILocalMediaDraftRepairService
{
    global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairResult CheckCurrentSelectionForDraftRepair(
        global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor? currentSelection,
        global::App.Mobile.Android.Outbox.PendingSyncItem? targetItem);
}