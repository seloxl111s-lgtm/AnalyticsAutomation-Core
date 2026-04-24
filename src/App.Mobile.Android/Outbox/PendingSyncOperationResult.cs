namespace App.Mobile.Android.Outbox;

internal sealed record PendingSyncOperationResult(
    bool Applied,
    string Message,
    PendingSyncItem? Item);