using System;
using System.Collections.Generic;

namespace pumpk1n_backend.Models.TransferModels.Inventories
{
    public class InventoryExportModel
    {
        public long? CustomerId { get; set; }
        public DateTime ExportedDate { get; set; } = DateTime.Now;
        public List<long> InventoryItems { get; set; }
    }
}