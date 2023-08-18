using Microsoft.AspNetCore.Mvc.Rendering;

namespace Project2.ViewModels
{
    public class AlbumEditVM
    {
        public int AlbumID { get; set; }
        public string AlbumTitle { get; set; } = string.Empty;
        public DateTime? ReleaseDate { get; set; }

        public int BandID { get; set; }
        public int[]? AlbumGenreIDs { get; set; }
        public SelectList? BandsSelectList { get; set; }
        public MultiSelectList? GenresSelectList { get; set; }
    }
}
