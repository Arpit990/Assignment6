using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ViewModel;
using VS2247A5.Controllers;

namespace Controllers
{
    public class EpisodeController : Controller
    {
        private readonly Manager m = new Manager();

        // GET: Episode
        public ActionResult Index()
        {
            return View(m.EpisodesGetAll());
        }

        // GET: Show/Add
        [Authorize(Roles = "Clerk")]
        [Route("Show/{id}/AddEpisode")]
        public ActionResult Create(int id)
        {
            var show = m.ShowGetById(id);
            if (show == null) return HttpNotFound();

            var form = new EpisodeAddFormViewModel
            {
                ShowId = id,
                ShowName = show.Name,
                AirDate = DateTime.Now,
                Genres = m.GenresGetAll().Select(g => new SelectListItem
                {
                    Value = g.Name,
                    Text = g.Name
                })
            };

            return View(form);
        }

        // POST: Show/Add
        [HttpPost]
        [Authorize(Roles = "Clerk")]
        [ValidateAntiForgeryToken]
        [Route("Show/{id}/AddEpisode")]
        public ActionResult Create(int id, EpisodeAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var show = m.ShowGetById(id);
                var form = new EpisodeAddFormViewModel
                {
                    ShowId = id,
                    Name = model.Name,
                    AirDate = model.AirDate,
                    Genre = model.Genre,
                    ImageUrl = model.ImageUrl,
                    Genres = m.GenresGetAll().Select(g => new SelectListItem
                    {
                        Value = g.Name,
                        Text = g.Name
                    }),
                    ShowName = show.Name
                };
                return View(form);
            }

            var addedShowId = m.EpisodeAdd(model, User.Identity.Name);
            if (addedShowId == null) return View(model);

            return RedirectToAction("Details", "Episode", new { id = addedShowId });
        }

        /*
        // GET: Episode/Create
        [Authorize(Roles = "Clerk")]
        public ActionResult Create()
        {
            var shows = m.ShowsGetAll().ToList();
            if (!shows.Any())
            {
                TempData["Error"] = "No shows available. Please add shows before creating episodes.";
                return RedirectToAction("Index");
            }

            var form = new EpisodeAddViewModel
            {
                AirDate = DateTime.Now,
                GenreList = new SelectList(m.GenresGetAll(), "Name", "Name"),
                ShowList = new SelectList(shows, "Id", "Name")
            };
            return View(form);
        }

        // POST: Episode/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Clerk")]
        public ActionResult Create(EpisodeAddViewModel episode)
        {
            if (!ModelState.IsValid)
            {
                episode.GenreList = new SelectList(m.GenresGetAll(), "Name", "Name");
                episode.ShowList = new SelectList(m.ShowsGetAll(), "Id", "Name");
                return View(episode);
            }

            try
            {
                var episodeId = m.EpisodeAdd(episode, User.Identity.Name);
                return RedirectToAction("Details", new { id = episodeId });
            }
            catch
            {
                episode.GenreList = new SelectList(m.GenresGetAll(), "Name", "Name");
                episode.ShowList = new SelectList(m.ShowsGetAll(), "Id", "Name");
                return View(episode);
            }
        }
        */

        // GET: Episode/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var episode = m.EpisodeGetById(id.Value);
            if (episode == null)
                return HttpNotFound();

            return View(episode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}