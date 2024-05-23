using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Projet.Models;
using Projet.Models.Repositories;
using Projet.ViewModels;
using System;
using System.IO;
using static Projet.Models.Repositories.ICategoryRepostiory;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Projet.Controllers
{
    [Authorize]
    public class ProductController : Controller
	{
		private readonly IProductRepository ProductRepository;
		private readonly ICategoryRepository CategoryRepository;
		private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ProductContext _context;
        public ProductController(ProductContext context,IProductRepository productRepository, ICategoryRepository categoryRepository, IWebHostEnvironment hostingEnvironment)
		{
			ProductRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
			CategoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
			_hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _context = context;
        }


        [AllowAnonymous]
        // GET: ProductController
        public async Task<IActionResult> IndexAsync(int? categoryId, string searchString, string sortOrder)
        {
            var products = ProductRepository.GetAll();

            if (categoryId.HasValue && categoryId > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId).ToList();
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "priceAsc":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case "priceDesc":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
            }
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = new SelectList (categories,"CategoryId", "CategoryName");

            return View(products);
        }


        // GET: ProductController/Details/5
        public ActionResult Details(int id)
		{
			var product = ProductRepository.GetById(id);
			if (product == null)
			{
				return NotFound();
			}

			var category = CategoryRepository.GetById(product.CategoryId);
			ViewBag.CategoryName = category?.CategoryName;
			return View(product);
		}

		// GET: ProductController/Create
		public ActionResult Create()
		{
			ViewBag.CategoryId = new SelectList(CategoryRepository.GetAll(), "CategoryId", "CategoryName");
			return View();
		}

		/// POST: ProductController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreateViewModel model)
		{
			if (ModelState.IsValid)
			{
				string uniqueFileName = ProcessUploadedFile(model);

				Product newProduct = new Product
				{
					Name = model.Name,
					Description = model.Description,
					Price = model.Price,
					StockQuantity = model.StockQuantity,
					Image = uniqueFileName,
					CategoryId = model.CategoryId 
				};

				if (CategoryRepository.GetById(newProduct.CategoryId) == null)
				{
					ModelState.AddModelError("CategoryId", "Invalid category.");
					ViewBag.CategoryId = new SelectList(CategoryRepository.GetAll(), "CategoryId", "CategoryName");
					return View(model);
				}

				ProductRepository.Add(newProduct);
				return RedirectToAction("Details", new { id = newProduct.ProductId });
			}

			ViewBag.CategoryId = new SelectList(CategoryRepository.GetAll(), "CategoryId", "CategoryName");
			return View(model);
		}

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            Product product = ProductRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            // Populate the ViewBag with the list of categories
            ViewBag.CategoryId = new SelectList(CategoryRepository.GetAll(), "CategoryId", "CategoryName");

            EditViewModel productEditViewModel = new EditViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = (float)product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                ExistingImagePath = product.Image,
            };

            return View(productEditViewModel);
        }


        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = ProductRepository.GetById(model.ProductId);
                if (product == null)
                {
                    return NotFound();
                }

                // Update product properties
                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.StockQuantity = model.StockQuantity;
                product.CategoryId = model.CategoryId;

                // Check if a new image was uploaded
                if (model.ImagePath != null)
                {
                    // Process the uploaded image and update the product's image path
                    product.Image = ProcessUploadedFile(model);
                }

                // Call repository method to update the product
                Product updatedProduct = ProductRepository.Update(product);

                if (updatedProduct != null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }

            // If ModelState is not valid, return the view with the model to display validation errors
            ViewBag.CategoryId = new SelectList(CategoryRepository.GetAll(), "CategoryId", "CategoryName");
            return View(model);
        }



        [NonAction]
		private string ProcessUploadedFile(dynamic model)
		{
			string uniqueFileName = null;
			if (model.ImagePath != null)
			{
				string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
				uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
				string filePath = Path.Combine(uploadsFolder, uniqueFileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					model.ImagePath.CopyTo(fileStream);
				}
			}
			return uniqueFileName;
		}

		// GET: ProductController/Delete/5
		public ActionResult Delete(int id)
		{
			var product = ProductRepository.GetById(id);
			if (product == null)
			{
				return NotFound();
			}
			return View(product);
		}

		// POST: ProductController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(Product product)
		{
			try
			{
				ProductRepository.Delete(product);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View(product);
			}
		}

		public ActionResult Search(string name, int? categoryId)
		{
			var result = ProductRepository.GetAll();
			if (!string.IsNullOrEmpty(name))
			{
				result = ProductRepository.FindByName(name);
			}
			else if (categoryId != null)
			{
				result = ProductRepository.GetProductByCategoryId((int)categoryId);
			}

			ViewBag.CategoryID = new SelectList(CategoryRepository.GetAll(), "CategoryID", "CategoryName");
			return View("Index", result);
		}


	}
}
