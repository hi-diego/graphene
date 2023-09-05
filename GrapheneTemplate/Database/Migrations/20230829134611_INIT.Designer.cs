﻿// <auto-generated />
using System;
using GrapheneTemplate.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GrapheneTemplate.Database.Migrations
{
    [DbContext(typeof(GrapheneCache))]
    [Migration("20230829134611_INIT")]
    partial class INIT
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Bill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("deleted_at");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("modified_at");

                    b.Property<int>("SpaceId")
                        .HasColumnType("int")
                        .HasColumnName("space_id");

                    b.Property<double>("Total")
                        .HasColumnType("double")
                        .HasColumnName("total");

                    b.Property<int>("Type")
                        .HasColumnType("int")
                        .HasColumnName("type");

                    b.Property<byte[]>("Uid")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("binary(16)")
                        .HasColumnName("uid")
                        .HasDefaultValueSql("(unhex(replace(uuid(),'-','')))");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_bills");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("SpaceId");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("bills");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int?>("BillId")
                        .HasColumnType("int")
                        .HasColumnName("bill_id");

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("comments");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int")
                        .HasColumnName("created_by_id");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("deleted_at");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("modified_at");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.Property<double>("Quantity")
                        .HasColumnType("double")
                        .HasColumnName("quantity");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.Property<byte[]>("Uid")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("binary(16)")
                        .HasColumnName("uid")
                        .HasDefaultValueSql("(unhex(replace(uuid(),'-','')))");

                    b.HasKey("Id")
                        .HasName("pk_orders");

                    b.HasIndex("BillId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("ProductId");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("orders");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<bool>("Available")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("available");

                    b.Property<string>("Category")
                        .HasColumnType("longtext")
                        .HasColumnName("category");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int")
                        .HasColumnName("created_by_id");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Description")
                        .HasColumnType("longtext")
                        .HasColumnName("description");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("modified_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.Property<string>("Packaging")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("packaging");

                    b.Property<double>("Price")
                        .HasColumnType("double")
                        .HasColumnName("price");

                    b.Property<string>("Subcategory")
                        .HasColumnType("longtext")
                        .HasColumnName("subcategory");

                    b.Property<byte[]>("Uid")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("binary(16)")
                        .HasColumnName("uid")
                        .HasDefaultValueSql("(unhex(replace(uuid(),'-','')))");

                    b.HasKey("Id")
                        .HasName("pk_products");

                    b.HasIndex("CreatedById");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("products");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Space", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("Capacity")
                        .HasColumnType("int")
                        .HasColumnName("capacity");

                    b.Property<int>("Configuration")
                        .HasColumnType("int")
                        .HasColumnName("configuration");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("description");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("modified_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.Property<int>("Type")
                        .HasColumnType("int")
                        .HasColumnName("type");

                    b.Property<byte[]>("Uid")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("binary(16)")
                        .HasColumnName("uid")
                        .HasDefaultValueSql("(unhex(replace(uuid(),'-','')))");

                    b.HasKey("Id")
                        .HasName("pk_spaces");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("spaces");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.Staff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("deleted_at");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("modified_at");

                    b.Property<int>("Role")
                        .HasColumnType("int")
                        .HasColumnName("role");

                    b.Property<int?>("SpaceId")
                        .HasColumnType("int")
                        .HasColumnName("space_id");

                    b.Property<byte[]>("Uid")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("binary(16)")
                        .HasColumnName("uid")
                        .HasDefaultValueSql("(unhex(replace(uuid(),'-','')))");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_staff");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("SpaceId");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("staff");
                });

            modelBuilder.Entity("GrapheneTemplate.Database.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("email");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("modified_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("password");

                    b.Property<byte[]>("Uid")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("binary(16)")
                        .HasColumnName("uid")
                        .HasDefaultValueSql("(unhex(replace(uuid(),'-','')))");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Uid")
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