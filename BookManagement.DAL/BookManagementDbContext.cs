﻿using System;
using System.Collections.Generic;
using BookManagement.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookManagement.DAL;

public partial class BookManagementDbContext : DbContext
{
    public BookManagementDbContext()
    {
    }

    public BookManagementDbContext(DbContextOptions<BookManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookCategory> BookCategories { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];

        return strConn;
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlServer(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Book__3DE0C207724E3FC9");

            entity.ToTable("Book");

            entity.Property(e => e.BookId).ValueGeneratedNever();
            entity.Property(e => e.Author).HasMaxLength(50);
            entity.Property(e => e.BookName).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.PublicationDate).HasColumnType("datetime");

            entity.HasOne(d => d.BookCategory).WithMany(p => p.Books)
                .HasForeignKey(d => d.BookCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Book_BookCategory");
        });

        modelBuilder.Entity<BookCategory>(entity =>
        {
            entity.HasKey(e => e.BookCategoryId).HasName("PK__BookCate__6347EC049A8F30CF");

            entity.ToTable("BookCategory");

            entity.Property(e => e.BookCategoryId).ValueGeneratedNever();
            entity.Property(e => e.BookGenreType).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__UserAcco__0CF04B1891AD20D1");

            entity.ToTable("UserAccount");

            entity.Property(e => e.MemberId).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
