﻿using WrocRide.Shared.Enums;

namespace WrocRide.Shared.DTOs.Document
{
    public class DocumentQuery
    {
        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        public DocumentStatus? DocumentStatus { get; set; }
    }
}
