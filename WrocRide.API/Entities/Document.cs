﻿namespace WrocRide.API.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public string FileLocation { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ExaminationDate { get; set; }
        public int? AdminId { get; set; }
        public virtual Admin Admin { get; set; }
    }
}
