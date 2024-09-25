using EFC_Course.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFC_Course.Data.config
{
    // UserConfiguration class is used to configure the User entity.
    // It implements IEntityTypeConfiguration<User>,
    // which is an interface for configuring entities in Entity Framework Core.
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        // Configure method is where the actual configuration of the User entity should be defined.
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("tblUsers");
        }
    }




}
