using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projet.Models;
using Projet.Models.Repositories;
using static Projet.Models.Repositories.ICategoryRepostiory;

namespace Projet.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

		

		// GET: Category
		public IActionResult Index()
        {
            var categories = categoryRepository.GetAll();
            return View(categories);
        }

        // GET: Category/Details/5
        public IActionResult Details(int id)
        {
            var category = categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            try
            {
                categoryRepository.Add(category);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(category);
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var category = categoryRepository.GetById(id);
           
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            try
            {
                categoryRepository.Edit(category);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(category);
            }
            
        }

        // GET: Category/Delete/5
        public IActionResult Delete(int id)
        {
            var category = categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = categoryRepository.GetById(id);
            categoryRepository.Delete(category);
            return RedirectToAction(nameof(Index));
        }
    }
}
