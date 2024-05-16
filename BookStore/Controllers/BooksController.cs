using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.StaticFiles;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookStoreContext _context;
        private readonly IWebHostEnvironment _environment;

        public BooksController(BookStoreContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult ViewFile(string fileName)
        {
            var filePath = Path.Combine(_environment.WebRootPath, "uploads", fileName);
            if (System.IO.File.Exists(filePath))
            {
                var provider = new FileExtensionContentTypeProvider();
                if (provider.TryGetContentType(fileName, out var contentType))
                {
                    return PhysicalFile(filePath, contentType);
                }
                else
                {
                    return PhysicalFile(filePath, "application/octet-stream"); // Default to octet-stream if MIME type cannot be determined
                }
            }
            else
            {
                return NotFound(); // Return 404 if the file does not exist
            }
        }

        // GET: Books
        public async Task<IActionResult> Index(string bookGenre, string searchString, string authorsearchString)
        {
            // Query the distinct genre names
            var genreQuery = _context.Genres
                .OrderBy(g => g.GenreName)
                .Select(g => g.GenreName)
                .Distinct();
            
            // Query the books
            var booksQuery = _context.Books
                .Include(b => b.Reviews)
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .Include(b => b.Author)
                .AsQueryable();

            var authorsQuery = _context.Author.AsQueryable();
            // Apply genre filter if provided
            if (!string.IsNullOrEmpty(bookGenre))
            {
                booksQuery = booksQuery.Where(b => b.BookGenres.Any(bg => bg.Genre.GenreName == bookGenre));
            }

            // Apply search string filter if provided
            if (!string.IsNullOrEmpty(searchString))
            {
                booksQuery = booksQuery.Where(b => b.Title.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(authorsearchString)) // Fixed to check autsrch instead of searchString
            {
                // Filter by full name
                booksQuery = booksQuery.Where(a => (a.Author.FirstName + " " + a.Author.LastName).Contains(authorsearchString));
            }

            // Execute the queries asynchronously
            var genres = await genreQuery.ToListAsync();
            var books = await booksQuery.ToListAsync();
            var authors = await authorsQuery.ToListAsync();
            // Construct the view model
            var viewModel = new BookGenreViewModel
            {
                Books = books,
                Genres = new SelectList(genres),
                BookGenre = bookGenre,
                SearchString = searchString,
                Authors = authors,
                AuthorSearchString = authorsearchString
            };

            return View(viewModel);
        }


        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books.Include(b => b.Author).Include(b => b.Reviews)
                .Include(b => b.BookGenres)
                .ThenInclude(b => b.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }
        //tuka treba
        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FullName");
            var genres = _context.Genres.AsEnumerable();

            BookGenresCreateViewModel viewmodel = new BookGenresCreateViewModel
            {
                GenreListCreate = new MultiSelectList(genres, "Id", "GenreName")
            };
            return View(viewmodel);
        }

        //nema cepnato
        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //i tuka treba
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookGenresCreateViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                if (viewmodel.FrontPageFile != null && viewmodel.FrontPageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewmodel.FrontPageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewmodel.FrontPageFile.CopyToAsync(fileStream);
                    }

                    // Save file path in the database
                    viewmodel.Book.FrontPage = "/uploads/" + uniqueFileName;
                    //book.FrontPage = filePath;
                }
                if (viewmodel.PdfFile != null && viewmodel.PdfFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                    string uniquePdfFileName = Guid.NewGuid().ToString() + "_" + viewmodel.PdfFile.FileName;
                    string pdfFilePath = Path.Combine(uploadsFolder, uniquePdfFileName);

                    using (var pdfFileStream = new FileStream(pdfFilePath, FileMode.Create))
                    {
                        await viewmodel.PdfFile.CopyToAsync(pdfFileStream);
                    }

                    // Save PDF file path in the database
                    viewmodel.Book.DownloadUrl = "/uploads/" + uniquePdfFileName;
                    //book.DownloadURL = pdfFilePath;
                }

                // Save your model to the database
                _context.Add(viewmodel.Book);
                await _context.SaveChangesAsync();
                foreach (int item in viewmodel.SelectedGenresCreate)
                {
                    _context.BookGenre.Add(new BookGenre { GenreId = item, BookId = viewmodel.Book.Id });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewmodel.Book);
        }
        //tuka cepnavme
        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books =  _context.Books.Where(m => m.Id == id).Include(m => m.BookGenres).First();
            if (books == null)
            {
                return NotFound();
            }
            var genres = _context.Genres.AsEnumerable();
            genres = genres.OrderBy(s => s.GenreName);

            BookGenresEditViewModel viewModel = new BookGenresEditViewModel
            {
                Book = books,
                GenreListEdit = new MultiSelectList(genres, "Id", "GenreName"),
                SelectedGenresEdit = books.BookGenres.Select(sa => sa.GenreId)
            };

            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FullName", books.AuthorId);
            return View(viewModel);
        }
        //i tuka cepnavme
        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,BookGenresEditViewModel viewModel)
        {
            if (id != viewModel.Book.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }

                    // Return the view with the viewModel so the user can correct the errors
                    ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FullName", viewModel.Book.AuthorId);
                    return View(viewModel);
                }
                try
                {
                    //od tuka 
                    if (viewModel.FrontPageFile != null && viewModel.FrontPageFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.FrontPageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await viewModel.FrontPageFile.CopyToAsync(fileStream);
                        }

                        // Save file path in the database
                        viewModel.Book.FrontPage = "/uploads/" + uniqueFileName;
                        //book.FrontPage = filePath;
                    }
                    if (viewModel.PdfFile != null && viewModel.PdfFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                        string uniquePdfFileName = Guid.NewGuid().ToString() + "_" + viewModel.PdfFile.FileName;
                        string pdfFilePath = Path.Combine(uploadsFolder, uniquePdfFileName);

                        using (var pdfFileStream = new FileStream(pdfFilePath, FileMode.Create))
                        {
                            await viewModel.PdfFile.CopyToAsync(pdfFileStream);
                        }

                        // Save PDF file path in the database
                        viewModel.Book.DownloadUrl = "/uploads/" + uniquePdfFileName;
                        //book.DownloadUrl = pdfFilePath;
                    }
                    //do tuka
                    _context.Update(viewModel.Book);
                    await _context.SaveChangesAsync();
                    IEnumerable<int> newGenreList = viewModel.SelectedGenresEdit;
                    IEnumerable<int> prevGenreList = _context.BookGenre.Where(s => s.BookId == id).Select(s => s.GenreId);
                    IQueryable<BookGenre> toBeRemoved = _context.BookGenre.Where(s => s.BookId == id);
                    if (newGenreList != null)
                    {
                        toBeRemoved = toBeRemoved.Where(s => !newGenreList.Contains(s.GenreId));
                        foreach (int actorId in newGenreList)
                        {
                            if (!prevGenreList.Any(s => s == actorId))
                            {
                                _context.BookGenre.Add(new BookGenre { GenreId = actorId, BookId = id });
                            }
                        }
                    }
                    _context.BookGenre.RemoveRange(toBeRemoved);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BooksExists(viewModel.Book.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FullName", viewModel.Book.AuthorId);
            return View(viewModel.Book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Reviews)
                .Include(b => b.BookGenres)
                .ThenInclude(b => b.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BookStoreContext.Books'  is null.");
            }
            var books = await _context.Books.FindAsync(id);
            if (books != null)
            {
                _context.Books.Remove(books);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BooksExists(int id)
        {
          return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
