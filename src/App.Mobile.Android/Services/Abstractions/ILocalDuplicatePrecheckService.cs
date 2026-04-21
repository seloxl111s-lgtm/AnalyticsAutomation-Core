namespace App.Mobile.Android.Services.Abstractions;

internal interface ILocalDuplicatePrecheckService
{
    global::App.Mobile.Android.DuplicatePrecheck.LocalDuplicatePrecheckResult CheckAgainstOutbox(
        global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor? currentSelection,
        IReadOnlyList<global::App.Mobile.Android.Outbox.PendingSyncItem> existingItems);
}
