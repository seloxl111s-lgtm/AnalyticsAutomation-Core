namespace App.Mobile.Android.DraftRepair;

internal sealed record LocalMediaDraftRepairResult(
    LocalMediaDraftRepairStatus Status,
    bool CanApply,
    string Message);
