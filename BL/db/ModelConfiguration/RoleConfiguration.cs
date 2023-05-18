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
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(r => r.Title).HasMaxLength(50).IsRequired();
            builder.Property(r => r.Id).IsRequired();

            builder.HasIndex(r => r.Id).IsUnique();
            builder.HasIndex(r => r.Title).IsUnique();
        }
    }
}
