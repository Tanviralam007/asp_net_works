using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger_Food_Distribution.Helpers;
using ZeroHunger_Food_Distribution.Models;
using ZeroHunger_Food_Distribution.ViewModels;

namespace ZeroHunger_Food_Distribution.Controllers
{
    [CustomAuthorize(UserRoles.Restaurant)]
    public class RestaurantController : BaseController
    {
        private ZeroHungerDBEntities db = new ZeroHungerDBEntities();

        // GET: Restaurant/Dashboard
        public ActionResult Dashboard()
        {
            try
            {
                // get current user id
                int userId = GetCurrentUserId();

                // get rastaurant data
                var restaurant = db.Restaurants.FirstOrDefault(r => r.UserId == userId);

                if (restaurant == null)
                {
                    TempData["ErrorMessage"] = "Restaurant Profile Not Found!";
                    return RedirectToAction("Login", "Account");
                }

                // get stat
                var allRequests = db.CollectRequests.Where(r => r.RestaurantId == restaurant.RestaurantId).ToList();

                ViewBag.TotalRequests = allRequests.Count;
                ViewBag.PendingRequests = allRequests.Count(r => r.RequestStatus == RequestStatus.Pending);
                ViewBag.AssignedRequests = allRequests.Count(r => r.RequestStatus == RequestStatus.Assigned);
                ViewBag.CompletedRequests = allRequests.Count(r => r.RequestStatus == RequestStatus.Completed);

                // get recent requests
                var recentRequests = allRequests.
                    OrderByDescending(r => r.RequestedDate)
                    .Take(5)
                    .ToList();

                ViewBag.RecentRequests = recentRequests;
                ViewBag.RestaurantName = restaurant.RestaurantName;

                return View();
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "Error Loading Dashboard" + ex.Message;
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Restaurant/CreateRequest
        public ActionResult CreateRequest()
        {
            return View();
        }

        // POST: Restaurant/CreateRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRequest(CollectRequestViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // get current user id
                int userId = GetCurrentUserId();
                
                // get restaurant records
                var restaurant = db.Restaurants.FirstOrDefault(x => x.UserId == userId);
                
                if (restaurant == null)
                {
                    TempData["ErrorMessage"] = "Restaurant Profile Not Found!";
                    return RedirectToAction("Dashboard");
                }

                // validate preservation time
                if (model.PreservationTime <= DateTime.Now)
                {
                    ModelState.AddModelError("PreservationTime", "Preservation time must be in the future");
                    return View(model);
                }

                // create new collection request
                var collectRequest = new CollectRequest
                {
                    RestaurantId = restaurant.RestaurantId,
                    FoodDescription = model.FoodDescription,
                    ApproximateQuantity = model.ApproximateQuantity,
                    PreservationTime = model.PreservationTime,
                    Remarks = model.Remarks ?? string.Empty,
                    RequestStatus = "Pending",
                    AssignedEmployeeId = null,
                    RequestedDate = DateTime.Now
                };

                db.CollectRequests.Add(collectRequest);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Collection request has created!";
                return RedirectToAction("Dashboard");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Error Creating Request" + ex.Message);
                return View(model);
            }
        }

        // GET: Restaurant/MyRequests
        public ActionResult MyRequests()
        {
            try
            {
                var userId = GetCurrentUserId();
                var restaurant = db.Restaurants.FirstOrDefault(x => x.UserId == userId);
                if (restaurant == null)
                {
                    TempData["ErrorMessage"] = "Restaurant profile not found";
                    return RedirectToAction("Dashboard");
                }

                // get all requests for this restaurant, ordered by date (newest first)
                var requests = db.CollectRequests
                    .Where(r => r.RestaurantId == restaurant.RestaurantId)
                    .OrderByDescending(r => r.RequestedDate)
                    .ToList();

                return View(requests);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading page" + ex.Message;
                return RedirectToAction("Dashboard");
            }
        }

        // GET: Restaurant/RequestDetails/5
        public ActionResult RequestDetails(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var restaurant = db.Restaurants.FirstOrDefault(x => x.UserId == userId);
                if (restaurant == null)
                {
                    TempData["ErrorMessage"] = "Restaurant profile not found";
                    return RedirectToAction("Dashboard");
                }

                // get all requests for this restaurant, ordered by date (newest first)
                var request = db.CollectRequests
                    .FirstOrDefault(r => r.CollectRequestId == id && 
                                    r.RestaurantId == restaurant.RestaurantId);
                if(request == null)
                {
                    TempData["ErrorMessage"] = "Request not found or access denied";
                    return RedirectToAction("MyRequests");
                }

                return View(request);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading request: " + ex.Message;
                return RedirectToAction("MyRequests");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}