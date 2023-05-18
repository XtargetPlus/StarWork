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
    public class FriendConfiguration : IEntityTypeConfiguration<Friend>
    {
        public void Configure(EntityTypeBuilder<Friend> builder)
        {
            builder.HasKey(f => new { f.UserFriendId, f.MainId });

            builder
                .HasOne(f => f.UserFriend)
                .WithMany(uf => uf.Friends)
                .HasForeignKey(f => f.UserFriendId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(f => f.Main)
                .WithMany(m => m.Mains)
                .HasForeignKey(f => f.MainId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
