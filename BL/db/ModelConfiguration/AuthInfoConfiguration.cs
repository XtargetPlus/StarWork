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
    public class AuthInfoConfiguration : IEntityTypeConfiguration<AuthInfo>
    {
        public void Configure(EntityTypeBuilder<AuthInfo> builder)
        {
            builder.Property(ai => ai.Id).IsRequired();
            builder.Property(ai => ai.Key).IsRequired();
            builder.Property(ai => ai.IV).IsRequired();
            builder.Property(ai => ai.Salt).IsRequired();
            builder.Property(ai => ai.Login).HasMaxLength(250).IsRequired(); 
            builder.Property(ai => ai.Password).HasMaxLength(250).IsRequired();
            builder.Property(ai => ai.Created).HasColumnType("TIMESTAMP");

            builder.HasIndex(ai => ai.Login).IsUnique();
            builder.HasIndex(ai => ai.Id).IsUnique();

            builder
                .HasOne(ai => ai.User)
                .WithMany(u => u.AuthInfos)
                .HasForeignKey(ai => ai.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(ai => ai.Role)
                .WithMany(r => r.AuthInfos)
                .HasForeignKey(ai => ai.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
