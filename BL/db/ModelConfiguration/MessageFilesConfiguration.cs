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
    public class MessageFilesConfiguration : IEntityTypeConfiguration<MessageFiles>
    {
        public void Configure(EntityTypeBuilder<MessageFiles> builder)
        {
            builder.Property(mf => mf.File).HasColumnType("blob");
            builder.Property(mf => mf.Id).IsRequired();

            builder.HasIndex(mf => mf.Id).IsUnique();

            builder
                .HasOne(mf => mf.Message)
                .WithMany(m => m.MessageFiles)
                .HasForeignKey(mf => mf.MessageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
