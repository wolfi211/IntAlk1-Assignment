using IntAlk1_Assignment.Models;
using IntAlk1_Assignment.Services;
using Microsoft.AspNetCore.Mvc;
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

            List<OwnerModel> ownerModels;

            if (!String.IsNullOrEmpty(searchTerm))
            {
                ownerModels = registryDAO.SearchOwners(searchTerm);
            }
            else
            {
                ownerModels = registryDAO.GetOwners();
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

            List<TenantModel> tenantModels;

            if (!String.IsNullOrEmpty(searchTerm))
            {
                tenantModels = registryDAO.SearchTenants(searchTerm);
            }
            else
            {
                tenantModels = registryDAO.GetTenants();
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

        public IActionResult PropertyList(int? page, string? searchTerm, string? currentFilter)
        {
            int pageSize = 15;

            if (searchTerm != null)
            {
                page = 1;
            }
            else
            {
                searchTerm = currentFilter;
            }

            ViewBag.CurrentFilter = searchTerm;

            List<PropertyModel> propertyModels;

            if (!String.IsNullOrEmpty(searchTerm))
            {
                propertyModels = registryDAO.SearchProperties(searchTerm);
            }
            else
            {
                propertyModels = registryDAO.GetProperties();
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

        public IActionResult RentListByProperties(int? page)
        {
            int pageSize = 17;

            List<RentModel> rentModels = registryDAO.GetRents();

            rentModels.Sort((x, y) => string.Compare(x.PropertyAddress, y.PropertyAddress));

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

            return View(rentModels.ToPagedList(page ?? 1, pageSize));
        }

        public IActionResult RentListByTenants(int? page)
        {
            int pageSize = 15;

            List<RentModel> rentModels = registryDAO.GetRents();

            rentModels.Sort((x, y) => string.Compare(x.TenantName, y.TenantName));

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

            return View(rentModels.ToPagedList(page ?? 1, pageSize));
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
            TempData["Success"] = "<strong>Success!</strong> Tenant creation successfull.";
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
            if (property.OwnerId == 0 && property.TenantId != 0)
            {
                ViewBag.Failed = "<strong>Failed! </strong>You can't add a Property with a Tenant but without an Owner!";
                ViewBag.OwnerList = registryDAO.GetOwners();
                ViewBag.TenantList = registryDAO.GetTenants();
                return View("CreatePropertyForm", property);
            }
            if (registryDAO.InsertProperty(property) < 0)
            {
                ViewBag.Failed = "<strong>Failed!</strong> Property creation failed for some reason!";
                return View("CreatePropertyForm");
            }
            TempData["Success"] = "<strong>Success!</strong> Property creation successfull.";
            return RedirectToAction("PropertyList");
        }

        //RENT

        public IActionResult SimulateFirstOfMonth()
        {
            registryDAO.CreateObligationForRent();

            return RedirectToAction("Index", "Home");
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

        public IActionResult DeleteRent(int year, int month, int property)
        {
            if (registryDAO.DeleteRent(year, month, property) > 0)
            {
                TempData["Success"] = "<strong>Success!</strong> The rent was successfully deleted";
                
            }
            else
            {
                TempData["Failed"] = "<strong>Failed!</strong>";
            }
            return RedirectToAction("RentListByTenants");
        }

        public IActionResult DeleteAllRent()
        {
            registryDAO.DeleteAllRent();
            return RedirectToAction("Index", "Home");
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
                TempData["Failed"] = "<strong>Failed!</strong> The tenant was not updated.";
            }
            else
            {
                TempData["Success"] = "<strong>Success!</strong> The tenant was successfully updated.";
            }
            return RedirectToAction("TenantList");
        }

        //PROPERTY

        public IActionResult EditProperty(int id)
        {
            PropertyModel property = registryDAO.GetPropertyById(id);
            return RedirectToAction("EditPropertyForm", property);
        }

        public IActionResult EditPropertyForm(PropertyModel property)
        {
            ViewBag.OwnerList = registryDAO.GetOwners();
            ViewBag.TenantList = registryDAO.GetTenants();

            return View(property);
        }

        public IActionResult EditPropertyProcess(PropertyModel property)
        {
            if(property.OwnerId == 0 && property.TenantId != 0)
            {
                ViewBag.Failed = "<strong>Failed! </strong>You can't delete the Owner and have a Tenant still renting the property!";
                ViewBag.OwnerList = registryDAO.GetOwners();
                ViewBag.TenantList = registryDAO.GetTenants();
                return View("EditPropertyForm", property);
            }
            if (registryDAO.UpdateProperty(property) < 0)
            {
                TempData["Failed"] = "<strong>Failed!</strong> The property was not updated.";
            }
            else
            {
                TempData["Success"] = "<strong>Success!</strong> The property was successfully updated.";
            }
            return RedirectToAction("PropertyList");
        }

        //RENT

        public IActionResult RecordPayment(int year, int month, int property)
        {
            RentModel rent = registryDAO.GetRentById(year, month, property);

            return View("RecordPaymentForm", rent);
        }

        public IActionResult RecordPaymentProcess(int year, int month, int property, int payment)
        {
            if(registryDAO.UpdateRent(year, month, property, payment) > 0)
            {
                TempData["Success"] = "<strong>Success!</strong> The payment was succesfully added to the rent.";
            }
            else
            {
                TempData["Failed"] = "<strong>Failed!</strong> Could not update the rent for some reason. Try again?";
            }
            return RedirectToAction("RentListByTenants");
        }

        #endregion

    }
}