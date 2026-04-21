namespace App.Mobile.Android.DuplicatePrecheck;

internal sealed record LocalDuplicatePrecheckResult(
    LocalDuplicatePrecheckStatus Status,
    bool CanEnqueue,
    string Message);
