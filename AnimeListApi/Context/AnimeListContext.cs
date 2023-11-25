﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AnimeListApi.Models.Data;

public partial class AnimeListContext : DbContext
{
    public AnimeListContext(DbContextOptions<AnimeListContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Anime> Anime { get; set; }

    public virtual DbSet<Animelist> Animelist { get; set; }

    public virtual DbSet<Characters> Characters { get; set; }

    public virtual DbSet<Favoritecharacters> Favoritecharacters { get; set; }

    public virtual DbSet<Manga> Manga { get; set; }

    public virtual DbSet<Mangalist> Mangalist { get; set; }

    public virtual DbSet<Profiles> Profiles { get; set; }

    public virtual DbSet<Status> Status { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn" })
            .HasPostgresEnum("net", "request_status", new[] { "PENDING", "SUCCESS", "ERROR" })
            .HasPostgresEnum("pgsodium", "key_status", new[] { "default", "valid", "invalid", "expired" })
            .HasPostgresEnum("pgsodium", "key_type", new[] { "aead-ietf", "aead-det", "hmacsha512", "hmacsha256", "auth", "shorthash", "generichash", "kdf", "secretbox", "secretstream", "stream_xchacha20" })
            .HasPostgresExtension("extensions", "pg_net")
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "pgjwt")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("pgsodium", "pgsodium")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Anime>(entity =>
        {
            entity.HasKey(e => e.Animeid).HasName("anime_pkey");

            entity.ToTable("anime");

            entity.Property(e => e.Animeid).HasColumnName("animeid");
            entity.Property(e => e.Episodecount).HasColumnName("episodecount");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Releaseyear).HasColumnName("releaseyear");
            entity.Property(e => e.Season)
                .HasMaxLength(20)
                .HasColumnName("season");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Animelist>(entity =>
        {
            entity.HasKey(e => e.Listid).HasName("animelist_pkey");

            entity.ToTable("animelist");

            entity.Property(e => e.Listid).HasColumnName("listid");
            entity.Property(e => e.Animeid).HasColumnName("animeid");
            entity.Property(e => e.Created)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created");
            entity.Property(e => e.Lastupdated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdated");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Watchedepisodes).HasColumnName("watchedepisodes");

            entity.HasOne(d => d.Anime).WithMany(p => p.Animelist)
                .HasForeignKey(d => d.Animeid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("animelist_animeid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Animelist)
                .HasForeignKey(d => d.Statusid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("animelist_statusid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Animelist)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("animelist_userid_fkey");
        });

        modelBuilder.Entity<Characters>(entity =>
        {
            entity.HasKey(e => e.Characterid).HasName("characters_pkey");

            entity.ToTable("characters");

            entity.Property(e => e.Characterid).HasColumnName("characterid");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Favoritecharacters>(entity =>
        {
            entity.HasKey(e => e.Favid).HasName("favoritecharacters_pkey");

            entity.ToTable("favoritecharacters");

            entity.Property(e => e.Favid).HasColumnName("favid");
            entity.Property(e => e.Characterid).HasColumnName("characterid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Character).WithMany(p => p.Favoritecharacters)
                .HasForeignKey(d => d.Characterid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("favoritecharacters_characterid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Favoritecharacters)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("favoritecharacters_userid_fkey");
        });

        modelBuilder.Entity<Manga>(entity =>
        {
            entity.HasKey(e => e.Mangaid).HasName("manga_pkey");

            entity.ToTable("manga");

            entity.Property(e => e.Mangaid).HasColumnName("mangaid");
            entity.Property(e => e.Chaptercount).HasColumnName("chaptercount");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Releaseyear).HasColumnName("releaseyear");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Mangalist>(entity =>
        {
            entity.HasKey(e => e.Listid).HasName("mangalist_pkey");

            entity.ToTable("mangalist");

            entity.Property(e => e.Listid).HasColumnName("listid");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created");
            entity.Property(e => e.Lastupdated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdated");
            entity.Property(e => e.Mangaid).HasColumnName("mangaid");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Readchapters).HasColumnName("readchapters");
            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Manga).WithMany(p => p.Mangalist)
                .HasForeignKey(d => d.Mangaid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("mangalist_mangaid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Mangalist)
                .HasForeignKey(d => d.Statusid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("mangalist_statusid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Mangalist)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("mangalist_userid_fkey");
        });

        modelBuilder.Entity<Profiles>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("profiles_pkey");

            entity.ToTable("profiles");

            entity.HasIndex(e => e.Username, "profiles_username_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AvatarUrl).HasColumnName("avatar_url");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Username).HasColumnName("username");
            entity.Property(e => e.Bio).HasColumnName("bio");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Statusid).HasName("status_pkey");

            entity.ToTable("status");

            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Statusname)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("statusname");
        });
        modelBuilder.HasSequence<int>("seq_schema_version", "graphql").IsCyclic();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}