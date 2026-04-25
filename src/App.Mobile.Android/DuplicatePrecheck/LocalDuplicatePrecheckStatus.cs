namespace App.Mobile.Android.DuplicatePrecheck;

internal enum LocalDuplicatePrecheckStatus
{
    NoCurrentSelection = 0,
    NoKnownDuplicateInOutbox = 1,
    LikelyAlreadyQueued = 2
}