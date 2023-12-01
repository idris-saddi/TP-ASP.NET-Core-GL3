using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP1.Models;

namespace TP1.Controllers;

public class MovieController : Controller
{
	private readonly AppDbContext _context;
	private readonly long _fileSizeLimit;
	private readonly string _uploadedFilesPath;
	private readonly string[] _permittedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

	public MovieController(AppDbContext context, IConfiguration config)
	{
		_context = context;
		_fileSizeLimit = config.GetValue<long>("FileSizeLimit");
		_uploadedFilesPath = config.GetValue<string>("UploadedFilesPath")!;
	}

	public IActionResult Index()
	{
		var movies = _context
			.Movies
			.Include(x => x.Genre)
			.ToList();

		return View(movies);
	}

	public IActionResult Create()
	{
		var movie = new Movie();
		ViewBag.GenreId = GenreSelectListItems(movie);
		return View(movie);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Create(Movie movie)
	{
		if (!ModelState.IsValid)
		{
			ViewBag.GenreId = GenreSelectListItems(movie);
			return View(movie);
		}

		if (movie.File != null && movie.File.Length > 0)
		{
			if (movie.File.Length > _fileSizeLimit)
			{
				ModelState.AddModelError("File", "File is too big (Max = 2MB)");
				return View(movie);
			}

			var ext = Path.GetExtension(movie.File.FileName).ToLowerInvariant();

			if (string.IsNullOrEmpty(ext) || !_permittedExtensions.Contains(ext))
			{
				ModelState.AddModelError("File", $"File extension is not permitted (Permitted Extensions = {string.Join(", ", _permittedExtensions)})");
				return View(movie);
			}

			var fileName = Guid.NewGuid().ToString() + ext;
			var filePath = Path.Combine(_uploadedFilesPath, fileName);

			using var stream = System.IO.File.Create(filePath);
			movie.File.CopyTo(stream);

			movie.Poster = filePath;
		}

		_context.Movies.Add(movie);
		_context.SaveChanges();
		return RedirectToAction(nameof(Index));
	}

	public IActionResult Details(Guid id)
	{
		try
		{
			var movie = _context
				.Movies
				.Include(x => x.Genre)
				.Where(x => x.Id == id)
				.Single();

			return View(movie);
		}
		catch (Exception)
		{
			return NotFound();
		}
	}

	public IActionResult Edit(Guid id)
	{
		var movie = _context.Movies.Find(id);

		if (movie == null)
		{
			return NotFound();
		}

		ViewBag.GenreId = GenreSelectListItems(movie);
		return View(movie);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Edit(Movie movie)
	{
		if (!ModelState.IsValid)
		{
			ViewBag.GenreId = GenreSelectListItems(movie);
			return View(movie);
		}

		if (movie.File != null && movie.File.Length > 0)
		{
			if (movie.File.Length > _fileSizeLimit)
			{
				ModelState.AddModelError("File", "File is too big (Max = 2MB)");
				return View(movie);
			}

			var ext = Path.GetExtension(movie.File.FileName).ToLowerInvariant();

			if (string.IsNullOrEmpty(ext) || !_permittedExtensions.Contains(ext))
			{
				ModelState.AddModelError("File", $"File extension is not permitted (Permitted Extensions = {string.Join(", ", _permittedExtensions)})");
				return View(movie);
			}

			var fileName = Guid.NewGuid().ToString() + ext;
			var filePath = Path.Combine(_uploadedFilesPath, fileName);

			using var stream = System.IO.File.Create(filePath);
			movie.File.CopyTo(stream);

			if (!string.IsNullOrEmpty(movie.Poster))
			{
				System.IO.File.Delete(movie.Poster);
			}

			movie.Poster = filePath;
		}

		_context.Movies.Update(movie);
		_context.SaveChanges();
		return RedirectToAction(nameof(Index));
	}

	public IActionResult Delete(Guid id)
	{
		try
		{
			var movie = _context
				.Movies
				.Include(x => x.Genre)
				.Where(x => x.Id == id)
				.Single();

			return View(movie);
		}
		catch (Exception)
		{
			return NotFound();
		}
	}

	[HttpPost]
	public IActionResult Delete(Movie movie)
	{
		_context.Movies.Remove(movie);
		_context.SaveChanges();
		return RedirectToAction(nameof(Index));
	}

	private IEnumerable<SelectListItem> GenreSelectListItems(Movie movie)
	{
		var selectedGenreId = movie.GenreId;

		return _context
			.Genres
			.Select(genre => new SelectListItem()
			{
				Text = genre.Name,
				Value = genre.Id.ToString(),
				Selected = (selectedGenreId != null) && (genre.Id == selectedGenreId),
			})
			.ToList()
			.Prepend(new()
			{
				Text = "Please select a genre",
				Value = "",
				Selected = selectedGenreId == null,
			});
	}
}

