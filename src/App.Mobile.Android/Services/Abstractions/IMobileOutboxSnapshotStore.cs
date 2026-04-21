namespace App.Mobile.Android.Services.Abstractions;

internal interface IMobileOutboxSnapshotStore
{
    Task<IReadOnlyList<global::App.Mobile.Android.Outbox.PendingSyncItem>> LoadAsync(
        CancellationToken cancellationToken = default);

    Task SaveAsync(
        IReadOnlyList<global::App.Mobile.Android.Outbox.PendingSyncItem> items,
        CancellationToken cancellationToken = default);
}
