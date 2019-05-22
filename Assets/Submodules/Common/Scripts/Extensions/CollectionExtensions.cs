using System.Collections;

public static class CollectionExtensions
{
    public static bool IsNullOrEmpty(this ICollection collection)
    {
        return collection == null || collection.Count == 0;
    }
}