﻿// <auto-generated />
using System;
using GrapheneTemplate.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GrapheneTemplate.Database.Migrations.Sqlite
{
    [DbContext(typeof(GrapheneCache))]
    partial class GrapheneCacheModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.9");

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Bill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("deleted_at");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("modified_at");

                    b.Property<int>("SpaceId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("space_id");

                    b.Property<double>("Total")
                        .HasColumnType("REAL")
                        .HasColumnName("total");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER")
                        .HasColumnName("type");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("user_id");

                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BLOB")
                        .HasColumnName("uuid")
                        .HasDefaultValueSql("randomblob(16)");

                    b.HasKey("Id")
                        .HasName("pk_bills");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("SpaceId");

                    b.HasIndex("UserId");

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("bills");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int?>("BillId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("bill_id");

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("comments");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_at");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("INTEGER")
                        .HasColumnName("created_by_id");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("deleted_at");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("modified_at");

                    b.Property<int?>("ProductId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("product_id");

                    b.Property<double>("Quantity")
                        .HasColumnType("REAL")
                        .HasColumnName("quantity");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER")
                        .HasColumnName("status");

                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BLOB")
                        .HasColumnName("uuid")
                        .HasDefaultValueSql("randomblob(16)");

                    b.HasKey("Id")
                        .HasName("pk_orders");

                    b.HasIndex("BillId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("ProductId");

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("orders");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<bool>("Available")
                        .HasColumnType("INTEGER")
                        .HasColumnName("available");

                    b.Property<string>("Category")
                        .HasColumnType("TEXT")
                        .HasColumnName("category");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_at");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("INTEGER")
                        .HasColumnName("created_by_id");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT")
                        .HasColumnName("description");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("modified_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.Property<string>("Packaging")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("packaging");

                    b.Property<double>("Price")
                        .HasColumnType("REAL")
                        .HasColumnName("price");

                    b.Property<string>("Subcategory")
                        .HasColumnType("TEXT")
                        .HasColumnName("subcategory");

                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BLOB")
                        .HasColumnName("uuid")
                        .HasDefaultValueSql("randomblob(16)");

                    b.HasKey("Id")
                        .HasName("pk_products");

                    b.HasIndex("CreatedById");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("products");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Space", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int>("Capacity")
                        .HasColumnType("INTEGER")
                        .HasColumnName("capacity");

                    b.Property<int>("Configuration")
                        .HasColumnType("INTEGER")
                        .HasColumnName("configuration");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("description");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("modified_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER")
                        .HasColumnName("type");

                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BLOB")
                        .HasColumnName("uuid")
                        .HasDefaultValueSql("randomblob(16)");

                    b.HasKey("Id")
                        .HasName("pk_spaces");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("spaces");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Staff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("deleted_at");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("modified_at");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER")
                        .HasColumnName("role");

                    b.Property<int?>("SpaceId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("space_id");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("user_id");

                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BLOB")
                        .HasColumnName("uuid")
                        .HasDefaultValueSql("randomblob(16)");

                    b.HasKey("Id")
                        .HasName("pk_staff");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("SpaceId");

                    b.HasIndex("UserId");

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("staff");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("email");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("modified_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("password");

                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BLOB")
                        .HasColumnName("uuid")
                        .HasDefaultValueSql("randomblob(16)");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Bill", b =>
                {
                    b.HasOne("GrapheneTemplate.Database.Models.Space", "Space")
                        .WithMany("Bills")
                        .HasForeignKey("SpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_bills_space_space_id");

                    b.HasOne("GrapheneTemplate.Database.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_bills_user_user_id");

                    b.Navigation("Space");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Order", b =>
                {
                    b.HasOne("GrapheneTemplate.Database.Models.Bill", "Bill")
                        .WithMany("Orders")
                        .HasForeignKey("BillId")
                        .HasConstraintName("fk_orders_bills_bill_id");

                    b.HasOne("GrapheneTemplate.Database.Models.User", "CreatedBy")
                        .WithMany("OrderHistory")
                        .HasForeignKey("CreatedById")
                        .HasConstraintName("fk_orders_user_created_by_id");

                    b.HasOne("GrapheneTemplate.Database.Models.Product", "Product")
                        .WithMany("Orders")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("fk_orders_product_product_id");

                    b.Navigation("Bill");

                    b.Navigation("CreatedBy");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Product", b =>
                {
                    b.HasOne("GrapheneTemplate.Database.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .HasConstraintName("fk_products_user_created_by_id");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Staff", b =>
                {
                    b.HasOne("GrapheneTemplate.Database.Models.Space", "Space")
                        .WithMany()
                        .HasForeignKey("SpaceId")
                        .HasConstraintName("fk_staff_spaces_space_id");

                    b.HasOne("GrapheneTemplate.Database.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_staff_user_user_id");

                    b.Navigation("Space");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Bill", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Product", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Space", b =>
                {
                    b.Navigation("Bills");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.User", b =>
                {
                    b.Navigation("OrderHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
