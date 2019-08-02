namespace pumpk1n_backend.Models.ReturnModels
{
    public class PaginationReturnModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ElementsPerPage { get; set; }
        public int TotalElements { get; set; }
        public bool IsListPartial { get; set; }
    }
}
