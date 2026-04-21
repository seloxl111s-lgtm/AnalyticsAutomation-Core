namespace App.Mobile.Android.Services.Stubs;

internal sealed class StubMobileOutboxService :
    global::App.Mobile.Android.Services.Abstractions.IMobileOutboxService
{
    private static readonly Action<global::Microsoft.Extensions.Logging.ILogger, string, Exception?> LogMediaDraftEnqueued =
        global::Microsoft.Extensions.Logging.LoggerMessage.Define<string>(
            global::Microsoft.Extensions.Logging.LogLevel.Information,
            new global::Microsoft.Extensions.Logging.EventId(1000, nameof(LogMediaDraftEnqueued)),
            "Enqueued local pending sync media draft item {ItemId}");

    private static readonly Action<global::Microsoft.Extensions.Logging.ILogger, string, Exception?> LogEnqueued =
        global::Microsoft.Extensions.Logging.LoggerMessage.Define<string>(
            global::Microsoft.Extensions.Logging.LogLevel.Information,
            new global::Microsoft.Extensions.Logging.EventId(1001, nameof(LogEnqueued)),
            "Enqueued local pending sync stub item {ItemId}");

    private static readonly Action<global::Microsoft.Extensions.Logging.ILogger, string, Exception?> LogRetried =
        global::Microsoft.Extensions.Logging.LoggerMessage.Define<string>(
            global::Microsoft.Extensions.Logging.LogLevel.Information,
            new global::Microsoft.Extensions.Logging.EventId(1002, nameof(LogRetried)),
            "Retried local pending sync stub item {ItemId}");

    private static readonly Action<global::Microsoft.Extensions.Logging.ILogger, string, Exception?> LogRemoved =
        global::Microsoft.Extensions.Logging.LoggerMessage.Define<string>(
            global::Microsoft.Extensions.Logging.LogLevel.Information,
            new global::Microsoft.Extensions.Logging.EventId(1003, nameof(LogRemoved)),
            "Removed local pending sync stub item {ItemId}");

    private readonly object _gate = new();
    private readonly List<global::App.Mobile.Android.Outbox.PendingSyncItem> _items = [];
    private readonly Dictionary<string, Func<Task<global::System.IO.Stream>>> _itemReadFactories = [];
    private readonly global::Microsoft.Extensions.Logging.ILogger<StubMobileOutboxService> _logger;
    private readonly global::App.Mobile.Android.Services.Abstractions.IMobileSelectedMediaStore _selectedMediaStore;
    private int _sequence;

    public StubMobileOutboxService(
        global::App.Mobile.Android.Services.Abstractions.IMobileSelectedMediaStore selectedMediaStore,
        global::Microsoft.Extensions.Logging.ILogger<StubMobileOutboxService> logger)
    {
        _selectedMediaStore = selectedMediaStore;
        _logger = logger;
    }

    public Task<IReadOnlyList<global::App.Mobile.Android.Outbox.PendingSyncItem>> GetItemsAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        lock (_gate)
        {
            IReadOnlyList<global::App.Mobile.Android.Outbox.PendingSyncItem> items =
            [
                .. _items.OrderByDescending(item => item.CreatedAtUtc)
            ];

            return Task.FromResult(items);
        }
    }

    public async Task<global::App.Mobile.Android.Outbox.PendingSyncOperationResult> EnqueueCurrentSelectionAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentSelectionEntry = await _selectedMediaStore.TakeCurrentAsync(cancellationToken);
        if (currentSelectionEntry is null)
        {
            return new global::App.Mobile.Android.Outbox.PendingSyncOperationResult(
                Applied: false,
                Message: global::App.Mobile.Android.Localization.MobileUiText.PendingSyncNoCurrentSelectionText,
                Item: null);
        }

        global::App.Mobile.Android.Outbox.PendingSyncItem item;

        lock (_gate)
        {
            _sequence++;

            item = new global::App.Mobile.Android.Outbox.PendingSyncItem(
                ItemId: $"pending-sync-{_sequence}",
                CreatedAtUtc: DateTimeOffset.UtcNow,
                Title: global::App.Mobile.Android.Localization.MobileUiText.GetPendingSyncMediaDraftTitle(
                    currentSelectionEntry.Descriptor.FileName),
                SummaryText: global::App.Mobile.Android.Localization.MobileUiText.PendingSyncMediaDraftSummary,
                Status: global::App.Mobile.Android.Outbox.PendingSyncItemStatus.Queued,
                LastActionText: global::App.Mobile.Android.Localization.MobileUiText.PendingSyncMediaDraftEnqueuedLastAction,
                LocalMediaDraft: new global::App.Mobile.Android.Outbox.PendingSyncItemLocalMediaDraft(
                    CacheKey: currentSelectionEntry.Descriptor.CacheKey,
                    Source: currentSelectionEntry.Descriptor.Source,
                    FileName: currentSelectionEntry.Descriptor.FileName,
                    ContentType: currentSelectionEntry.Descriptor.ContentType,
                    SelectedAtUtc: currentSelectionEntry.Descriptor.SelectedAtUtc,
                    HasLocalReadHandle: currentSelectionEntry.Descriptor.HasLocalReadHandle));

            _items.Insert(0, item);
            _itemReadFactories[item.ItemId] = currentSelectionEntry.OpenReadAsync;
        }

        LogMediaDraftEnqueued(_logger, item.ItemId, null);

        return new global::App.Mobile.Android.Outbox.PendingSyncOperationResult(
            Applied: true,
            Message: global::App.Mobile.Android.Localization.MobileUiText.GetPendingSyncMediaDraftHandoffResultText(
                item.LocalMediaDraft!.FileName),
            Item: item);
    }

    public Task<global::App.Mobile.Android.Outbox.PendingSyncOperationResult> EnqueueStubItemAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        global::App.Mobile.Android.Outbox.PendingSyncItem item;

        lock (_gate)
        {
            _sequence++;

            item = new global::App.Mobile.Android.Outbox.PendingSyncItem(
                ItemId: $"pending-sync-{_sequence}",
                CreatedAtUtc: DateTimeOffset.UtcNow,
                Title: global::App.Mobile.Android.Localization.MobileUiText.GetPendingSyncItemTitle(_sequence),
                SummaryText: global::App.Mobile.Android.Localization.MobileUiText.PendingSyncItemSummary,
                Status: global::App.Mobile.Android.Outbox.PendingSyncItemStatus.Queued,
                LastActionText: global::App.Mobile.Android.Localization.MobileUiText.PendingSyncEnqueuedLastAction,
                LocalMediaDraft: null);

            _items.Insert(0, item);
        }

        LogEnqueued(_logger, item.ItemId, null);

        return Task.FromResult(
            new global::App.Mobile.Android.Outbox.PendingSyncOperationResult(
                Applied: true,
                Message: global::App.Mobile.Android.Localization.MobileUiText.GetPendingSyncEnqueueResultText(item.Title),
                Item: item));
    }

    public Task<global::App.Mobile.Android.Outbox.PendingSyncOperationResult> RetryAsync(
        string itemId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        global::App.Mobile.Android.Outbox.PendingSyncItem? updatedItem = null;

        lock (_gate)
        {
            var index = _items.FindIndex(item => item.ItemId == itemId);
            if (index < 0)
            {
                return Task.FromResult(
                    new global::App.Mobile.Android.Outbox.PendingSyncOperationResult(
                        Applied: false,
                        Message: global::App.Mobile.Android.Localization.MobileUiText.PendingSyncItemNotFoundText,
                        Item: null));
            }

            updatedItem = _items[index] with
            {
                Status = global::App.Mobile.Android.Outbox.PendingSyncItemStatus.RetryRequested,
                LastActionText = global::App.Mobile.Android.Localization.MobileUiText.PendingSyncRetriedLastAction
            };

            _items[index] = updatedItem;
        }

        LogRetried(_logger, updatedItem!.ItemId, null);

        return Task.FromResult(
            new global::App.Mobile.Android.Outbox.PendingSyncOperationResult(
                Applied: true,
                Message: global::App.Mobile.Android.Localization.MobileUiText.GetPendingSyncRetryResultText(updatedItem.Title),
                Item: updatedItem));
    }

    public Task<global::App.Mobile.Android.Outbox.PendingSyncOperationResult> RemoveAsync(
        string itemId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        global::App.Mobile.Android.Outbox.PendingSyncItem? removedItem = null;

        lock (_gate)
        {
            var index = _items.FindIndex(item => item.ItemId == itemId);
            if (index < 0)
            {
                return Task.FromResult(
                    new global::App.Mobile.Android.Outbox.PendingSyncOperationResult(
                        Applied: false,
                        Message: global::App.Mobile.Android.Localization.MobileUiText.PendingSyncItemNotFoundText,
                        Item: null));
            }

            removedItem = _items[index];
            _items.RemoveAt(index);
            _itemReadFactories.Remove(removedItem.ItemId);
        }

        LogRemoved(_logger, removedItem!.ItemId, null);

        return Task.FromResult(
            new global::App.Mobile.Android.Outbox.PendingSyncOperationResult(
                Applied: true,
                Message: global::App.Mobile.Android.Localization.MobileUiText.GetPendingSyncRemoveResultText(removedItem.Title),
                Item: removedItem));
    }
}