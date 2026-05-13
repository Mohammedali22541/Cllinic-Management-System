namespace ClinicApp.Data.Entites.Common
{
    public class BaseEntity<TKey>
    {
        public TKey Id { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}