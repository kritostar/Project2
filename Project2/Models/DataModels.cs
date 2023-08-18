using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project2.Models
{
    public class Band
    {

        public int BandID { get; set; }
        public string BandName { get; set; } = string.Empty;
        public DateTime? DateFormed { get; set; }
        public List<Album>? ListOfAlbums { get; set; }

    }
    public class Album
    {
        public int AlbumID { get; set; }
        public string AlbumTitle { get; set; } = string.Empty;
        public DateTime? ReleaseDate { get; set; }

        public int BandId { get; set; }// foreign key
        public Band? Band { get; set; }// navigation
        public List<AlbumGenre>? AlbumGenres { get; set; } // navigation

        [NotMapped] // This attribute indicates that this property is not mapped to the database
        [BindProperty]
        public List<int>? ListOfAlbumGenre { get; set; } // Holds the selected genre IDs
    }

    public class Genre
    {
        public int GenreID { get; set; }
        public string GenreName { get; set; } = string.Empty;

       public List<AlbumGenre>? GenreAlbums { get; set; } // navigation

    }

    public class AlbumGenre
    {
        public int AlbumID { get; set; }
        public Album? Album { get; set; }

        public int GenreID { get; set; }
        public Genre? Genre { get; set; }
    }

}
