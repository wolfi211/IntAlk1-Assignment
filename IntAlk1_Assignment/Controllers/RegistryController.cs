using IntAlk1_Assignment.Models;
using IntAlk1_Assignment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagedList;

namespace IntAlk1_Assignment.Controllers
{
    public class RegistryController : Controller
    {
        readonly RegistryDAO registryDAO = new();
        public IActionResult Index()
        {
            return RedirectToAction("PropertyList");
        }

        /*********************************************************************
         * Listings
         *********************************************************************/

        #region LIST

        public IActionResult PropertyList(int? page, string? searchTerm, string? currentFilter)
        {
            int pageSize = 12;

            if (searchTerm != null)
            {
                page = 1;
            }
            else
            {
                searchTerm = currentFilter;
            }

            ViewBag.CurrentFilter = searchTerm;

            List<PropertyModel> propertyModels = registryDAO.GetProperties();

            if (!String.IsNullOrEmpty(searchTerm))
            {
                propertyModels = registryDAO.SearchProperties(searchTerm);
            }

            if (TempData["Success"] != null)
            {
                ViewBag.Success = Convert.ToString(TempData["Success"]);
                TempData.Remove("Success");
            }
            if (TempData["Failed"] != null)
            {
                ViewBag.Failed = Convert.ToString(TempData["Failed"]);
                TempData.Remove("Failed");
            }

            return View(propertyModels.ToPagedList(page ?? 1, pageSize));
        }

        public IActionResult OwnerList(int? page, string? searchTerm, string? currentFilter)
        {
            int pageSize = 12;

            if (searchTerm != null)
            {
                page = 1;
            }
            else
            {
                searchTerm = currentFilter;
            }

            ViewBag.CurrentFilter = searchTerm;

            List<OwnerModel> ownerModels = registryDAO.GetOwners();

            if (!String.IsNullOrEmpty(searchTerm))
            {
                ownerModels = registryDAO.SearchOwners(searchTerm);
            }

            if (TempData["Success"] != null)
            {
                ViewBag.Success = Convert.ToString(TempData["Success"]);
                TempData.Remove("Success");
            }
            if (TempData["Failed"] != null)
            {
                ViewBag.Failed = Convert.ToString(TempData["Failed"]);
                TempData.Remove("Failed");
            }

            return View(ownerModels.ToPagedList(page ?? 1, pageSize));
        }

        public IActionResult TenantList(int? page, string? searchTerm, string? currentFilter)
        {
            int pageSize = 12;

            if (searchTerm != null)
            {
                page = 1;
            }
            else
            {
                searchTerm = currentFilter;
            }

            ViewBag.CurrentFilter = searchTerm;

            List<TenantModel> tenantModels = registryDAO.GetTenants();

            if (!String.IsNullOrEmpty(searchTerm))
            {
                tenantModels = registryDAO.SearchTenants(searchTerm);
            }

            if (TempData["Success"] != null)
            {
                ViewBag.Success = Convert.ToString(TempData["Success"]);
                TempData.Remove("Success");
            }
            if (TempData["Failed"] != null)
            {
                ViewBag.Failed = Convert.ToString(TempData["Failed"]);
                TempData.Remove("Failed");
            }

            return View(tenantModels.ToPagedList(page ?? 1, pageSize));
        }

        #endregion

        /*********************************************************************
         * Create Forms and Ongoing Creation Processes
         *********************************************************************/

        #region CREATE

        //OWNER

        public IActionResult CreateOwnerForm()
        {
            return View();
        }

        public IActionResult ProcessOwnerCreation(OwnerModel owner)
        {
            if(registryDAO.InsertOwner(owner) < 0)
            {
                ViewBag.Failed = "<strong>Failed!</strong> Owner creation failed for some reason!";
                return View("CreateOwnerForm");
            }
            TempData["Success"] = "<strong>Success!</strong> Owner creation successfull.";
            return RedirectToAction("OwnerList");
        }

        //TENANT

        public IActionResult CreateTenantForm()
        {
            return View();
        }

        public IActionResult ProcessTenantCreation(TenantModel tenant)
        {
            if (registryDAO.InsertTenant(tenant) < 0)
            {
                ViewBag.Failed = "<strong>Failed!</strong> Tenant creation failed for some reason!";
                return View("CreateTenantForm");
            }
            TempData["Success"] = "<strong>Success!</strong> Owner creation successfull.";
            return RedirectToAction("TenantList");
        }

        //PROPERTY

        public IActionResult CreatePropertyForm()
        {
            ViewBag.OwnerList = registryDAO.GetOwners();
            ViewBag.TenantList = registryDAO.GetTenants();

            return View();
        }

        public IActionResult ProcessPropertyCreation(PropertyModel property)
        {
            throw new NotImplementedException();
        }

        #endregion

        /*********************************************************************
         * Details Pages /// there is no Property Details by design
         *********************************************************************/

        #region DETAILS

        public IActionResult DetailsOwner(int id)
        {
            return View(registryDAO.GetOwnerById(id));
        }

        public IActionResult DetailsTenant(int id)
        {
            return View(registryDAO.GetTenantById(id));
        }

        #endregion

        /*********************************************************************
         * Delete Processes
         *********************************************************************/

        #region DELETE

        public IActionResult DeleteOwner(int id)
        {
            if (registryDAO.DeleteOwner(id) < 0)
            {
                TempData["Failed"] = "<strong>Failed!</strong> The owner's property list may not be empty.";
            }
            else
            {
                TempData["Success"] = "<strong>Success!</strong> The owner was successfully deleted.";
            }
            return RedirectToAction("OwnerList");
        }

        public IActionResult DeleteTenant(int id)
        {
            if (registryDAO.DeleteTenant(id) < 0)
            {
                TempData["Failed"] = "<strong>Failed!</strong> The tenant's property list may not be empty.";
            }
            else
            {
                TempData["Success"] = "<strong>Success!</strong> The tenant was successfully deleted.";
            }
            return RedirectToAction("TenantList");
        }

        public IActionResult DeleteProperty(int id)
        {
            if (registryDAO.DeleteProperty(id) < 0)
            {
                TempData["Failed"] = "<strong>Failed!</strong> The property may have a connected payment obligation.";
            }
            else
            {
                TempData["Success"] = "<strong>Success!</strong> The property was successfully deleted.";
            }
            return RedirectToAction("PropertyList");
        }

        #endregion

        /*********************************************************************
         * Edit Pages and Processes
         *********************************************************************/

        #region EDIT

        //OWNER

        public IActionResult EditOwner(int id)
        {
            var owner = registryDAO.GetOwnerById(id);
            return RedirectToAction("EditOwnerForm", owner);
        }

        public IActionResult EditOwnerForm(OwnerModel owner)
        {
            return View(owner);
        }

        public IActionResult EditOwnerProcess(OwnerModel owner)
        {
            if (registryDAO.UpdateOwner(owner) < 0)
            {
                TempData["Failed"] = "<strong>Failed!</strong> The owner was not updated.";
            }
            else
            {
                TempData["Success"] = "<strong>Success!</strong> The owner was successfully updated.";
            }
            return RedirectToAction("OwnerList");
        }

        //TENANT

        public IActionResult EditTenant(int id)
        {
            return RedirectToAction("EditTenantForm", registryDAO.GetTenantById(id));
        }

        public IActionResult EditTenantForm(TenantModel tenant)
        {
            return View(tenant);
        }

        public IActionResult EditTenantProcess(TenantModel tenant)
        {
            if (registryDAO.UpdateTenant(tenant) < 0)
            {
                TempData["Failed"] = "<strong>Failed!</strong> The owner was not updated.";
            }
            else
            {
                TempData["Success"] = "<strong>Success!</strong> The owner was successfully updated.";
            }
            return RedirectToAction("TenantList");
        }

        #endregion
    }
}
