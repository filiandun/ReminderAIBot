
namespace ReminderAIBot.Models.UI
{
    public class PagedList<T>
    {
        private readonly List<T> _items;

        public int CountItems => this._items.Count;
        public int CountPages => this.CountItems == 0 ? 0 : (int)Math.Ceiling(this.CountItems / (double)this.PageSize);
        public int PageSize { get; init; }


        public PagedList(IEnumerable<T> items, int pageSize)
        {
            this._items = items.ToList();

            this.PageSize = pageSize;
        }


        public IReadOnlyList<T> GetPage(int page)
        {
            if (this.CountPages == 0) return new List<T>();

            page = Math.Max(0, Math.Min(page, this.CountPages - 1));

            return this._items.Skip(page * this.PageSize).Take(this.PageSize).ToList();
        }
    }
}
