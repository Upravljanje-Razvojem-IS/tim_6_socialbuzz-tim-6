﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReactionService.Entities;

namespace ReactionService.Migrations
{
    [DbContext(typeof(ReactionDbContext))]
    [Migration("20210726185455_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ReactionService.Entities.Reaction", b =>
                {
                    b.Property<Guid>("ReactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ReactionTypeId")
                        .HasColumnType("int");

                    b.HasKey("ReactionId");

                    b.ToTable("Reactions");

                    b.HasData(
                        new
                        {
                            ReactionId = new Guid("3b6d0a06-e64b-4f42-8689-fc10e8e6edf7"),
                            AccountId = new Guid("42b70088-9dbd-4b19-8fc7-16414e94a8a6"),
                            PostId = new Guid("15908a81-dcae-43e7-fecb-08d94eb2a3fe"),
                            ReactionTypeId = 1
                        },
                        new
                        {
                            ReactionId = new Guid("d8fe100b-8d5f-4027-961a-fa75bf8a3b94"),
                            AccountId = new Guid("59ed7d80-39c9-42b8-a822-70ddd295914a"),
                            PostId = new Guid("8ccb1467-9f38-4164-88da-15882fe82e58"),
                            ReactionTypeId = 2
                        },
                        new
                        {
                            ReactionId = new Guid("19e0acbf-5707-49ee-8cb6-134c00b7c10b"),
                            AccountId = new Guid("f2f88bcd-d0a2-4fe7-a23f-df97a59731cd"),
                            PostId = new Guid("23d2cce9-86d7-4bff-887e-f7712b16766d"),
                            ReactionTypeId = 3
                        });
                });

            modelBuilder.Entity("ReactionService.Entities.ReactionType", b =>
                {
                    b.Property<int>("ReactionTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ReactionTypeId");

                    b.ToTable("ReactionTypes");

                    b.HasData(
                        new
                        {
                            ReactionTypeId = 1,
                            TypeName = "Like"
                        },
                        new
                        {
                            ReactionTypeId = 2,
                            TypeName = "Heart"
                        },
                        new
                        {
                            ReactionTypeId = 3,
                            TypeName = "Smiley"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
