﻿// <auto-generated />
using System;
using BL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BL.Migrations
{
    [DbContext(typeof(DbApplicationContext))]
    [Migration("20230418181450_AddedDateTimeToMessage")]
    partial class AddedDateTimeToMessage
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("BL.Model.AuthInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TIMESTAMP");

                    b.Property<byte[]>("IV")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<byte[]>("Key")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varbinary(250)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AuthInfos");
                });

            modelBuilder.Entity("BL.Model.Chat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TIMESTAMP");

                    b.Property<byte[]>("IV")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<byte[]>("Key")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Note")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("BL.Model.Friend", b =>
                {
                    b.Property<int>("UserFriendId")
                        .HasColumnType("int");

                    b.Property<int>("MainId")
                        .HasColumnType("int");

                    b.Property<bool>("IsSuccess")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("UserFriendId", "MainId");

                    b.HasIndex("MainId");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("BL.Model.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ChatId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("FatherId")
                        .HasColumnType("int");

                    b.Property<bool>("IsRead")
                        .HasColumnType("tinyint(1)");

                    b.Property<byte[]>("Text")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("varbinary(1000)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("FatherId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("BL.Model.MessageFiles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<byte[]>("File")
                        .IsRequired()
                        .HasColumnType("blob");

                    b.Property<int>("MessageId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("MessageId");

                    b.ToTable("MessagesFiles");
                });

            modelBuilder.Entity("BL.Model.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Title = "user"
                        });
                });

            modelBuilder.Entity("BL.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<DateTime?>("LastEntrance")
                        .HasColumnType("TIMESTAMP");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Note")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Phone")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varbinary(250)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BL.Model.UsersChats", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ChatId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Key")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.HasKey("UserId", "ChatId", "RoleId");

                    b.HasIndex("ChatId");

                    b.HasIndex("RoleId");

                    b.ToTable("UsersChats");
                });

            modelBuilder.Entity("BL.Model.AuthInfo", b =>
                {
                    b.HasOne("BL.Model.Role", "Role")
                        .WithMany("AuthInfos")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BL.Model.User", "User")
                        .WithMany("AuthInfos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BL.Model.Friend", b =>
                {
                    b.HasOne("BL.Model.User", "Main")
                        .WithMany("Mains")
                        .HasForeignKey("MainId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BL.Model.User", "UserFriend")
                        .WithMany("Friends")
                        .HasForeignKey("UserFriendId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Main");

                    b.Navigation("UserFriend");
                });

            modelBuilder.Entity("BL.Model.Message", b =>
                {
                    b.HasOne("BL.Model.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BL.Model.Message", "Father")
                        .WithOne("Child")
                        .HasForeignKey("BL.Model.Message", "FatherId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BL.Model.User", "User")
                        .WithMany("Messages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("Father");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BL.Model.MessageFiles", b =>
                {
                    b.HasOne("BL.Model.Message", "Message")
                        .WithMany("MessageFiles")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Message");
                });

            modelBuilder.Entity("BL.Model.UsersChats", b =>
                {
                    b.HasOne("BL.Model.Chat", "Chat")
                        .WithMany("UsersChats")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BL.Model.Role", "Role")
                        .WithMany("UsersChats")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BL.Model.User", "User")
                        .WithMany("UsersChats")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BL.Model.Chat", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("UsersChats");
                });

            modelBuilder.Entity("BL.Model.Message", b =>
                {
                    b.Navigation("Child");

                    b.Navigation("MessageFiles");
                });

            modelBuilder.Entity("BL.Model.Role", b =>
                {
                    b.Navigation("AuthInfos");

                    b.Navigation("UsersChats");
                });

            modelBuilder.Entity("BL.Model.User", b =>
                {
                    b.Navigation("AuthInfos");

                    b.Navigation("Friends");

                    b.Navigation("Mains");

                    b.Navigation("Messages");

                    b.Navigation("UsersChats");
                });
#pragma warning restore 612, 618
        }
    }
}
