using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {

        //Dependency Injection
        private readonly IUnitOfWork _unitOfWork;
        
        //Constructor
        public CompanyController(IUnitOfWork unitOfWork)
        {
            //Constructor Injection
            _unitOfWork = unitOfWork;
            
        }
        public IActionResult Index()
        {
            //Fetch data from database and pass to view
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(objCompanyList);
        }
        public IActionResult Upsert(int? id)
        {

            
            if (id == null || id == 0)
            {
                //Create Company
                return View(new Company());
            }
            else
            {
                //Update Company
                Company companyobj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyobj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company Companyobj)
        {

            if (ModelState.IsValid)
            {
               
                if (Companyobj.Id == 0)
                {
                    _unitOfWork.Company.Add(Companyobj);
                    TempData["success"] = "Company created successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(Companyobj);
                    TempData["success"] = "Company updated successfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
               return View(Companyobj);
            }

        }

       //region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }


    }

}
