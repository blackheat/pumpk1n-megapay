using System;

namespace pumpk1n_backend.Models.ReturnModels
{
    public class PaginationReturnModel
    {
        public Int32 CurrentPage { get; set; }
        public Int32 TotalPages { get; set; }
        public Int32 ElementsPerPage { get; set; }
    }
}
