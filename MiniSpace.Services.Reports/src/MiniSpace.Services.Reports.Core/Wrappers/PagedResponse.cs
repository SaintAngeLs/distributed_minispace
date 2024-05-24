namespace MiniSpace.Services.Reports.Core.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public int TotalPages { get; }
        public int TotalElements { get; }
        public int Size { get; }
        public int Number { get; }
        public bool First { get; }
        public bool Last { get; }
        public bool Empty { get; }

        public PagedResponse(T content, int pageNumber, int pageSize, int totalPages, int totalElements)
        {
            Content = content;
            TotalPages = totalPages;
            TotalElements = totalElements;
            Size = pageSize;
            Number = pageNumber;
            First = pageNumber == 0;
            Last = pageNumber == totalPages - 1;
            Empty = totalElements == 0;
            Succeeded = true;
            Errors = null;
            Message = null;
        }
    }
}