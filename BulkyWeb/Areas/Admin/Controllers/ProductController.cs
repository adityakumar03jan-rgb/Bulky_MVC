using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {       
       
        //Dependency Injection
        private readonly IUnitOfWork _unitOfWork;
        //Constructor
        public ProductController(IUnitOfWork unitOfWork)
        {
            //Constructor Injection
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //Fetch data from database and pass to view
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            return View(objProductList);
        }
        public IActionResult Create()
        {
            //Projecting the Category data to SelectListItem for dropdown in view
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            ViewBag.CategoryList = CategoryList;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Product obj)
        {
            //Server side validation
            //Custom validation
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name.");
            //}
            if (ModelState.IsValid) {
            _unitOfWork.Product.Add(obj);
            _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
           if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productyFromDb = _unitOfWork.Product.Get(c => c.Id == id);
            //Product? productyFromDb1 = _db.Categories.FirstOrDefault(c => c.Id == id);
            //Product? productyFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (productyFromDb == null)
            {
                return NotFound();
            }
            return View(productyFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
           
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productyFromDb = _unitOfWork.Product.Get(c => c.Id == id);

            if (productyFromDb == null)
            {
                return NotFound();
            }
            return View(productyFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletPost(int? id)
        {
            Product? obj =     _unitOfWork.Product.Get(c => c.Id == id);
            if (obj == null) { 
                return NotFound();
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
           
        }
    }

}
