
namespace ReminderAIBot.Presentation
{
    public class PagedResult<T>
    {
        public List<T> Items { get; }

        public int ItemsCount => this.Items.Count;

        public int CurrentPage { get; }
        public int TotalPages { get; }

        public int PrevPage => this.HasPrevPage ? this.CurrentPage - 1 : this.CurrentPage;
        public int NextPage => this.HasNextPage ? this.CurrentPage + 1 : this.CurrentPage;

        public bool HasPrevPage => this.CurrentPage > 0;
        public bool HasNextPage => this.CurrentPage + 1 < this.TotalPages;


        public PagedResult()
        {
            this.Items = new();

            this.CurrentPage = 0;
            this.TotalPages = 0;
        }

        public PagedResult(List<T> items, int currentPage, int totalPages)
        {
            this.Items = items;

            this.CurrentPage = currentPage;
            this.TotalPages = totalPages;
        }
    }
}
