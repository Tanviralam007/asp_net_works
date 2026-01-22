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
    [CustomAuthorize(UserRoles.Employee)]
    public class EmployeeController : BaseController
    {
        private ZeroHungerDBEntities db = new ZeroHungerDBEntities();

        // GET: Employee/Dashboard
        public ActionResult Dashboard()
        {
            try
            {
                // get current user ID
                int userId = GetCurrentUserId();

                // get employee record
                var employee = db.Employees.FirstOrDefault(e => e.UserId == userId);

                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee profile not found";
                    return RedirectToAction("Login", "Account");
                }

                // get all assignments for this employee
                var myAssignments = db.CollectRequests
                    .Where(r => r.AssignedEmployeeId == employee.EmployeeId)
                    .OrderByDescending(r => r.RequestedDate)
                    .ToList();

                // Statistics
                ViewBag.TotalAssignments = myAssignments.Count;
                ViewBag.PendingAssignments = myAssignments.Count(r => r.RequestStatus == "Assigned");
                ViewBag.CollectedAssignments = myAssignments.Count(r => r.RequestStatus == "Collected");
                ViewBag.CompletedAssignments = myAssignments.Count(r => r.RequestStatus == "Completed");

                // recent assignments
                ViewBag.MyAssignments = myAssignments.Take(10).ToList();

                if (employee.User != null)
                {
                    ViewBag.EmployeeName = employee.User.FullName;
                }

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading dashboard: " + ex.Message;
                return View();
            }
        }

        // GET: Employee/MyAssignments
        public ActionResult MyAssignments(string searchId, string statusFilter)
        {
            try
            {
                // Get current user ID
                int userId = GetCurrentUserId();

                // Get employee record
                var employee = db.Employees.FirstOrDefault(e => e.UserId == userId);

                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee profile not found";
                    return RedirectToAction("Dashboard");
                }

                // Get all assignments
                var assignments = db.CollectRequests
                    .Where(r => r.AssignedEmployeeId == employee.EmployeeId)
                    .OrderByDescending(r => r.RequestedDate)
                    .ToList();

                // Apply search by ID
                if (!string.IsNullOrEmpty(searchId))
                {
                    int requestId;
                    if (int.TryParse(searchId, out requestId))
                    {
                        assignments = assignments.Where(r => r.CollectRequestId == requestId).ToList();
                    }
                }

                // Apply status filter
                if (!string.IsNullOrEmpty(statusFilter))
                {
                    assignments = assignments.Where(r => r.RequestStatus == statusFilter).ToList();
                }

                // Pass search values to view
                ViewBag.SearchId = searchId;
                ViewBag.StatusFilter = statusFilter;

                return View(assignments);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading assignments: " + ex.Message;
                return RedirectToAction("Dashboard");
            }
        }


        // GET: Employee/AssignmentDetails/5
        public ActionResult AssignmentDetails(int id)
        {
            try
            {
                // get current user ID
                int userId = GetCurrentUserId();

                // get employee record
                var employee = db.Employees.FirstOrDefault(e => e.UserId == userId);

                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee profile not found";
                    return RedirectToAction("Dashboard");
                }

                // get the assignment
                var assignment = db.CollectRequests
                    .FirstOrDefault(r => r.CollectRequestId == id && r.AssignedEmployeeId == employee.EmployeeId);

                if (assignment == null)
                {
                    TempData["ErrorMessage"] = "Assignment not found or access denied";
                    return RedirectToAction("MyAssignments");
                }

                return View(assignment);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading assignment: " + ex.Message;
                return RedirectToAction("MyAssignments");
            }
        }

        // POST: Employee/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int requestId, string newStatus)
        {
            try
            {
                // get current user ID
                int userId = GetCurrentUserId();

                // get employee record
                var employee = db.Employees.FirstOrDefault(e => e.UserId == userId);

                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee profile not found";
                    return RedirectToAction("Dashboard");
                }

                // get the request
                var request = db.CollectRequests
                    .FirstOrDefault(r => r.CollectRequestId == requestId && r.AssignedEmployeeId == employee.EmployeeId);

                if (request == null)
                {
                    TempData["ErrorMessage"] = "Request not found or access denied";
                    return RedirectToAction("MyAssignments");
                }

                // validate status transition
                if (request.RequestStatus == "Assigned" && newStatus == "Collected")
                {
                    request.RequestStatus = "Collected";
                }
                else if (request.RequestStatus == "Collected" && newStatus == "Completed")
                {
                    request.RequestStatus = "Completed";
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid status transition";
                    return RedirectToAction("AssignmentDetails", new { id = requestId });
                }

                db.SaveChanges();

                TempData["SuccessMessage"] = $"Status updated to {newStatus} successfully!";
                return RedirectToAction("MyAssignments");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating status: " + ex.Message;
                return RedirectToAction("MyAssignments");
            }
        }

        // GET: Employee/RecordCollection/5
        public ActionResult RecordCollection(int id)
        {
            try
            {
                int userId = GetCurrentUserId();
                var employee = db.Employees.FirstOrDefault(e => e.UserId == userId);

                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee profile not found";
                    return RedirectToAction("Dashboard");
                }

                // Get the assignment
                var assignment = db.CollectRequests
                    .FirstOrDefault(r => r.CollectRequestId == id && r.AssignedEmployeeId == employee.EmployeeId);

                if (assignment == null)
                {
                    TempData["ErrorMessage"] = "Assignment not found or access denied";
                    return RedirectToAction("MyAssignments");
                }

                if (assignment.RequestStatus != "Assigned")
                {
                    TempData["ErrorMessage"] = "This assignment cannot be collected";
                    return RedirectToAction("AssignmentDetails", new { id });
                }

                var model = new CollectionActivityViewModel
                {
                    CollectRequestId = id,
                    CollectionTime = DateTime.Now,
                    ActualQuantity = assignment.ApproximateQuantity
                };

                ViewBag.Assignment = assignment;

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
                return RedirectToAction("MyAssignments");
            }
        }

        // POST: Employee/RecordCollection
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecordCollection(CollectionActivityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var assignment = db.CollectRequests.Find(model.CollectRequestId);
                ViewBag.Assignment = assignment;
                return View(model);
            }

            try
            {
                int userId = GetCurrentUserId();
                var employee = db.Employees.FirstOrDefault(e => e.UserId == userId);

                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee profile not found";
                    return RedirectToAction("Dashboard");
                }

                var request = db.CollectRequests
                    .FirstOrDefault(r => r.CollectRequestId == model.CollectRequestId &&
                                         r.AssignedEmployeeId == employee.EmployeeId);

                if (request == null)
                {
                    TempData["ErrorMessage"] = "Request not found or access denied";
                    return RedirectToAction("MyAssignments");
                }

                // Create collection activity record
                var collectionActivity = new CollectionActivity
                {
                    CollectRequestId = model.CollectRequestId,
                    EmployeeId = employee.EmployeeId,
                    CollectedDate = model.CollectionTime,
                    CollectionNotes = model.CollectionNotes,
                    ActualQuantity = model.ActualQuantity
                };

                db.CollectionActivities.Add(collectionActivity);

                // Update request status to Collected
                request.RequestStatus = "Collected";

                db.SaveChanges();

                TempData["SuccessMessage"] = "Collection activity recorded successfully! Status updated to Collected.";
                return RedirectToAction("MyAssignments");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error recording collection: " + ex.Message;
                var assignment = db.CollectRequests.Find(model.CollectRequestId);
                ViewBag.Assignment = assignment;
                return View(model);
            }
        }

        // GET: Employee/CollectionHistory/5
        public ActionResult CollectionHistory(int id)
        {
            try
            {
                int userId = GetCurrentUserId();
                var employee = db.Employees.FirstOrDefault(e => e.UserId == userId);

                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee profile not found";
                    return RedirectToAction("Dashboard");
                }

                var assignment = db.CollectRequests
                    .FirstOrDefault(r => r.CollectRequestId == id && r.AssignedEmployeeId == employee.EmployeeId);

                if (assignment == null)
                {
                    TempData["ErrorMessage"] = "Assignment not found";
                    return RedirectToAction("MyAssignments");
                }

                var activities = db.CollectionActivities
                    .Where(a => a.CollectRequestId == id)
                    .OrderByDescending(a => a.CollectedDate)
                    .ToList();

                ViewBag.Assignment = assignment;

                return View(activities);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
                return RedirectToAction("MyAssignments");
            }
        }

        // GET: Employee/RecordDistribution/5
        public ActionResult RecordDistribution(int id)
        {
            try
            {
                int userId = GetCurrentUserId();
                var employee = db.Employees.FirstOrDefault(e => e.UserId == userId);

                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee profile not found";
                    return RedirectToAction("Dashboard");
                }

                // Get the assignment
                var assignment = db.CollectRequests
                    .FirstOrDefault(r => r.CollectRequestId == id && r.AssignedEmployeeId == employee.EmployeeId);

                if (assignment == null)
                {
                    TempData["ErrorMessage"] = "Assignment not found or access denied";
                    return RedirectToAction("MyAssignments");
                }

                if (assignment.RequestStatus != "Collected")
                {
                    TempData["ErrorMessage"] = "This assignment must be collected before distribution";
                    return RedirectToAction("AssignmentDetails", new { id });
                }

                var model = new DistributionActivityViewModel
                {
                    CollectRequestId = id,
                    DistributionTime = DateTime.Now
                };

                ViewBag.Assignment = assignment;

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
                return RedirectToAction("MyAssignments");
            }
        }

        // POST: Employee/RecordDistribution
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecordDistribution(DistributionActivityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var assignment = db.CollectRequests.Find(model.CollectRequestId);
                ViewBag.Assignment = assignment;
                return View(model);
            }

            try
            {
                int userId = GetCurrentUserId();
                var employee = db.Employees.FirstOrDefault(e => e.UserId == userId);

                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee profile not found";
                    return RedirectToAction("Dashboard");
                }

                var request = db.CollectRequests
                    .FirstOrDefault(r => r.CollectRequestId == model.CollectRequestId &&
                                         r.AssignedEmployeeId == employee.EmployeeId);

                if (request == null)
                {
                    TempData["ErrorMessage"] = "Request not found or access denied";
                    return RedirectToAction("MyAssignments");
                }

                // Create distribution activity record
                var distributionActivity = new DistributionActivity
                {
                    CollectRequestId = model.CollectRequestId,
                    EmployeeId = employee.EmployeeId,
                    DistributedDate = model.DistributionTime,
                    DistributionLocation = model.DistributionLocation,
                    NumberOfPeopleServed = model.BeneficiariesServed,
                    DistributionNotes = model.DistributionNotes
                };

                db.DistributionActivities.Add(distributionActivity);

                // Update request status to Completed
                request.RequestStatus = "Completed";

                db.SaveChanges();

                TempData["SuccessMessage"] = "Distribution activity recorded successfully! Status updated to Completed.";
                return RedirectToAction("MyAssignments");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error recording distribution: " + ex.Message;
                var assignment = db.CollectRequests.Find(model.CollectRequestId);
                ViewBag.Assignment = assignment;
                return View(model);
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