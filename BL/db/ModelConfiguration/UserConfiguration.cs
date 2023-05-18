using BL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.db.ModelConfiguration
{

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.NickName).HasMaxLength(100).IsRequired();
            builder.Property(u => u.Id).IsRequired();
            builder.Property(u => u.Age).IsRequired();
            builder.Property(u => u.Phone).HasMaxLength(250).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(250);
            builder.Property(u => u.LastEntrance).HasColumnType("TIMESTAMP");

            builder.HasIndex(u => u.Id).IsUnique();
            builder.HasIndex(u => u.NickName).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}
