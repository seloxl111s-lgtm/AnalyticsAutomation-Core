namespace App.Mobile.Android.Media;

internal sealed record LocalSelectedMediaStoreEntry(
    LocalSelectedMediaDescriptor Descriptor,
    Func<Task<global::System.IO.Stream>> OpenReadAsync);
