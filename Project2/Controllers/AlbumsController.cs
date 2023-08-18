using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Project2.Data;
using Project2.Models;
using Project2.ViewModels;

namespace Project2.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly Project2Context _context;

        public AlbumsController(Project2Context context)
        {
            _context = context;
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            var project2Context = _context.Albums.Include(a => a.Band);
            return View(await project2Context.ToListAsync());
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Band)
                .Include(a => a.AlbumGenres)
                .ThenInclude(ag => ag.Genre)
                .FirstOrDefaultAsync(m => m.AlbumID == id);


            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Albums/Create
        public IActionResult Create()
        {
            ViewData["BandID"] = new SelectList(_context.Bands, "BandID", "BandName");
            ViewData["GenreID"] = new MultiSelectList(_context.Genres, "GenreID", "GenreName");
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("AlbumID,AlbumTitle,ReleaseDate,BandId,ListOfAlbumGenre")] Album album)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(album);
                    await _context.SaveChangesAsync();

                // Save associated genres to AlbumGenres table
                if (album.ListOfAlbumGenre != null)
                {
                    List<AlbumGenre> albumGenres = new List<AlbumGenre>();
                    foreach (int genreId in album.ListOfAlbumGenre)
                    {
                        albumGenres.Add(new AlbumGenre
                        {
                            AlbumID = album.AlbumID,
                            GenreID = genreId
                        });
                    }
                    _context.AlbumGenres.AddRange(albumGenres);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
                }
                ViewData["BandID"] = new SelectList(_context.Bands, "BandID", "BandID", album.BandId);
                ViewData["GenreID"] = new MultiSelectList(_context.Genres, "GenreID", "GenreID", album.AlbumGenres);
                return View(album);
            }

            // GET: Albums/Edit/5
            public async Task<IActionResult> Edit(int? id)
        {

            AlbumEditVM vm = new AlbumEditVM();

            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            //var album = await _context.Albums.FindAsync(id);

            var album = await _context.Albums
                .Include(b => b.AlbumGenres)
                .FirstOrDefaultAsync(b => b.AlbumID == id);


            if (album == null)
            {
                return NotFound();
            }

            vm.AlbumID = album.AlbumID;
            vm.AlbumTitle = album.AlbumTitle;
            vm.ReleaseDate = album.ReleaseDate;

            vm.BandID = album.BandId;
            vm.BandsSelectList = new SelectList(_context.Bands, "BandID", "BandName", album.BandId);

            if (album.AlbumGenres != null)
            {
                vm.AlbumGenreIDs = album.AlbumGenres.Select(b => b.GenreID).ToArray();
            }

            vm.GenresSelectList = new MultiSelectList(_context.Genres, "GenreID", "GenreName", vm.AlbumGenreIDs);


            return View(vm);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AlbumEditVM vm)
        {
            if (id != vm.AlbumID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Album album = new Album()
                {
                    AlbumID = vm.AlbumID,
                    AlbumTitle = vm.AlbumTitle,
                    ReleaseDate = vm.ReleaseDate,
                    BandId = vm.BandID
                };
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();

                    _context.AlbumGenres.RemoveRange(_context.AlbumGenres.Where(bc => bc.AlbumID == vm.AlbumID).ToList());

                    if (vm.AlbumGenreIDs != null)
                    {
                        List<AlbumGenre> albumCategories = new List<AlbumGenre>();

                        foreach (int bcid in vm.AlbumGenreIDs)
                        {
                            albumCategories.Add(new AlbumGenre()
                            {
                                AlbumID = vm.AlbumID,
                                GenreID = bcid,
                            });
                        }
                        _context.AlbumGenres.AddRange(albumCategories);
                        _context.SaveChanges();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.AlbumID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BandId"] = new SelectList(_context.Bands, "BandID", "BandID", vm.BandID);
            return View(vm);
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Band)
                .FirstOrDefaultAsync(m => m.AlbumID == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Albums == null)
            {
                return Problem("Entity set 'Project2Context.Albums'  is null.");
            }
            var album = await _context.Albums.FindAsync(id);
            if (album != null)
            {
                _context.Albums.Remove(album);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
          return (_context.Albums?.Any(e => e.AlbumID == id)).GetValueOrDefault();
        }
    }
}
