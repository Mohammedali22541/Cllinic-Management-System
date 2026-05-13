using ClinicApp.Data.Entites.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicApp.Data.Configurations
{
    public class BaseConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity<TKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(c => c.CreatedAt)
                        .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(c => c.IsDeleted)
                    .HasDefaultValue(false);
        }
    }
}