using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mills.Database.Entities.User
{
    internal class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property(m => m.Id).HasColumnName("Id");
            builder.Property(m => m.Username).HasColumnName("Username").HasMaxLength(25).IsRequired();
            builder.Property(m => m.Password).HasColumnName("Password").HasMaxLength(255).IsRequired();
            builder.Property(m => m.MailAddress).HasColumnName("MailAddress").HasMaxLength(255);
        }
    }
}
