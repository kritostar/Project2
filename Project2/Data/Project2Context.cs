using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Project2.Models;

namespace Project2.Data
{
    public class Project2Context : DbContext
    {
        public Project2Context (DbContextOptions<Project2Context> options)
        : base(options)
        {
        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Band> Bands { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<AlbumGenre> AlbumGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AlbumGenre>()
                .HasKey(bc => new { bc.AlbumID, bc.GenreID });

            modelBuilder.Entity<AlbumGenre>()
                .HasOne(bc => bc.Album)
                .WithMany(bc => bc.AlbumGenres)
                .HasForeignKey(bc => bc.AlbumID);

            modelBuilder.Entity<AlbumGenre>()
                .HasOne(bc => bc.Genre)
                .WithMany(bc => bc.GenreAlbums)
                .HasForeignKey(bc => bc.GenreID);

            base.OnModelCreating(modelBuilder);
        }

    }
}
