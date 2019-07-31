using System.Collections.Generic;

namespace pumpk1n_backend.Models
{
    public class CustomList<T> : List<T> where T : class
    {
        public int StartAt;
        public int EndAt;
        public int Total;
        public bool IsListPartial;
        
        public CustomList(int startAt, int endAt, int total, bool isListPartial)
        {
            StartAt = startAt;
            EndAt = endAt;
            Total = total;
            IsListPartial = isListPartial;
        }
        
        public CustomList() {}
    }
}