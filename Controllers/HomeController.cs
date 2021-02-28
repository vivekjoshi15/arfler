using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Arfler.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Arfler.Models.AccountViewModels;
using Arfler.Services;

namespace Arfler.Controllers
{
    public class HomeController : Controller
    {
        private readonly ArflerDBContext _context;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(UserManager<ApplicationUser> userManager, ArflerDBContext context, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public IActionResult Index(int id)
        {
            // ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            //  ViewData["UserId"] = HttpContext.Session.GetString("UserId");
            ViewData["categories"] = _context.CategoryDetail.ToList();
          //  youTubeProgram _objYT = new youTubeProgram();
           // _objYT.yTList();
            if (id != 0)
            {
                var _restaurant = _context.RestaurantDetail.ToList();
                List<RestaurantDetail> _rest = new List<RestaurantDetail>();
                foreach (var rr in _restaurant)
                {
                    if (!string.IsNullOrEmpty(rr.categoryIds))
                    {
                        string[] aa = rr.categoryIds.Split(',');
                        foreach (var _ver in aa)
                        {
                            if (Convert.ToInt32(_ver) == id)
                            {
                                _rest.Add(rr);
                            }
                        }
                        //for (int i = 0; i < aa.Length; i++)
                        //{
                        //    if (Convert.ToInt32(aa[i]) == id)
                        //    {
                        //        _rest.Add(rr);
                        //    }
                        //}
                    }
                }
                var rests = _rest;
                return View(rests);
            }
            else
            {
                return View(_context.RestaurantDetail.ToList());
            }
        }

        public IActionResult Resturant(int id)
        {
            if (id <= 0)
            {
                return View();
            }

            var resturant = _context.RestaurantDetail.SingleOrDefault(m => m.id == id);
            if (resturant != null)
            {
                return View(resturant);
            }
            else
                return View();
        }

        public ActionResult ActUser()
        {
            return View();
        }

        public ActionResult AboutUs(int id)
        {
            var _restDetails = _context.RestaurantDetail.SingleOrDefault(a => a.id == id);
            ViewBag.RestIntro = _restDetails.intro;
            ViewBag.RestDesc = _restDetails.Description;
            var images = _context.ImageDetail.Where(a => a.restaurantId == id).ToList().OrderByDescending(a => a.sortOrder);
            ViewData["images"] = images;
            return View(_restDetails);
        }

        public ActionResult Location(int id)
        {
            var _restDetails = _context.RestaurantDetail.SingleOrDefault(a => a.id == id);
            ViewData["address"] = _restDetails.address1 + " " + _restDetails.address2 + ", " + _restDetails.city + ", " + _restDetails.state + " " + _restDetails.zipCode + ", " + _restDetails.country;
            ViewBag.email = _restDetails.email;
            ViewBag.phone = _restDetails.phone;
            return View(_restDetails);
        }

        public ActionResult OurTeam(int id)
        {
            var _restDetails = _context.RestaurantDetail.SingleOrDefault(a => a.id == id);

            var PeopleDetail = _context.PeopleDetail.Where(a => a.restaurantId == id).ToList().OrderByDescending(a => a.sortOrder);
            ViewData["PeopleDetail"] = PeopleDetail;
            return View(_restDetails);
        }

        public ActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Review(long id)
        {
            var _restDetails = _context.RestaurantDetail.SingleOrDefault(a => a.id == id);
            ViewData["tagline"] = _restDetails.tagline;
            ViewData["mainImageUrl"] = _restDetails.mainImageUrl;
            ViewData["intro"] = _restDetails.intro;
            ViewData["Description"] = _restDetails.Description;
            ViewData["RestaurantId"] = id;

            var reviews = _context.ReviewDetail.Where(a => a.restaurantId == id).OrderByDescending(a => a.createdDate).ToList();

            return View(reviews);
        }

        [HttpPost]
        public IActionResult Review(ReviewDetail model)
        {
            string reviewContnt = HttpContext.Request.Form["reviewBox"];
            string restaurantId = HttpContext.Request.Form["restaurantId"];

            ReviewDetail objreview = new ReviewDetail();
            objreview.createdDate = DateTime.Now;
            objreview.restaurantId = Convert.ToInt32(restaurantId);
            objreview.review = reviewContnt;
            _context.ReviewDetail.Add(objreview);
            _context.SaveChanges();
            var reviews = _context.ReviewDetail.Where(a => a.restaurantId == Convert.ToInt32(restaurantId)).OrderByDescending(a => a.createdDate).ToList();

            return View(reviews);
        }

        public IActionResult Contact(long id)
        {
            //  ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            // ViewData["UserId"] = HttpContext.Session.GetString("UserId");
            var _restDetails = _context.RestaurantDetail.SingleOrDefault(a => a.id == id);
            ViewData["tagline"] = _restDetails.tagline;
            ViewData["mainImageUrl"] = _restDetails.mainImageUrl;
            ViewData["intro"] = _restDetails.intro;
            ViewData["Description"] = _restDetails.Description;
            ViewData["RestaurantId"] = id;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(long id, ContactViewModel model)
        {
            var _restDetails = _context.RestaurantDetail.SingleOrDefault(a => a.id == id);
            ViewData["tagline"] = _restDetails.tagline;
            ViewData["mainImageUrl"] = _restDetails.mainImageUrl;
            ViewData["intro"] = _restDetails.intro;
            ViewData["Description"] = _restDetails.Description;
            ViewData["RestaurantId"] = id;

            if (ModelState.IsValid)
            {
                ContactDetails _contactDetail = new ContactDetails();
                _contactDetail.contactEmail = model.Email;
                _contactDetail.contactMessage = model.Message;
                _contactDetail.contactName = model.Name;
                _contactDetail.createdDate = DateTime.Now;
                _contactDetail.cSubject = model.cSubject;

                _context.ContactDetails.Add(_contactDetail);
                _context.SaveChanges();
                string msg = string.Empty;
                string subj = string.Empty;
                subj = model.cSubject;
                msg += "A new message is recieved from " + model.Name + "<br/>";
                msg += model.Message;
                await _emailSender.SendEmailAsync(model.Email, subj, msg);

            }
            return View();
        }

        public ActionResult Reservation(int id)
        {
            var _restDetails = _context.RestaurantDetail.SingleOrDefault(a => a.id == id);
            List<SelectListItem> _catList = new List<SelectListItem>();
            var cats = _context.MenuCategoryDetail.Where(a => a.restaurantId == Convert.ToInt32(id) && a.parentMenuCategoryId == null).ToList();
            foreach (var item in cats)
            {
                _catList.Add(new SelectListItem
                {
                    Text = item.menuCategoryName,
                    Value = item.menuCategoryId.ToString()
                });
            }

            // _catList.Insert(0, new CategoryDetail { CategoryId = 0, CategoryName = "Select" });
            ViewData["CatList"] = _catList;
            return View(_restDetails);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservation(RestaurantDetail model)
        {
            string firstName = HttpContext.Request.Form["firstName"];
            string lastName = HttpContext.Request.Form["lastName"];
            string email = HttpContext.Request.Form["email"];
            string Phone = HttpContext.Request.Form["Phone"];
            string noGuests = HttpContext.Request.Form["noGuests"];
            string datepicker = HttpContext.Request.Form["datepicker"];
            string ddTime = HttpContext.Request.Form["ddTime"];
            string resType = HttpContext.Request.Form["resType"];
            ReservationDetail _resvDetail = new ReservationDetail();
            _resvDetail.createdDate = DateTime.Now;
            _resvDetail.firstName = firstName;
            _resvDetail.lastName = lastName;
            _resvDetail.guestNum = Convert.ToInt32(noGuests);
            _resvDetail.reservationDate = datepicker;
            _resvDetail.reservationEmail = email;
            _resvDetail.reservationPhone = Phone;
            _resvDetail.reservationTime = ddTime;
            _resvDetail.reservationType = resType;
            _resvDetail.restaurantId = model.id;
            _context.ReservationDetail.Add(_resvDetail);
            _context.SaveChanges();

            //to admin
            string msg = string.Empty;
            msg += "A new Reservation has been made at  " + @model.restaurantName + " by " + firstName + " " + lastName + "<br/>";
            msg += "<b> Total Number of Guests : </b>" + noGuests + "<br/> <b> " + firstName + "'s Phone Number </b>" + Phone + " and <b> email address :</b>" + email;
            msg += "<br/> Reservation is made for Day : " + datepicker + " at " + ddTime + " for " + resType;

            await _emailSender.SendEmailAsync(email, "New Reservation request", msg);

            ////to user
            //string msg1 = string.Empty;
            //msg1 += "Hello " + firstName + " " + lastName + ",<br/>";
            //msg1 += "Your request for resrvation at " + model.restaurantName + " for " + resType + " on " + datepicker + " at " + ddTime + " Has been recieved. Our staff will wait for you thanks.<br/>";

            //await _emailSender.SendEmailAsync(email, "Reservation at Arfler", msg1);

            return View();
        }

        public IActionResult QuickAdd(int? id)
        {
            ViewBag.rId = id;
            List<CategoryDetail> CategoryList = new List<Models.CategoryDetail>();
            CategoryList = _context.CategoryDetail.ToList();
            CategoryList.Insert(0, new CategoryDetail { CategoryId = 0, CategoryName = "Select" });
            ViewBag.Categories = CategoryList;

            if (id != null)
            {
                var restDetail = _context.RestaurantDetail.FirstOrDefault(a => a.id == id);
                return View(restDetail);
            }
            else
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> QuickAdd(RestaurantDetail model, long? Id)
        {
            string zipCode = HttpContext.Request.Form["ZipCode"];
            if (Id == null)
            {
                //var user = await GetCurrentUserAsync();
                //  model.userId = user.id;
                model.zipCode = zipCode;
                model.createdDate = DateTime.Now;
                model.modifiedDate = DateTime.Now;
                _context.RestaurantDetail.Add(model);
                _context.SaveChanges();
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
                return RedirectToAction("AddCategories", "Home", new { id = restId });
            }
            else
            {
                RestaurantDetail objrest = new RestaurantDetail();
                string add2 = HttpContext.Request.Form["Address2"];
                string tagline = HttpContext.Request.Form["tagline"];
                string userId = HttpContext.Request.Form["userId"];
                string Desc = HttpContext.Request.Form["Desc"];
                string country = HttpContext.Request.Form["country"];
                string rimage = HttpContext.Request.Form["rimage"];
                string Rcategoryids = HttpContext.Request.Form["Rcategoryids"];
                string isenable = HttpContext.Request.Form["isenable"];
                string sortOrder = HttpContext.Request.Form["sortOrder"];

                objrest.address1 = model.address1;
                if (!string.IsNullOrEmpty(add2))
                    objrest.address2 = add2;
                else
                    objrest.address2 = null;

                if (!string.IsNullOrEmpty(tagline))
                    objrest.tagline = tagline;
                else
                    objrest.tagline = null;

                if (!string.IsNullOrEmpty(userId))
                    objrest.userId = userId;
                else
                    objrest.userId = null;

                if (!string.IsNullOrEmpty(Desc))
                    objrest.Description = Desc;
                else
                    objrest.Description = null;

                if (!string.IsNullOrEmpty(country))
                    objrest.country = country;
                else
                    objrest.country = null;

                if (!string.IsNullOrEmpty(rimage))
                    objrest.mainImageUrl = rimage;
                else
                    objrest.mainImageUrl = null;

                if (!string.IsNullOrEmpty(Rcategoryids))
                    objrest.categoryIds = Rcategoryids;
                else
                    objrest.categoryIds = null;


                objrest.isEnable = true;


                if (!string.IsNullOrEmpty(sortOrder))
                    objrest.sortOrder = Convert.ToInt32(sortOrder);



                objrest.zipCode = zipCode;
                objrest.city = model.city;
                objrest.state = model.state;
                objrest.intro = model.intro;
                objrest.restaurantName = model.restaurantName;
                objrest.createdDate = DateTime.Now;
                objrest.modifiedDate = DateTime.Now;
                objrest.id = Convert.ToInt32(Id);
                _context.Update(objrest);
                _context.SaveChanges();

                return RedirectToAction("AddCategories", "Home", new { id = Id });
            }
        }


        public IActionResult AddCategories(int? id)
        {
            ViewBag.rId = id;
            var restCats = _context.RestaurantDetail.FirstOrDefault(a => a.id == Convert.ToInt32(id));

            if (restCats != null)
                ViewBag.CatsRestaurant = restCats.categoryIds;

            List<MenuCategoryDetail> lstmenucats = new List<MenuCategoryDetail>();
            lstmenucats = _context.MenuCategoryDetail.Where(a => a.restaurantId == Convert.ToInt32(id)).ToList();
            ViewBag.menuCats = lstmenucats;
            List<MenuCategoryDetail> lstmenuDDcats = new List<MenuCategoryDetail>();
            lstmenuDDcats = _context.MenuCategoryDetail.Where(a => a.restaurantId == Convert.ToInt32(id) && a.parentMenuCategoryId == null).ToList();
            lstmenuDDcats.Insert(0, new MenuCategoryDetail { menuCategoryId = 0, menuCategoryName = "No Parent" });
            ViewBag.menuDDCats = lstmenuDDcats;

            List<CategoryDetail> CategoryList = new List<Models.CategoryDetail>();
            CategoryList = _context.CategoryDetail.ToList();
            CategoryList.Insert(0, new CategoryDetail { CategoryId = 0, CategoryName = "Select" });
            ViewBag.Categories = CategoryList;
            return View();
        }

        [HttpPost]
        public IActionResult AddCategories(MenuCategoryDetail model, string stepTwo, string update, long? id)
        {
            string rid = HttpContext.Request.Form["rid"];
            if (!string.IsNullOrEmpty(stepTwo))
            {
                if (stepTwo.ToLower() == "save and continue")
                {
                    long Id = Convert.ToInt32(id);
                    string parentCategory = HttpContext.Request.Form["CatName"];
                    string NewCategory = HttpContext.Request.Form["NewCategory"];
                    string menuCategoryDesc = HttpContext.Request.Form["menuCategoryDesc"];

                    if (!string.IsNullOrEmpty(NewCategory))
                    {
                        MenuCategoryDetail _menucats = new MenuCategoryDetail();
                        _menucats.createdDate = DateTime.Now;
                        _menucats.isEnabled = true;
                        _menucats.menuCategoryDesc = menuCategoryDesc;
                        _menucats.menuCategoryName = NewCategory;
                        _menucats.modifiedDate = DateTime.Now;

                        long pcatId = 0;
                        if (!string.IsNullOrEmpty(parentCategory))
                        {
                            if (parentCategory.ToLower() != "no parent")
                            {
                                var pcats = _context.MenuCategoryDetail.FirstOrDefault(a => a.restaurantId == Convert.ToInt32(rid) && a.menuCategoryName.ToLower() == parentCategory.ToLower());
                                _menucats.parentMenuCategoryId = pcats.menuCategoryId;
                                pcatId = pcats.menuCategoryId;
                            }
                        }
                        _menucats.restaurantId = Convert.ToInt32(rid);
                        var _sorder = _context.MenuCategoryDetail.Where(a => a.restaurantId == Convert.ToInt32(rid) && a.parentMenuCategoryId == pcatId).Max(a => a.sortOrder);
                        int sortOrder = 0;
                        if (_sorder > 0)
                            _menucats.sortOrder = _sorder + 1;
                        else
                            _menucats.sortOrder = _sorder + 1;

                        _context.MenuCategoryDetail.Add(_menucats);
                        _context.SaveChanges();

                    }
                }

            }
            else if (!string.IsNullOrEmpty(update))
            {
                if (update.ToLower() == "update")
                {
                    string parentCategory = HttpContext.Request.Form["CatName"];
                    string NewCategory = HttpContext.Request.Form["NewCategory"];
                    string menuCategoryDesc = HttpContext.Request.Form["menuCategoryDesc"];
                    string catId = HttpContext.Request.Form["mcId"];
                    string ddMenuCategories = HttpContext.Request.Form["pcid"];
                    string Sorder = HttpContext.Request.Form["Sorder"];
                    MenuCategoryDetail _menucats = new MenuCategoryDetail();
                    long pcatId = 0;
                    if (!string.IsNullOrEmpty(parentCategory))
                    {
                        if (parentCategory.ToLower() != "no parent")
                        {
                            var pcats = _context.MenuCategoryDetail.FirstOrDefault(a => a.restaurantId == Convert.ToInt32(rid) && a.menuCategoryName.ToLower() == parentCategory.ToLower());
                            _menucats.parentMenuCategoryId = pcats.menuCategoryId;
                            pcatId = pcats.menuCategoryId;
                        }
                    }
                    _menucats.restaurantId = Convert.ToInt32(rid);
                    _menucats.menuCategoryDesc = menuCategoryDesc;
                    _menucats.menuCategoryName = NewCategory;
                    _menucats.menuCategoryId = Convert.ToInt32(catId);
                    _menucats.createdDate = DateTime.Now;
                    _menucats.parentMenuCategoryId = Convert.ToInt32(ddMenuCategories);
                    _menucats.isEnabled = true;
                    _menucats.modifiedDate = DateTime.Now;
                    _menucats.sortOrder = Convert.ToInt32(Sorder);
                    _context.MenuCategoryDetail.Update(_menucats);
                    _context.SaveChanges();

                }
            }
            return RedirectToAction("SelectMenuItems", "Home", new { id = rid });
        }

        public IActionResult DeleteMenuCategories(long id)
        {
            var rest = _context.MenuCategoryDetail.FirstOrDefault(a => a.menuCategoryId == id);
            var restId = rest.restaurantId;
            if (rest != null)
            {
                _context.MenuCategoryDetail.Remove(rest);
                _context.SaveChanges();
            }
            return RedirectToAction("SelectMenuItems", "Home", new { id = restId });
        }

        public IActionResult SelectMenuItems(long? id)
        {
            ViewBag.rId = id;
            List<MenuCategoryDetail> CategoryList = new List<Models.MenuCategoryDetail>();
            CategoryList = _context.MenuCategoryDetail.Where(a => a.restaurantId == id).ToList();
            CategoryList.Insert(0, new MenuCategoryDetail { menuCategoryId = 0, menuCategoryName = "Select" });
            ViewBag.Categories = CategoryList;


            List<MenuItemDetail> _menuItem = new List<MenuItemDetail>();
            var menuItems = _context.MenuItemDetail.Where(a => a.restaurantId == id).ToList();
            foreach (var menuItem in menuItems)
            {
                MenuItemDetail _objMenuItem = new MenuItemDetail();
                _objMenuItem = menuItem;
                _objMenuItem.MenuItemCategoryName = _context.MenuCategoryDetail.FirstOrDefault(a => a.menuCategoryId == menuItem.menuCategoryId).menuCategoryName;
                _menuItem.Add(_objMenuItem);
            }
            ViewBag.MenuItemList = _menuItem;
            return View();
        }
        [HttpPost]
        public IActionResult SelectMenuItems(MenuItemDetail model, long? id)
        {
            string menuCat = HttpContext.Request.Form["menuCat"];
            string MenuItem = HttpContext.Request.Form["MenuItem"];
            string MenuItemPrice = HttpContext.Request.Form["MenuItemPrice"];

            string menuCatId = HttpContext.Request.Form["menuCatId"];
            string menuItemId = HttpContext.Request.Form["menuItemId"];
            string menuItemName = HttpContext.Request.Form["menuItemName"];
            string ItemPrice = HttpContext.Request.Form["ItemPrice"];
            string sortOrder = HttpContext.Request.Form["sortOrder"];

            long Id = Convert.ToInt32(id);
            string[] cats = menuCatId.Split(',');
            string[] itemName = menuItemName.Split(',');
            string[] itemPrice = ItemPrice.Split(',');
            string[] sorder = sortOrder.Split(',');
            string[] _menuItemId = menuItemId.Split(',');
            for (int i = 0; i < cats.Length; i++)
            {
                if (Convert.ToInt32(_menuItemId[i]) > 0)
                {
                    MenuItemDetail _catdet = new MenuItemDetail();
                    _catdet.createdDate = DateTime.Now;
                    _catdet.menuCategoryId = Convert.ToInt32(cats[i]);
                    _catdet.menuItemName = itemName[i];
                    _catdet.menuItemRate = itemPrice[i];
                    _catdet.restaurantId = Id;
                    _catdet.menuItemId = Convert.ToInt32(_menuItemId[i]);
                    _catdet.sortOrder = Convert.ToInt32(sortOrder[i]);

                    _context.MenuItemDetail.Update(_catdet);
                    _context.SaveChanges();
                }
                else
                {
                    long sort = 0;
                    var _sorder = _context.MenuItemDetail.Where(a => a.restaurantId == id && a.menuCategoryId == Convert.ToInt32(cats[i])).Max(a => a.sortOrder);
                    if (_sorder < 1)
                        sort = 0;
                    else
                        sort = _sorder + 1;

                    MenuItemDetail _catdet = new MenuItemDetail();
                    _catdet.createdDate = DateTime.Now;
                    _catdet.menuCategoryId = Convert.ToInt32(cats[i]);
                    _catdet.menuItemName = itemName[i];
                    _catdet.menuItemRate = itemPrice[i];
                    _catdet.restaurantId = Id;
                    _catdet.sortOrder = sort;

                    _context.MenuItemDetail.Add(_catdet);
                    _context.SaveChanges();

                }
            }
            return RedirectToAction("index", "home");
        }

        public IActionResult Search()
        {
            ViewData["categories"] = _context.CategoryDetail.ToList();
            string text = HttpContext.Request.Query["text"].ToString();
            var rest = _context.RestaurantDetail.Where(a => a.restaurantName.Contains(text)).ToList();
            return View(rest);
        }

        public IActionResult PrivacyPolicy(long? id)
        {
            var _restDetails = _context.RestaurantDetail.SingleOrDefault(a => a.id == id);

            var policy = _context.privacyPolicies.Where(a => a.restaurantId == id).FirstOrDefault();
            ViewData["policyTitle"] = policy.Title;
            ViewData["policyDesc"] = policy.Description;
            ViewData["tagline"] = _restDetails.tagline;
            ViewData["mainImageUrl"] = _restDetails.mainImageUrl;
            ViewData["intro"] = _restDetails.intro;
            ViewData["Description"] = _restDetails.Description;
            ViewData["RestaurantId"] = id;
            return View(_restDetails);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
