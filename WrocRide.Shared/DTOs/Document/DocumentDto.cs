﻿using WrocRide.Shared.Enums;

namespace WrocRide.Shared.DTOs.Document
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public string FileLocation { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
