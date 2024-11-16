namespace R3M.Financas.Api.Extensions;

public static class IAsyncEnumerableExtensions
{
    public static async Task<IList<T>> ToListAsync<T>(this IAsyncEnumerable<T> target)
    {
        List<T> values = [];
        await foreach (var item in target)
        {
            values.Add(item);
        }
        return values;
    }
}
