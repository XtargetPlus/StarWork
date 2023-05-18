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
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.Property(c => c.Title).HasMaxLength(50).IsRequired();
            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.Key).IsRequired();
            builder.Property(c => c.IV).IsRequired();
            builder.Property(c => c.Salt).IsRequired();
            builder.Property(c => c.Created).HasColumnType("TIMESTAMP");

            builder.HasIndex(c => c.Id).IsUnique();
            builder.HasIndex(c => c.Title).IsUnique();
        }
    }
}
