using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NManga.Models;
using NManga.DataAccess;

namespace NManga.Controllers
{
    [Authorize]
    public class ComicController : Controller
    {
        private readonly IComicDac _comicDac;

        public ComicController()
        {
            _comicDac = new ComicDac(new ApplicationDbContext());
        }

        public ComicController(IComicDac comicDac)
        {
            _comicDac = comicDac;
        }

        [AllowAnonymous]
        public ActionResult Browse()
        {
            ViewBag.Comics = _comicDac.GetAllByPublishDate();

            return View();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.ComicCount = _comicDac.GetComicCount();

            var comic = _comicDac.GetLatestComic();

            if (comic == null)
            {
                throw new Exception("There are no comics in your installation of NManga.");
            }

            return View("View", comic);
        }

        [AllowAnonymous]
        public ActionResult View(int id)
        {
            ViewBag.ComicCount = _comicDac.GetComicCount();

            var comic = _comicDac.GetComicByOrdinal(id);

            if(comic == null)
            {
                throw new Exception("Comic with provided ID not found.");
            }

            return View(comic);
        }

        [AllowAnonymous]
        public ActionResult Image(int id)
        {
            var comic = _comicDac.GetComicById(id);

            if (comic == null)
            {
                throw new Exception("Comic with provided ID not found.");
            }

            return File(comic.Content, comic.ContentType);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Comic comic, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    comic.FileName = System.IO.Path.GetFileName(upload.FileName);
                    comic.ContentType = upload.ContentType;

                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        comic.Content = reader.ReadBytes(upload.ContentLength);
                    }
                }

                comic.TimePublished = DateTime.Now;
                comic.TimeUpdated = DateTime.Now;
                comic.TimeCreated = DateTime.Now;
                comic.Ordinal = _comicDac.GetComicCount() + 1;
                _comicDac.InsertComic(comic);

                return RedirectToAction("Index");
            }

            return View(comic);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var comic = _comicDac.GetComicByOrdinal(id);

            if (comic == null)
            {
                throw new Exception("Comic with provided ID not found.");
            }

            return View(comic);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Comic updatedComic, HttpPostedFileBase upload)
        {
            var comic = _comicDac.GetComicByOrdinal(updatedComic.Ordinal);

            if (comic == null)
            {
                throw new Exception("Comic with provided ID not found.");
            }

            if (ModelState.IsValid)
            {
                comic.Name = updatedComic.Name;
                comic.AltText = updatedComic.AltText;

                if (upload != null && upload.ContentLength > 0)
                {
                    comic.FileName = System.IO.Path.GetFileName(upload.FileName);
                    comic.ContentType = upload.ContentType;

                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        comic.Content = reader.ReadBytes(upload.ContentLength);
                    }
                }

                comic.TimeUpdated = DateTime.Now;
                _comicDac.UpdateComic(comic);

                return RedirectToAction("Index");
            }

            return View(comic);
        }

        public void Reindex()
        {
            _comicDac.ReindexComics();
        }
    }
}