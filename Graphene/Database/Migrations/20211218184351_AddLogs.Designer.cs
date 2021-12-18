﻿// <auto-generated />
using System;
using Graphene.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Graphene.Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20211218184351_AddLogs")]
    partial class AddLogs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Graphene.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password");

                    b.Property<Guid>("Uid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("uid");

                    b.HasKey("Id")
                        .HasName("pk_authors");

                    b.ToTable("authors");
                });

            modelBuilder.Entity("Graphene.Models.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("deleted_at");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at");

                    b.Property<Guid>("Uid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("uid");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("url");

                    b.HasKey("Id")
                        .HasName("pk_blogs");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("blogs");
                });

            modelBuilder.Entity("Graphene.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Event")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("event");

                    b.Property<string>("From")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("from");

                    b.Property<int>("InstanceEntityState")
                        .HasColumnType("int")
                        .HasColumnName("instance_entity_state");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("model");

                    b.Property<int>("ModelId")
                        .HasColumnType("int")
                        .HasColumnName("model_id");

                    b.Property<Guid>("ModelUid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("model_uid");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at");

                    b.Property<string>("To")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("to");

                    b.Property<Guid>("Uid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("uid");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_logs");

                    b.ToTable("logs");
                });

            modelBuilder.Entity("Graphene.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int")
                        .HasColumnName("author_id");

                    b.Property<int>("BlogId")
                        .HasColumnType("int")
                        .HasColumnName("blog_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("deleted_at");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("title");

                    b.Property<Guid>("Uid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("uid");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_posts");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BlogId");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("posts");
                });

            modelBuilder.Entity("Graphene.Models.Post", b =>
                {
                    b.HasOne("Graphene.Models.Author", "Author")
                        .WithMany("Posts")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_posts_authors_author_id");

                    b.HasOne("Graphene.Models.Blog", "Blog")
                        .WithMany("Posts")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_posts_blogs_blog_id");

                    b.Navigation("Author");

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("Graphene.Models.Author", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("Graphene.Models.Blog", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
