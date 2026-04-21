namespace App.Mobile.Android.Outbox;

internal enum PendingSyncItemStatus
{
    Unknown,
    Queued,
    RetryRequested
}
