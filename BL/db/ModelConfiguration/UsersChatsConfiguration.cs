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
    public class UsersChatsConfiguration : IEntityTypeConfiguration<UsersChats>
    {
        public void Configure(EntityTypeBuilder<UsersChats> builder)
        {
            builder.HasKey(uc => new { uc.UserId, uc.ChatId, uc.RoleId });

            builder
                .HasOne(uc => uc.User)
                .WithMany(u => u.UsersChats)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(uc => uc.Chat)
                .WithMany(c => c.UsersChats)
                .HasForeignKey(uc => uc.ChatId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(uc => uc.Role)
                .WithMany(r => r.UsersChats)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
