# ZeroHunger-Food-Distribution
## **Core Purpose**
A food-waste management system that connects restaurants with an NGO to ensure surplus food reaches deprived individuals instead of being wasted.

---

## Key Workflows

1. **Restaurant submits request** - Creates request with food details and preservation time
2. **Admin assigns employee** - Reviews pending requests and assigns available employee
3. **Employee collects food** - Records collection activity with quantity and notes
4. **Employee distributes food** - Records distribution activity with location and beneficiaries
5. **System tracks everything** - Status updates: Pending → Assigned → Collected → Completed

---

## User Roles

### Restaurant
- Register profile and create collection requests
- View dashboard with request statistics
- Track status of all requests

### NGO Admin
- View system-wide dashboard
- Assign employees to requests
- Manage employee profiles
- Search and filter requests

### Employee
- View assigned tasks
- Record collection activities
- Record distribution activities
- Update request status

---

## Entities

### User
- **Attributes:** UserId, FullName, Email, Password, Phone, UserRole, IsActive
- **Roles:** Admin, Restaurant, Employee

### Restaurant
- **Attributes:** RestaurantId, UserId (FK), RestaurantName, Address, ContactPhone, IsActive
- **Relationship:** 1:M with CollectRequests

### Employee
- **Attributes:** EmployeeId, UserId (FK), EmployeeCode, Address, JoiningDate, IsActive
- **Relationship:** 1:M with CollectRequests, CollectionActivities, DistributionActivities

### CollectRequest
- **Attributes:** CollectRequestId, RestaurantId (FK), FoodDescription, ApproximateQuantity, PreservationTime, RequestStatus, AssignedEmployeeId (FK), RequestedDate, Remarks
- **Status:** Pending → Assigned → Collected → Completed

### CollectionActivity
- **Attributes:** CollectionId, CollectRequestId (FK), EmployeeId (FK), CollectedDate, ActualQuantity, CollectionNotes
- **Created when:** Employee records collection

### DistributionActivity
- **Attributes:** DistributionId, CollectRequestId (FK), EmployeeId (FK), DistributedDate, DistributionLocation, NumberOfPeopleServed, DistributionNotes
- **Created when:** Employee records distribution



### **Relationship Among Entities**
```
- User (1) ←→ (0..1) Restaurant
  - One user can manage at most one restaurant account.
  - Constraint: UserRole must be "Restaurant"

- User (1) ←→ (0..1) Employee  
  - One user can be assigned as one employee profile.
  - Constraint: UserRole must be "Employee"

- Restaurant (1) ←→ (Many) CollectRequest  
  - A single restaurant can create multiple food collection requests.
  - One restaurant has many requests (1:M relationship)

- Employee (0..1) ←→ (Many) CollectRequest  
  - One employee can be assigned to multiple collection requests.
  - A request may have zero or one assigned employee (0..1:M relationship)
  - Foreign Key: AssignedEmployeeId (nullable)

- CollectRequest (1) ←→ (0..1) CollectionActivity  
  - Each collection request can have at most one collection activity record.
  - Created when employee collects food from restaurant (1:0..1)

- CollectRequest (1) ←→ (0..1) DistributionActivity  
  - Each collection request can have at most one distribution activity record.
  - Created when employee distributes collected food to beneficiaries (1:0..1)

- Employee (1) ←→ (Many) CollectionActivity  
  - One employee can record multiple collection activities.
  - Each collection activity is performed by exactly one employee (1:M)

- Employee (1) ←→ (Many) DistributionActivity  
  - One employee can record multiple distribution activities.
  - Each distribution activity is performed by exactly one employee (1:M)

```

## 5. DATABASE DIAGRAM (Relationships)
                                ┌─────────────────┐
                                │     Users       │
                                │─────────────────│
                                │ UserId (PK)     │◄────┐
                                │ FullName        │     │
                                │ Email (Unique)  │     │
                                │ Password (Hash) │     │
                                │ Phone           │     │
                                │ UserRole        │     │
                                │ IsActive        │     │
                                └─────────────────┘     │
                                                        │
                                        ┌───────────────┴─────────────┐
                                        │                             │
                                ┌───────┴────────┐            ┌───────┴────────┐
                                │  Restaurants   │            │   Employees    │
                                │────────────────│            │────────────────│
                                │RestaurantId(PK)│            │EmployeeId (PK) │◄──┐
                                │UserId (FK)     │            │UserId (FK)     │   │
                                │RestaurantName  │            │EmployeeCode    │   │
                                │Address         │            │Address         │   │
                                │ContactPhone    │            │JoiningDate     │   │
                                │IsActive        │            │IsActive        │   │
                                └────────┬───────┘            └────────┬───────┘   │
                                         │1                            │           │
                                         │                            *│           │
                                         │                             │           │
                                  ┌──────▼─────────────────────────────▼───────┐   │
                                  │         CollectRequests                    │   │
                                  │────────────────────────────────────────────│   │
                                  │CollectRequestId (PK)                       │   │
                                  │RestaurantId (FK)                           │   │
                                  │FoodDescription                             │   │
                                  │ApproximateQuantity                         │   │
                                  │PreservationTime                            │   │
                                  │RequestStatus (Pending/Assigned/Collected/  │   │
                                  │              Completed)                    │   │
                                  │AssignedEmployeeId (FK)─────────────────────┼───┘
                                  │RequestedDate                               │
                                  │Remarks                                     │
                                  └──────────────┬─────────────────────────────┘
                                                 │1
                                                 │
                                         ┌───────┴────────┐
                                         │1              1│
                                  ┌──────▼──────┐  ┌──────▼─────────────┐
                                  │Collection   │  │Distribution        │
                                  │Activities   │  │Activities          │
                                  │─────────────│  │────────────────────│
                                  │CollectionId │  │DistributionId (PK) │
                                  │(PK)         │  │CollectRequestId(FK)│
                                  │CollectReque │  │EmployeeId (FK)     │
                                  │stId (FK)    │  │DistributedDate     │
                                  │EmployeeId   │  │DistributionLocation│
                                  │(FK)         │  │NumberOfPeopleServed│
                                  │CollectedDate│  │DistributionNotes   │
                                  │ActualQuantit│  └────────────────────┘
                                  │y            │
                                  │CollectionNot│
                                  │es           │
                                  └─────────────┘
   

### **Project Structure**

```
ZeroHunger-Food-Distribution/
│
├── Controllers/
│   ├── AccountController.cs
│   ├── BaseController.cs
│   ├── RestaurantController.cs
│   ├── AdminController.cs
│   └── EmployeeController.cs
│
├── Models/
│   ├── User.cs
│   ├── Restaurant.cs
│   ├── Employee.cs
│   ├── CollectRequest.cs
│   ├── CollectionActivity.cs
│   ├── DistributionActivity.cs
│   └── ZeroHungerEntities.cs
│
├── ViewModels/
│   ├── LoginViewModel.cs
│   ├── RegisterViewModel.cs
│   ├── CollectRequestViewModel.cs
│   ├── CollectionActivityViewModel.cs
│   └── DistributionActivityViewModel.cs
│
├── Helpers/
│   ├── SessionHelper.cs
│   ├── UserRoles.cs
│   └── CustomAuthorizeAttribute.cs
│
└── Views/
    ├── Shared/
    │   └── _Layout.cshtml
    ├── Account/
    │   ├── Login.cshtml
    │   └── Register.cshtml
    ├── Restaurant/
    │   ├── Dashboard.cshtml
    │   ├── CreateRequest.cshtml
    │   ├── MyRequests.cshtml
    │   └── RequestDetails.cshtml
    ├── Admin/
    │   ├── Dashboard.cshtml
    │   ├── AllRequests.cshtml
    │   ├── RequestDetails.cshtml
    │   ├── AssignEmployee.cshtml
    │   ├── Employees.cshtml
    │   └── EmployeeDetails.cshtml
    └── Employee/
        ├── Dashboard.cshtml
        ├── MyAssignments.cshtml
        ├── AssignmentDetails.cshtml
        ├── RecordCollection.cshtml
        └── RecordDistribution.cshtml

```
---
