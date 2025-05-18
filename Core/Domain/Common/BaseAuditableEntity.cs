namespace Domain.Common
{
    public class BaseAuditableEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

        public Guid? Deleted {  get; set; }
        public DateTime DeletedDate { get; set; }
    }
}
