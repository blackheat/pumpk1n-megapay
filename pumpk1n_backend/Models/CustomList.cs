using System.Collections.Generic;
using pumpk1n_backend.Models.ReturnModels;

namespace pumpk1n_backend.Models
{
    public class CustomList<T> : List<T> where T : class
    {
        public int CurrentPage;
        public int TotalPages;
        public int TotalItems;
        public bool IsListPartial;
        
        public CustomList(int currentPage, int totalPages, int totalItems, bool isListPartial)
        {
            CurrentPage = currentPage;
            TotalPages = totalPages;
            TotalItems = totalItems;
            IsListPartial = isListPartial;
        }
        
        public CustomList() {}

        public PaginationReturnModel GetPaginationData()
        {
            return new PaginationReturnModel
            {
                CurrentPage = CurrentPage,
                ElementsPerPage = Count,
                TotalPages = TotalPages,
                TotalElements = TotalItems,
                IsListPartial = IsListPartial
            };
        }
    }
}