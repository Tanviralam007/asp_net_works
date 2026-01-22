using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger_Food_Distribution.Helpers;
using ZeroHunger_Food_Distribution.Models;

namespace ZeroHunger_Food_Distribution.Controllers
{
    [CustomAuthorize(UserRoles.Admin)]
    public class AdminController : BaseController
    {
        private ZeroHungerDBEntities db = new ZeroHungerDBEntities();

        // GET: Admin/Dashboard
        public ActionResult Dashboard()
        {
            try
            {
                // sys wide stat
                var allRequests = db.CollectRequests.ToList();
                ViewBag.TotalRequests = allRequests.Count;
                
                ViewBag.PendingRequests = allRequests.Count(r => r.RequestStatus == RequestStatus.Pending);
                ViewBag.AssignedRequests = allRequests.Count(r => r.RequestStatus == RequestStatus.Assigned);
                ViewBag.CollectedRequests = allRequests.Count(r => r.RequestStatus == RequestStatus.Collected);
                ViewBag.CompletedRequests = allRequests.Count(r => r.RequestStatus == RequestStatus.Completed);

                // additional stat
                ViewBag.TotalRestaurants = db.Restaurants.Count(r => r.IsActive);
                ViewBag.TotalEmployees = db.Employees.Count(e => e.IsActive);
                ViewBag.ActiveRequests = allRequests.Count(
                    r => r.RequestStatus == RequestStatus.Assigned 
                    || r.RequestStatus == RequestStatus.Pending);

                // recent requests
                var recentRequests = db.CollectRequests
                    .OrderByDescending(r => r.RequestedDate)
                    .Take(10)
                    .ToList();

                ViewBag.RecentRequests = recentRequests;
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading Dashboard" + ex.Message;
                return View();
            }
        }

        // GET: Admin/AllRequests
        public ActionResult AllRequests(string searchId, string statusFilter)
        {
            try
            {
                // get all requests
                var allRequests = db.CollectRequests
                    .OrderByDescending(r => r.RequestedDate)
                    .ToList();

                // apply search by ID
                if (!string.IsNullOrEmpty(searchId))
                {
                    int requestId;
                    if (int.TryParse(searchId, out requestId))
                    {
                        allRequests = allRequests.Where(r => r.CollectRequestId == requestId).ToList();
                    }
                }

                // apply status filter
                if (!string.IsNullOrEmpty(statusFilter))
                {
                    allRequests = allRequests.Where(r => r.RequestStatus == statusFilter).ToList();
                }

                // pass search values to view
                ViewBag.SearchId = searchId;
                ViewBag.StatusFilter = statusFilter;

                return View(allRequests);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading requests: " + ex.Message;
                return RedirectToAction("Dashboard");
            }
        }

        // GET: Admin/AssignEmployee/5
        public ActionResult AssignEmployee(int id)
        {
            try
            {
                // get the request
                var request = db.CollectRequests.Find(id);

                if (request == null)
                {
                    TempData["ErrorMessage"] = "Request not found";
                    return RedirectToAction("AllRequests");
                }

                // check if already assigned
                if (request.RequestStatus != "Pending")
                {
                    TempData["ErrorMessage"] = "This request is not pending";
                    return RedirectToAction("AllRequests");
                }

                // get all active employees
                var employees = db.Employees
                    .Where(e => e.IsActive)
                    .ToList();

                ViewBag.RequestId = id;
                ViewBag.Request = request;
                ViewBag.Employees = employees;

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
                return RedirectToAction("AllRequests");
            }
        }

        // POST: Admin/AssignEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignEmployee(int requestId, int employeeId)
        {
            try
            {
                // get the request
                var request = db.CollectRequests.Find(requestId);

                if (request == null)
                {
                    TempData["ErrorMessage"] = "Request not found";
                    return RedirectToAction("AllRequests");
                }

                // get the employee
                var employee = db.Employees.Find(employeeId);

                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee not found";
                    return RedirectToAction("AssignEmployee", new { id = requestId });
                }

                // assign the employee
                request.AssignedEmployeeId = employeeId;
                request.RequestStatus = "Assigned";

                db.SaveChanges();

                TempData["SuccessMessage"] = "Employee assigned successfully!";
                return RedirectToAction("AllRequests");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error assigning employee: " + ex.Message;
                return RedirectToAction("AllRequests");
            }
        }

        // GET: Admin/RequestDetails/5
        public ActionResult RequestDetails(int id)
        {
            try
            {
                // get the request with all related data
                var request = db.CollectRequests.Find(id);

                if (request == null)
                {
                    TempData["ErrorMessage"] = "Request not found";
                    return RedirectToAction("AllRequests");
                }

                return View(request);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading request: " + ex.Message;
                return RedirectToAction("AllRequests");
            }
        }

        // GET: Admin/Employees
        public ActionResult Employees()
        {
            try
            {
                // get all employees with user info
                var employees = db.Employees
                    .OrderByDescending(e => e.IsActive)
                    .ThenBy(e => e.EmployeeCode)
                    .ToList();

                // get assignment counts for each employee
                var assignmentCounts = db.CollectRequests
                    .Where(r => r.AssignedEmployeeId.HasValue)
                    .GroupBy(r => r.AssignedEmployeeId.Value)
                    .Select(g => new { EmployeeId = g.Key, Count = g.Count() })
                    .ToDictionary(x => x.EmployeeId, x => x.Count);

                ViewBag.AssignmentCounts = assignmentCounts;

                return View(employees);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading employees: " + ex.Message;
                return RedirectToAction("Dashboard");
            }
        }

        // GET: Admin/EmployeeDetails/5
        public ActionResult EmployeeDetails(int id)
        {
            try
            {
                // get employee with user info
                var employee = db.Employees.Find(id);

                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee not found";
                    return RedirectToAction("Employees");
                }

                // get all requests assigned to this employee
                var assignedRequests = db.CollectRequests
                    .Where(r => r.AssignedEmployeeId == id)
                    .OrderByDescending(r => r.RequestedDate)
                    .ToList();

                ViewBag.AssignedRequests = assignedRequests;

                // get statistics
                ViewBag.TotalAssignments = assignedRequests.Count;
                ViewBag.PendingAssignments = assignedRequests.Count(r => r.RequestStatus == "Assigned");
                ViewBag.CollectedAssignments = assignedRequests.Count(r => r.RequestStatus == "Collected");
                ViewBag.CompletedAssignments = assignedRequests.Count(r => r.RequestStatus == "Completed");

                return View(employee);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading employee details: " + ex.Message;
                return RedirectToAction("Employees");
            }
        }

    }
}