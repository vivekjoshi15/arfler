using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Arfler.Models;
using Newtonsoft.Json;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using CloudinaryDotNet.Actions;

namespace Arfler.Controllers
{
    public class AdminController : Controller
    {
        private readonly ArflerDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _env;
        public AdminController(UserManager<ApplicationUser> userManager, ArflerDBContext context, IHostingEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {

            var user = await GetCurrentUserAsync();
            var role = await _userManager.GetRolesAsync(user);
            TempData["userRole"] = role[0];
            List<RestaurantDetail> _restaurantlist = new List<RestaurantDetail>();
            if (role[0].ToLower() == "admin")
            {
                _restaurantlist = _context.RestaurantDetail.ToList();
            }
            else if (role[0].ToLower() == "user")
            {
                _restaurantlist = _context.RestaurantDetail.Where(a => a.userId == user.Id).ToList();
            }
            //   var dummyItems = _context.RestaurantDetail.ToList();
            var pager = new PagerRest(_restaurantlist.Count(), page);
            var viewModel = new PageRestaurantViewModel
            {
                Items = _restaurantlist.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                PagerRest = pager
            };
            return View(viewModel);
        }

        [HttpGet]
        //  GET: /Admin/RestaurantDetails/Id
        public async Task<IActionResult> RestaurantDetails(long id)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                if (id > 0)
                {
                    List<CategoryDetail> lstCat = new List<CategoryDetail>();
                    var _restDetails = _context.RestaurantDetail.FirstOrDefault(a => a.id == id);


                    var cat_list = _context.CategoryDetail.ToList();
                    foreach (var clist in cat_list)
                    {
                        if (_restDetails.categoryIds != null)
                        {
                            if (_restDetails.categoryIds.Contains(clist.CategoryId.ToString()))
                            {
                                clist.checkCategory = true;

                            }
                        }
                        lstCat.Add(clist);
                    }
                    ViewData["Categories"] = lstCat;
                    return View(_restDetails);
                }
                else
                {
                    var cat_list = _context.CategoryDetail.ToList();
                    ViewData["Categories"] = cat_list;
                    return View();
                }
            }
            else
            {
                return RedirectToAction("QuickRegister", "Account");
            }
        }

        [HttpPost]
        //Post : Admin/RestaurantDetails/
        public async Task<IActionResult> RestaurantDetails(RestaurantDetail model)
        {
            string ddBrakTimeFrom = HttpContext.Request.Form["ddBrakTimeFrom"];
            string ddBrakTimeTo = HttpContext.Request.Form["ddBreakTimingTO"];

            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                string aa = HttpContext.Request.Form["cats"];
                string filename = HttpContext.Request.Form["file"];

                string filepath = HttpContext.Request.Form["filePath"];

                if (!string.IsNullOrEmpty(filepath))
                {
                    CloudinaryDotNet.Account account = new CloudinaryDotNet.Account("hkm2gz727", "654416183426452", "AZJIv_WvBo1Z7gkzN-uXFVg2_BE");
                    Cloudinary cloudinary = new Cloudinary(account);

                    CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams();
                    uploadParams.File = new CloudinaryDotNet.Actions.FileDescription(filepath);

                    CloudinaryDotNet.Actions.ImageUploadResult uploadResult = await cloudinary.UploadAsync(uploadParams);
                    string url = cloudinary.Api.UrlImgUp.BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));
                    model.mainImageUrl = url;
                }
                else
                {
                    string imageurl = HttpContext.Request.Form["imageurl"];
                    model.mainImageUrl = imageurl;
                }
                model.timing1 = model.timing1;
                model.timing2 = model.timing2;

                if (model.id != 0)
                {
                    model.userId = model.userId;
                    model.categoryIds = aa;
                    model.modifiedDate = DateTime.Now;
                    _context.RestaurantDetail.Update(model);
                    _context.SaveChanges();
                }
                else
                {
                    model.userId = user.Id;
                    model.categoryIds = aa;
                    model.createdDate = DateTime.Now;
                    model.modifiedDate = DateTime.Now;
                    model.sortOrder = _context.RestaurantDetail.Max(a => a.sortOrder) + 1;
                    _context.RestaurantDetail.Add(model);
                    _context.SaveChanges();
                }
                var restId = _context.RestaurantDetail.Max(a => a.id);
                MenuCategoryDetail objmenucats = new MenuCategoryDetail();
                objmenucats.createdDate = DateTime.Now;
                objmenucats.isEnabled = true;
                objmenucats.parentMenuCategoryId = null;
                objmenucats.menuCategoryDesc = null;
                objmenucats.menuCategoryName = "Breakfast";
                objmenucats.modifiedDate = DateTime.Now;
                objmenucats.restaurantId = restId;
                objmenucats.sortOrder = 1;
                _context.MenuCategoryDetail.Add(objmenucats);
                _context.SaveChanges();

                MenuCategoryDetail objmenucats1 = new MenuCategoryDetail();
                objmenucats1.createdDate = DateTime.Now;
                objmenucats1.isEnabled = true;
                objmenucats.parentMenuCategoryId = null;
                objmenucats1.menuCategoryDesc = null;
                objmenucats1.menuCategoryName = "Lunch";
                objmenucats1.modifiedDate = DateTime.Now;
                objmenucats1.restaurantId = restId;
                objmenucats1.sortOrder = 2;
                _context.MenuCategoryDetail.Add(objmenucats1);
                _context.SaveChanges();

                MenuCategoryDetail objmenucats2 = new MenuCategoryDetail();
                objmenucats2.createdDate = DateTime.Now;
                objmenucats2.isEnabled = true;
                objmenucats2.menuCategoryDesc = null;
                objmenucats.parentMenuCategoryId = null;
                objmenucats2.menuCategoryName = "Dinner";
                objmenucats2.modifiedDate = DateTime.Now;
                objmenucats2.restaurantId = restId;
                objmenucats2.sortOrder = 3;
                _context.MenuCategoryDetail.Add(objmenucats2);
                _context.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("QuickRegister", "Account");
            }
        }

        public IActionResult DeleteResaturant(long id)
        {
            var rest = _context.RestaurantDetail.FirstOrDefault(a => a.id == id);
            if (rest != null)
            {
                _context.RestaurantDetail.Remove(rest);
                _context.SaveChanges();
            }
            return RedirectToAction("index", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Categories(int? page)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["resturant"]))
                {
                    string res = HttpContext.Request.Query["resturant"].ToString();
                }

                var dummyItems = _context.CategoryDetail.ToList();
                // ViewBag.Rname = dummyItems.restaurantName;
                List<CategoryDetail> _listCategories = new List<Models.CategoryDetail>();
                var pager = new Pager(dummyItems.Count(), page);
                var viewModel = new PageViewModel
                {
                    Items = dummyItems.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };

                return View(viewModel);
            }
            else
                return RedirectToAction("QuickRegister", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> CategoryDetail(long Id)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                List<CategoryDetail> CategoryList = new List<Models.CategoryDetail>();
                CategoryList = _context.CategoryDetail.ToList();
                CategoryList.Insert(0, new CategoryDetail { CategoryId = 0, CategoryName = "Select" });
                ViewBag.Categories = CategoryList;
                if (Id > 0)
                {
                    var _restDetails = _context.CategoryDetail.FirstOrDefault(a => a.CategoryId == Id);
                    return View(_restDetails);
                }
                else
                {
                    return View();
                }
            }
            else
                return RedirectToAction("QuickRegister", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> CategoryDetail(CategoryDetail model)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                if (ModelState.IsValid)
                {
                    if (model.CategoryId != 0)
                    {

                        _context.CategoryDetail.Update(model);
                        _context.SaveChanges();
                    }
                    else
                    {

                        model.SortOrder = _context.CategoryDetail.Max(a => a.SortOrder) + 1;
                        _context.CategoryDetail.Add(model);
                        _context.SaveChanges();
                    }

                }
                return RedirectToAction("Categories", "Admin");
            }
            else
                return RedirectToAction("QuickRegister", "Account");
        }

        public IActionResult DeleteCategories(long id)
        {
            var rest = _context.CategoryDetail.FirstOrDefault(a => a.CategoryId == id);
            if (rest != null)
            {
                _context.CategoryDetail.Remove(rest);
                _context.SaveChanges();
            }
            return RedirectToAction("Categories", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> MenuCategories(int? page)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["resturant"]))
                {
                    string res = HttpContext.Request.Query["resturant"].ToString();
                    var dummy = _context.RestaurantDetail.FirstOrDefault(a => a.id == Convert.ToInt32(res));
                    ViewBag.Rname = dummy.restaurantName;
                    @ViewBag.restId = res;
                    string pcats = HttpContext.Request.Query["pcat"].ToString();
                    List<MenuCategoryDetail> objMenuCats = new List<MenuCategoryDetail>();
                    if (!string.IsNullOrEmpty(pcats))
                    {
                        var _PmenuCategories = _context.MenuCategoryDetail.Where(a => a.restaurantId == Convert.ToInt32(res) && a.parentMenuCategoryId == Convert.ToInt32(pcats)).ToList();
                        foreach (var menuCats in _PmenuCategories)
                        {
                            var rname = _context.RestaurantDetail.FirstOrDefault(a => a.id == menuCats.restaurantId);
                            menuCats.RestaurantName = rname.restaurantName;
                            if (menuCats.parentMenuCategoryId != -1)
                            {
                                var cname = _context.MenuCategoryDetail.FirstOrDefault(a => a.menuCategoryId == menuCats.parentMenuCategoryId);
                                menuCats.ParentCategoryName = cname.menuCategoryName;
                                objMenuCats.Add(menuCats);
                            }
                            else
                            {
                                menuCats.ParentCategoryName = "N/A";
                                objMenuCats.Add(menuCats);
                            }
                        }

                    }
                    else
                    {
                        List<MenuCategoryDetail> objMenuCat = new List<MenuCategoryDetail>();
                        objMenuCat = _context.MenuCategoryDetail.Where(a => a.restaurantId == Convert.ToInt32(res)).ToList();
                        foreach (var menuCats in objMenuCat)
                        {
                            //  var rname = _context.RestaurantDetail.FirstOrDefault(a => a.id == Convert.ToInt32(res));
                            menuCats.RestaurantName = dummy.restaurantName;
                            if (menuCats.parentMenuCategoryId != null)
                            {
                                var cname = _context.MenuCategoryDetail.FirstOrDefault(a => a.menuCategoryId == menuCats.parentMenuCategoryId);
                                menuCats.ParentCategoryName = cname.menuCategoryName;
                                objMenuCats.Add(menuCats);
                            }
                            else
                            {
                                menuCats.ParentCategoryName = "N/A";
                                objMenuCats.Add(menuCats);
                            }
                        }

                    }

                    var dummyItems = objMenuCats;
                    var pager = new PagerMenuCategory(dummyItems.Count(), page);

                    var viewModel = new MenuCatePageViewModel
                    {
                        Items = dummyItems.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                        PagerMenuCategory = pager
                    };

                    return View(viewModel);
                }
                else
                    return View();

            }
            return RedirectToAction("QuickRegister", "Account");

        }

        [HttpGet]
        public async Task<IActionResult> MenuCategoryDetail(long Id)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["resturant"]))
                {
                    string res = HttpContext.Request.Query["resturant"].ToString();
                    ViewBag.restId = res;

                    List<MenuCategoryDetail> restaurantList = new List<MenuCategoryDetail>();
                    restaurantList = _context.MenuCategoryDetail.Where(a => a.restaurantId == Convert.ToInt32(res)).ToList(); ;
                    restaurantList.Insert(0, new MenuCategoryDetail { menuCategoryId = 0, menuCategoryName = "Select" });


                    var dummy = _context.RestaurantDetail.FirstOrDefault(a => a.id == Convert.ToInt32(res));
                    ViewBag.Rname = dummy.restaurantName;


                    ViewBag.Restaurants = restaurantList;
                    if (Id > 0)
                    {
                        var _restDetails = _context.MenuCategoryDetail.FirstOrDefault(a => a.menuCategoryId == Id);

                        return View(_restDetails);
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
            else
                return RedirectToAction("QuickRegister", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> GetCatonRestID(string restID)
        {
            //Here I'll bind the list of cities corresponding to selected state's state id  

            List<MenuCategoryDetail> CategoryList = new List<Models.MenuCategoryDetail>();
            int _restID = Convert.ToInt32(restID);

            CategoryList = _context.MenuCategoryDetail.Where(a => a.restaurantId == _restID).ToList();

            string result = JsonConvert.SerializeObject(CategoryList);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> MenuCategoryDetail(MenuCategoryDetail model)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                if (ModelState.IsValid)
                {
                    if (model.parentMenuCategoryId == 0)
                    {
                        model.parentMenuCategoryId = -1;
                    }

                    if (model.menuCategoryId != 0)
                    {

                        model.createdDate = DateTime.Now;
                        model.modifiedDate = DateTime.Now;
                        _context.MenuCategoryDetail.Update(model);
                        _context.SaveChanges();
                    }
                    else
                    {
                        model.createdDate = DateTime.Now;
                        model.sortOrder = _context.MenuCategoryDetail.Max(a => a.sortOrder) + 1;
                        _context.MenuCategoryDetail.Add(model);
                        _context.SaveChanges();
                    }

                }
                // return RedirectToAction("MenuCategories", "Admin");
                return Redirect("~/Admin/MenuCategories" + "?resturant=" + model.restaurantId);
            }
            else
                return RedirectToAction("QuickRegister", "Account");
        }

        public IActionResult DeleteMenuCategory(long id)
        {
            var rest = _context.MenuCategoryDetail.FirstOrDefault(a => a.menuCategoryId == id);
            if (rest != null)
            {
                _context.MenuCategoryDetail.Remove(rest);
                _context.SaveChanges();
            }

            return Redirect("~/Admin/MenuCategories" + "?resturant=" + rest.restaurantId);
        }

        [HttpGet]
        public async Task<IActionResult> MenuItems(int? page)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                string res = string.Empty;
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["resturant"]))
                    res = HttpContext.Request.Query["resturant"].ToString();

                string menuItemType = string.Empty;
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["itemtype"]))
                    menuItemType = HttpContext.Request.Query["itemtype"];

                ViewBag.menuItemType = menuItemType;

                var cats = _context.MenuCategoryDetail.FirstOrDefault(a => a.menuCategoryId == Convert.ToInt32(menuItemType) && a.parentMenuCategoryId == null);
                if (cats != null)
                    ViewBag.catName = cats.menuCategoryName;
                else
                    ViewBag.CatName = "N/A";

                ViewBag.restId = res;
                var dummy = _context.RestaurantDetail.FirstOrDefault(a => a.id == Convert.ToInt32(res));
                ViewBag.Rname = dummy.restaurantName;

                List<MenuItemDetail> objMenuCats = new List<MenuItemDetail>();
                var _menuItems = _context.MenuItemDetail.Where(a => a.restaurantId == Convert.ToInt32(res) && a.menuCategoryId == Convert.ToInt32(menuItemType)).ToList();
                foreach (var menuCats in _menuItems)
                {
                    var rname = _context.RestaurantDetail.FirstOrDefault(a => a.id == menuCats.restaurantId);
                    menuCats.restaurantName = rname.restaurantName;
                    if (menuCats.menuCategoryId != -1)
                    {
                        var cname = _context.MenuCategoryDetail.FirstOrDefault(a => a.menuCategoryId == menuCats.menuCategoryId);
                        menuCats.MenuItemCategoryName = cname.menuCategoryName;
                        objMenuCats.Add(menuCats);
                    }
                    else
                    {
                        menuCats.MenuItemCategoryName = "No Parent Category";
                        objMenuCats.Add(menuCats);
                    }
                }
                var dummyItems = objMenuCats;
                var pager = new PageMenuItems(dummyItems.Count(), page);

                var viewModel = new MenuItemPageViewModel
                {
                    Items = dummyItems.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    PageMenuItems = pager
                };

                return View(viewModel);
            }
            else
                return RedirectToAction("QuickRegister", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> MenuItemDetails(long Id)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                List<MenuCategoryDetail> CategoryList = new List<Models.MenuCategoryDetail>();


                string res = string.Empty;
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["resturant"]))
                    res = HttpContext.Request.Query["resturant"].ToString();

                var dummy = _context.RestaurantDetail.FirstOrDefault(a => a.id == Convert.ToInt32(res));
                ViewBag.Rname = dummy.restaurantName;

                string menuItemType = string.Empty;
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["itemtype"]))
                    menuItemType = HttpContext.Request.Query["itemtype"];

                ViewBag.menuItemType = menuItemType;

                var cats = _context.MenuCategoryDetail.FirstOrDefault(a => a.menuCategoryId == Convert.ToInt32(menuItemType));
                ViewBag.catName = cats.menuCategoryName;

                if (Id > 0)
                {

                    var _restDetails = _context.MenuItemDetail.FirstOrDefault(a => a.menuItemId == Id);

                    ViewBag.restId = _restDetails.restaurantId;

                    return View(_restDetails);
                }
                else
                {
                    string res1 = string.Empty;
                    if (!string.IsNullOrEmpty(HttpContext.Request.Query["resturant"]))
                        res1 = HttpContext.Request.Query["resturant"].ToString();

                    ViewBag.restId = res1;

                    return View();
                }
            }
            else
                return RedirectToAction("QuickRegister", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> MenuItemDetails(MenuItemDetail model)
        {
            string restaurantId = HttpContext.Request.Form["restId"];
            string menuCategoryId = HttpContext.Request.Form["menuCategoryId"];
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                string filepath = HttpContext.Request.Form["filePath"];
                if (!string.IsNullOrEmpty(filepath))
                {
                    CloudinaryDotNet.Account account = new CloudinaryDotNet.Account("hkm2gz727", "654416183426452", "AZJIv_WvBo1Z7gkzN-uXFVg2_BE");
                    Cloudinary cloudinary = new Cloudinary(account);

                    CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams();
                    uploadParams.File = new CloudinaryDotNet.Actions.FileDescription(filepath);

                    CloudinaryDotNet.Actions.ImageUploadResult uploadResult = await cloudinary.UploadAsync(uploadParams);
                    string url = cloudinary.Api.UrlImgUp.BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));
                    model.menuItemImageUrl = url;
                }
                else
                {
                    string imageurl = HttpContext.Request.Form["imageurl"];
                    model.menuItemImageUrl = imageurl;
                }

                model.menuCategoryId = Convert.ToInt32(menuCategoryId);
                model.restaurantId = Convert.ToInt32(restaurantId);

                // if (ModelState.IsValid)
                // {
                if (model.menuItemId != 0)
                {
                    model.createdDate = DateTime.Now;
                    _context.MenuItemDetail.Update(model);
                    _context.SaveChanges();
                }
                else
                {
                    model.createdDate = DateTime.Now;

                    model.sortOrder = _context.MenuItemDetail.Where(a => a.restaurantId == Convert.ToInt32(restaurantId) && a.menuCategoryId == Convert.ToInt32(menuCategoryId)).Max(a => a.sortOrder) + 1;
                    _context.MenuItemDetail.Add(model);
                    _context.SaveChanges();
                }

                // }
                return Redirect("~/Admin/MenuItems" + "?resturant=" + model.restaurantId + "&itemtype=" + model.menuCategoryId);
                //return RedirectToAction("MenuItems", "Admin");
            }
            else
                return RedirectToAction("QuickRegister", "Account");
        }

        public IActionResult DeleteMenuItems(long id)
        {
            var rest = _context.MenuItemDetail.FirstOrDefault(a => a.menuItemId == id);
            if (rest != null)
            {
                _context.MenuItemDetail.Remove(rest);
                _context.SaveChanges();
            }
            return Redirect("~/Admin/MenuItems" + "?resturant=" + rest.restaurantId);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        [HttpGet]
        public async Task<IActionResult> people(int? page)
        {

            TempData["ReturnUrl"] = "/Admin/people";
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["resturant"]))
                {
                    string res = HttpContext.Request.Query["resturant"].ToString();
                    ViewBag.restId = res;
                    var dummy = _context.RestaurantDetail.FirstOrDefault(a => a.id == Convert.ToInt32(res));
                    ViewBag.Rname = dummy.restaurantName;
                    List<PeopleDetail> objMenuCats = new List<PeopleDetail>();
                    objMenuCats = _context.PeopleDetail.Where(a => a.restaurantId == Convert.ToInt32(res)).ToList();

                    var dummyItems = objMenuCats;
                    var pager = new PagePeople(dummyItems.Count(), page);

                    var viewModel = new PeopleViewModel
                    {
                        Items = dummyItems.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                        PagePeoples = pager
                    };

                    return View(viewModel);
                }
                else
                    return View();
            }
            else
            {

                return RedirectToAction("QuickRegister", "Account", new { returnUrl = TempData["ReturnUrl"] });
            }
        }

        [HttpGet]
        public async Task<IActionResult> peopleDetail(long? Id)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                if (Id > 0)
                {
                    var _restDetails = _context.PeopleDetail.FirstOrDefault(a => a.id == Id);
                    ViewBag.restId = _restDetails.restaurantId;
                    return View(_restDetails);
                }
                else
                {
                    string res = HttpContext.Request.Query["resturant"].ToString();
                    ViewBag.restId = res;
                    return View();
                }
            }
            else
                return RedirectToAction("QuickRegister", "Account");

        }

        [HttpPost]
        public async Task<IActionResult> peopleDetail(PeopleDetail model)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                string rid = HttpContext.Request.Form["restId"];
                string sortOrder = HttpContext.Request.Form["sortOrder"];
                string firstName = HttpContext.Request.Form["fname"];
                string lastname = HttpContext.Request.Form["lname"];

                string filepath = HttpContext.Request.Form["filePath"];

                if (!string.IsNullOrEmpty(filepath))
                {
                    CloudinaryDotNet.Account account = new CloudinaryDotNet.Account("hkm2gz727", "654416183426452", "AZJIv_WvBo1Z7gkzN-uXFVg2_BE");
                    Cloudinary cloudinary = new Cloudinary(account);

                    CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams();
                    uploadParams.File = new CloudinaryDotNet.Actions.FileDescription(filepath);

                    CloudinaryDotNet.Actions.ImageUploadResult uploadResult = await cloudinary.UploadAsync(uploadParams);
                    string url = cloudinary.Api.UrlImgUp.BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));
                    model.imageUrl = url;
                }
                else
                {
                    string imageurl = HttpContext.Request.Form["imageurl"];
                    model.imageUrl = imageurl;
                }
                model.userId = user.Id;
                model.firstName = firstName;
                model.lastName = lastname;
                model.restaurantId = Convert.ToInt32(rid);
                model.createdDate = DateTime.Now;
                model.modifiedDate = DateTime.Now;
                if (model.id != 0)
                {
                    _context.PeopleDetail.Update(model);
                    _context.SaveChanges();
                }
                else
                {
                    model.sortOrder = _context.PeopleDetail.Where(a => a.restaurantId == Convert.ToInt32(rid)).Max(a => a.sortOrder) + 1;
                    _context.PeopleDetail.Add(model);
                    _context.SaveChanges();
                }


                return Redirect("~/Admin/people?resturant=" + model.restaurantId);
            }
            else
                return RedirectToAction("QuickRegister", "Account");
        }

        public IActionResult DeletpeopleDetail(long id)
        {
            var rest = _context.PeopleDetail.FirstOrDefault(a => a.id == id);
            if (rest != null)
            {
                _context.PeopleDetail.Remove(rest);
                _context.SaveChanges();
            }
            return Redirect("~/Admin/people?resturant=" + rest.restaurantId);
        }

        [HttpGet]
        public async Task<IActionResult> Images(int? page)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                string res = string.Empty;
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["resturant"]))
                    res = HttpContext.Request.Query["resturant"].ToString();

                ViewBag.rid = res;

                var dummy = _context.RestaurantDetail.FirstOrDefault(a => a.id == Convert.ToInt32(res));
                ViewBag.Rname = dummy.restaurantName;

                List<ImageDetail> imageDetail = new List<ImageDetail>();
                var _imageDetails = _context.ImageDetail.Where(a => a.restaurantId == Convert.ToInt32(res)).ToList();
                foreach (var imgs in _imageDetails)
                {
                    imgs.RestaurantName = dummy.restaurantName;
                    imageDetail.Add(imgs);
                }

                var dummyItems = imageDetail;
                var pager = new PageImages(dummyItems.Count(), page);

                var viewModel = new ImagesViewModel
                {
                    Items = dummyItems.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    PageImageitems = pager
                };

                return View(viewModel);
            }
            else
                return RedirectToAction("QuickRegister", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> ImageDetail(long? Id)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];

                if (Id > 0)
                {
                    var _restDetails = _context.ImageDetail.FirstOrDefault(a => a.id == Id);
                    ViewBag.restId = _restDetails.restaurantId;

                    var _rests = _context.RestaurantDetail.FirstOrDefault(a => a.id == _restDetails.restaurantId);
                    ViewBag.RestName = _rests.restaurantName;
                    return View(_restDetails);
                }
                else
                {
                    string restId = string.Empty;
                    restId = HttpContext.Request.Query["resturant"].ToString();
                    ViewBag.restId = restId;
                    return View();
                }
            }
            else
                return RedirectToAction("QuickRegister", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> ImageDetail(ImageDetail model, long? Id)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                string filepath = HttpContext.Request.Form["filePath"];
                string restId = string.Empty;

                restId = HttpContext.Request.Form["restId"];


                if (!string.IsNullOrEmpty(filepath))
                {
                    CloudinaryDotNet.Account account = new CloudinaryDotNet.Account("hkm2gz727", "654416183426452", "AZJIv_WvBo1Z7gkzN-uXFVg2_BE");
                    Cloudinary cloudinary = new Cloudinary(account);

                    CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams();
                    uploadParams.File = new CloudinaryDotNet.Actions.FileDescription(filepath);

                    CloudinaryDotNet.Actions.ImageUploadResult uploadResult = await cloudinary.UploadAsync(uploadParams);
                    string url = cloudinary.Api.UrlImgUp.BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));
                    model.imageUrl = url;
                }
                else
                {
                    string imageurl = HttpContext.Request.Form["imageurl"];
                    model.imageUrl = imageurl;
                }
                if (ModelState.IsValid)
                {
                    model.createdDate = DateTime.Now;
                    model.modifiedDate = DateTime.Now;
                    model.userId = user.Id;
                    model.restaurantId = Convert.ToInt32(restId);
                    if (model.id != 0)
                    {
                        _context.ImageDetail.Update(model);
                        _context.SaveChanges();
                    }
                    else
                    {
                        model.sortOrder = _context.ImageDetail.Max(a => a.sortOrder) + 1;
                        _context.ImageDetail.Add(model);
                        _context.SaveChanges();
                    }
                }

                return Redirect("~/Admin/Images?resturant=" + restId);
            }
            else
                return RedirectToAction("QuickRegister", "Account");
        }

        public IActionResult DeleteImages(long Id)
        {
            var rest = _context.ImageDetail.FirstOrDefault(a => a.id == Id);
            if (rest != null)
            {
                _context.ImageDetail.Remove(rest);
                _context.SaveChanges();
            }
            return Redirect("~/Admin/Images?resturant=" + rest.restaurantId);
        }

        [HttpGet]
        public async Task<IActionResult> Contacts(int? page)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                string res = string.Empty;
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["resturant"]))
                    res = HttpContext.Request.Query["resturant"].ToString();

                var dummy = _context.RestaurantDetail.FirstOrDefault(a => a.id == Convert.ToInt32(res));
                ViewBag.Rname = dummy.restaurantName;

                List<ContactDetails> imageDetail = new List<ContactDetails>();
                var _imageDetails = _context.ContactDetails.Where(a => a.restaurantId == Convert.ToInt32(res)).ToList();
                foreach (var imgs in _imageDetails)
                {
                    imgs.RestaurantName = dummy.restaurantName;
                    imageDetail.Add(imgs);
                }

                var dummyItems = imageDetail;
                var pager = new PageContact(dummyItems.Count(), page);

                var viewModel = new ContactsViewModel
                {
                    Items = dummyItems.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    PageContacts = pager
                };

                return View(viewModel);
            }
            else
                return RedirectToAction("QuickRegister", "Account");
        }

        public IActionResult DeleteContact(long id)
        {
            var rest = _context.ContactDetails.FirstOrDefault(a => a.contactId == id);
            if (rest != null)
            {
                _context.ContactDetails.Remove(rest);
                _context.SaveChanges();
            }
            return Redirect("~/Admin/Contacts?resturant=" + rest.restaurantId);
        }

        [HttpGet]
        public async Task<IActionResult> ContactDetails(long? id)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];

                var _contactsDetails = _context.ContactDetails.FirstOrDefault(a => a.contactId == Convert.ToInt32(id));
                var _rest = _context.RestaurantDetail.FirstOrDefault(a => a.id == _contactsDetails.restaurantId);
                _contactsDetails.RestaurantName = _rest.restaurantName;
                return View(_contactsDetails);
            }
            else
                return RedirectToAction("QuickRegister", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> privacyPolicies(int? page)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];
                string res = string.Empty;
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["resturant"]))
                    res = HttpContext.Request.Query["resturant"].ToString();

                var dummy = _context.RestaurantDetail.FirstOrDefault(a => a.id == Convert.ToInt32(res));
                ViewBag.Rname = dummy.restaurantName;

                ViewBag.rId = res;

                List<privacyPolicies> imageDetail = new List<privacyPolicies>();
                var _imageDetails = _context.privacyPolicies.Where(a => a.restaurantId == Convert.ToInt32(res)).FirstOrDefault();

                //   _imageDetails.restaurantId = Convert.ToInt32(res);


                return View(_imageDetails);
            }
            else
                return RedirectToAction("QuickRegister", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> privacyPolicies(privacyPolicies model)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                string title = HttpContext.Request.Form["ptitle"];
                string description = HttpContext.Request.Form["pdesc"];
                string restId = HttpContext.Request.Form["restId"];
                string Id = HttpContext.Request.Form["pId"];

                var role = await _userManager.GetRolesAsync(user);
                TempData["userRole"] = role[0];

                if (ModelState.IsValid)
                {
                    model.Title = title;
                    //  model.Description = description;
                    model.modifiedDate = DateTime.Now;
                    model.restaurantId = Convert.ToInt32(restId);
                    model.createdDate = DateTime.Now;

                    if (model.Id > 0)
                    {
                        model.Id = Convert.ToInt32(Id);
                        _context.privacyPolicies.Update(model);
                        _context.SaveChanges();
                    }
                    else
                    {
                        _context.privacyPolicies.Add(model);
                        _context.SaveChanges();
                    }
                }
                return Redirect("~/Admin/privacyPolicies?resturant=" + restId);
            }
            else
                return RedirectToAction("QuickRegister", "Account");
        }
    }
}