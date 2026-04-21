namespace App.Mobile.Android.Services.Abstractions;

internal interface IMobileOutboxService
{
    Task<IReadOnlyList<global::App.Mobile.Android.Outbox.PendingSyncItem>> GetItemsAsync(
        CancellationToken cancellationToken = default);

    Task<global::App.Mobile.Android.Outbox.PendingSyncOperationResult> EnqueueStubItemAsync(
        CancellationToken cancellationToken = default);

    Task<global::App.Mobile.Android.Outbox.PendingSyncOperationResult> RetryAsync(
        string itemId,
        CancellationToken cancellationToken = default);

    Task<global::App.Mobile.Android.Outbox.PendingSyncOperationResult> RemoveAsync(
        string itemId,
        CancellationToken cancellationToken = default);
}
