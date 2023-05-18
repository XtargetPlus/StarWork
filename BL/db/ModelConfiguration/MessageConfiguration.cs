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
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(m => m.Text).HasMaxLength(1000);
            builder.Property(m => m.Id).IsRequired();

            builder.HasIndex(m => m.FatherId).IsUnique(false);
            builder.HasIndex(m => m.Id).IsUnique();

            builder
                .HasOne(m => m.Father)
                .WithOne(f => f.Child)
                .HasForeignKey<Message>(m => m.FatherId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(m => m.Child)
                .WithOne(f => f.Father)
                .HasForeignKey<Message>(m => m.ChildId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
