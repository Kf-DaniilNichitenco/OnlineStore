namespace Catalog.Views
{
    public class PageableResult<T>
    {
        public int Total { get; set; }
        public int Size { get; set; }
        public int Page { get; set; }
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
    }
}
