using STFMS.DAL.Entities;

namespace STFMS.DAL.Data
{
    public static class DataSeeder
    {
        public static void SeedData(AppDbContext context)
        {
            if (!context.Cities.Any())
            {
                var cities = new List<City>
                {
                    new City
                    {
                        CityName = "London",
                        IsActive = true,
                        BaseFare = 5.00m,
                        PerKmRate = 1.50m
                    },
                    new City
                    {
                        CityName = "Manchester",
                        IsActive = true,
                        BaseFare = 4.50m,
                        PerKmRate = 1.30m
                    },
                    new City
                    {
                        CityName = "Birmingham",
                        IsActive = true,
                        BaseFare = 4.00m,
                        PerKmRate = 1.20m
                    },
                    new City
                    {
                        CityName = "Glasgow",
                        IsActive = true,
                        BaseFare = 4.00m,
                        PerKmRate = 1.25m
                    }
                };

                context.Cities.AddRange(cities);
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        FullName = "System Administrator",
                        Email = "admin@superrides.com",
                        PhoneNumber = "07700900000",
                        PasswordHash = "Admin@123",
                        UserType = UserType.Admin,
                        RegisteredDate = DateTime.UtcNow.AddMonths(-12),
                        IsActive = true
                    },
                    new User
                    {
                        FullName = "John Smith",
                        Email = "john.smith@example.com",
                        PhoneNumber = "07700900001",
                        PasswordHash = "Customer@123",
                        UserType = UserType.Customer,
                        RegisteredDate = DateTime.UtcNow.AddMonths(-6),
                        IsActive = true
                    },
                    new User
                    {
                        FullName = "Emily Davis",
                        Email = "emily.davis@example.com",
                        PhoneNumber = "07700900002",
                        PasswordHash = "Customer@123",
                        UserType = UserType.Customer,
                        RegisteredDate = DateTime.UtcNow.AddMonths(-4),
                        IsActive = true
                    },
                    new User
                    {
                        FullName = "Michael Brown",
                        Email = "michael.brown@example.com",
                        PhoneNumber = "07700900003",
                        PasswordHash = "Customer@123",
                        UserType = UserType.Customer,
                        RegisteredDate = DateTime.UtcNow.AddMonths(-3),
                        IsActive = true
                    },
                    new User
                    {
                        FullName = "James Wilson",
                        Email = "james.wilson@superrides.com",
                        PhoneNumber = "07700900010",
                        PasswordHash = "Driver@123",
                        UserType = UserType.Driver,
                        RegisteredDate = DateTime.UtcNow.AddMonths(-10),
                        IsActive = true
                    },
                    new User
                    {
                        FullName = "Sarah Johnson",
                        Email = "sarah.johnson@superrides.com",
                        PhoneNumber = "07700900011",
                        PasswordHash = "Driver@123",
                        UserType = UserType.Driver,
                        RegisteredDate = DateTime.UtcNow.AddMonths(-8),
                        IsActive = true
                    },
                    new User
                    {
                        FullName = "David Martinez",
                        Email = "david.martinez@superrides.com",
                        PhoneNumber = "07700900012",
                        PasswordHash = "Driver@123",
                        UserType = UserType.Driver,
                        RegisteredDate = DateTime.UtcNow.AddMonths(-5),
                        IsActive = true
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }

            if (!context.Drivers.Any())
            {
                var driverUser1 = context.Users.FirstOrDefault(u => u.Email == "james.wilson@superrides.com");
                var driverUser2 = context.Users.FirstOrDefault(u => u.Email == "sarah.johnson@superrides.com");
                var driverUser3 = context.Users.FirstOrDefault(u => u.Email == "david.martinez@superrides.com");

                if (driverUser1 != null && driverUser2 != null && driverUser3 != null)
                {
                    var drivers = new List<Driver>
                    {
                        new Driver
                        {
                            UserId = driverUser1.UserId,
                            LicenseNumber = "UK123456789",
                            Rating = 4.75m,
                            TotalRides = 250,
                            Status = DriverStatus.Available,
                            JoinedDate = DateTime.UtcNow.AddMonths(-10)
                        },
                        new Driver
                        {
                            UserId = driverUser2.UserId,
                            LicenseNumber = "UK987654321",
                            Rating = 4.90m,
                            TotalRides = 320,
                            Status = DriverStatus.Available,
                            JoinedDate = DateTime.UtcNow.AddMonths(-8)
                        },
                        new Driver
                        {
                            UserId = driverUser3.UserId,
                            LicenseNumber = "UK555666777",
                            Rating = 4.60m,
                            TotalRides = 180,
                            Status = DriverStatus.Busy,
                            JoinedDate = DateTime.UtcNow.AddMonths(-5)
                        }
                    };

                    context.Drivers.AddRange(drivers);
                    context.SaveChanges();
                }
            }

            if (!context.Vehicles.Any())
            {
                var driver1 = context.Drivers.FirstOrDefault(d => d.LicenseNumber == "UK123456789");
                var driver2 = context.Drivers.FirstOrDefault(d => d.LicenseNumber == "UK987654321");
                var driver3 = context.Drivers.FirstOrDefault(d => d.LicenseNumber == "UK555666777");

                if (driver1 != null && driver2 != null && driver3 != null)
                {
                    var vehicles = new List<Vehicle>
                    {
                        new Vehicle
                        {
                            DriverId = driver1.DriverId,
                            RegistrationNumber = "AB12 CDE",
                            Model = "Toyota Prius",
                            VehicleType = VehicleType.Sedan,
                            Capacity = 4,
                            Status = VehicleStatus.Active,
                            LastServiceDate = DateTime.UtcNow.AddDays(-45)
                        },
                        new Vehicle
                        {
                            DriverId = driver2.DriverId,
                            RegistrationNumber = "XY98 ZWQ",
                            Model = "Honda CR-V",
                            VehicleType = VehicleType.SUV,
                            Capacity = 5,
                            Status = VehicleStatus.Active,
                            LastServiceDate = DateTime.UtcNow.AddDays(-20)
                        },
                        new Vehicle
                        {
                            DriverId = driver3.DriverId,
                            RegistrationNumber = "LM45 NOP",
                            Model = "Mercedes-Benz E-Class",
                            VehicleType = VehicleType.Luxury,
                            Capacity = 4,
                            Status = VehicleStatus.Active,
                            LastServiceDate = DateTime.UtcNow.AddDays(-60)
                        },
                        new Vehicle
                        {
                            DriverId = driver1.DriverId,
                            RegistrationNumber = "FG67 HIJ",
                            Model = "Ford Transit",
                            VehicleType = VehicleType.Van,
                            Capacity = 8,
                            Status = VehicleStatus.Maintenance,
                            LastServiceDate = DateTime.UtcNow.AddDays(-95)
                        }
                    };

                    context.Vehicles.AddRange(vehicles);
                    context.SaveChanges();
                }
            }

            if (!context.Bookings.Any())
            {
                var customer1 = context.Users.FirstOrDefault(u => u.Email == "john.smith@example.com");
                var customer2 = context.Users.FirstOrDefault(u => u.Email == "emily.davis@example.com");
                var customer3 = context.Users.FirstOrDefault(u => u.Email == "michael.brown@example.com");

                var driver1 = context.Drivers.FirstOrDefault(d => d.LicenseNumber == "UK123456789");
                var driver2 = context.Drivers.FirstOrDefault(d => d.LicenseNumber == "UK987654321");
                var driver3 = context.Drivers.FirstOrDefault(d => d.LicenseNumber == "UK555666777");

                var vehicle1 = context.Vehicles.FirstOrDefault(v => v.RegistrationNumber == "AB12 CDE");
                var vehicle2 = context.Vehicles.FirstOrDefault(v => v.RegistrationNumber == "XY98 ZWQ");
                var vehicle3 = context.Vehicles.FirstOrDefault(v => v.RegistrationNumber == "LM45 NOP");

                if (customer1 != null && customer2 != null && customer3 != null &&
                    driver1 != null && driver2 != null && driver3 != null &&
                    vehicle1 != null && vehicle2 != null && vehicle3 != null)
                {
                    var bookings = new List<Booking>
                    {
                        new Booking
                        {
                            UserId = customer1.UserId,
                            DriverId = driver1.DriverId,
                            VehicleId = vehicle1.VehicleId,
                            PickupLocation = "London Heathrow Airport",
                            DropoffLocation = "Central London, Westminster",
                            BookingTime = DateTime.UtcNow.AddDays(-10),
                            PickupTime = DateTime.UtcNow.AddDays(-10).AddMinutes(15),
                            CompletionTime = DateTime.UtcNow.AddDays(-10).AddMinutes(55),
                            Status = BookingStatus.Completed,
                            EstimatedFare = 45.00m,
                            ActualFare = 47.50m,
                            ServiceType = ServiceType.Ride
                        },
                        new Booking
                        {
                            UserId = customer2.UserId,
                            DriverId = driver2.DriverId,
                            VehicleId = vehicle2.VehicleId,
                            PickupLocation = "Manchester Piccadilly Station",
                            DropoffLocation = "Manchester Airport",
                            BookingTime = DateTime.UtcNow.AddDays(-7),
                            PickupTime = DateTime.UtcNow.AddDays(-7).AddMinutes(10),
                            CompletionTime = DateTime.UtcNow.AddDays(-7).AddMinutes(35),
                            Status = BookingStatus.Completed,
                            EstimatedFare = 30.00m,
                            ActualFare = 32.00m,
                            ServiceType = ServiceType.Corporate
                        },
                        new Booking
                        {
                            UserId = customer3.UserId,
                            DriverId = driver1.DriverId,
                            VehicleId = vehicle1.VehicleId,
                            PickupLocation = "Birmingham New Street",
                            DropoffLocation = "Birmingham Business Park",
                            BookingTime = DateTime.UtcNow.AddDays(-5),
                            PickupTime = DateTime.UtcNow.AddDays(-5).AddMinutes(8),
                            CompletionTime = DateTime.UtcNow.AddDays(-5).AddMinutes(28),
                            Status = BookingStatus.Completed,
                            EstimatedFare = 25.00m,
                            ActualFare = 25.00m,
                            ServiceType = ServiceType.Ride
                        },
                        new Booking
                        {
                            UserId = customer1.UserId,
                            DriverId = driver3.DriverId,
                            VehicleId = vehicle3.VehicleId,
                            PickupLocation = "Glasgow Central Station",
                            DropoffLocation = "Glasgow Airport",
                            BookingTime = DateTime.UtcNow.AddHours(-1),
                            PickupTime = DateTime.UtcNow.AddMinutes(-45),
                            CompletionTime = null,
                            Status = BookingStatus.InProgress,
                            EstimatedFare = 28.00m,
                            ActualFare = null,
                            ServiceType = ServiceType.Ride
                        },
                        new Booking
                        {
                            UserId = customer2.UserId,
                            DriverId = driver2.DriverId,
                            VehicleId = vehicle2.VehicleId,
                            PickupLocation = "London King's Cross",
                            DropoffLocation = "London Bridge",
                            BookingTime = DateTime.UtcNow.AddMinutes(-30),
                            PickupTime = null,
                            CompletionTime = null,
                            Status = BookingStatus.Assigned,
                            EstimatedFare = 18.00m,
                            ActualFare = null,
                            ServiceType = ServiceType.Ride
                        },
                        new Booking
                        {
                            UserId = customer3.UserId,
                            DriverId = null,
                            VehicleId = null,
                            PickupLocation = "Manchester Arndale Centre",
                            DropoffLocation = "Manchester Trafford Centre",
                            BookingTime = DateTime.UtcNow.AddMinutes(-15),
                            PickupTime = null,
                            CompletionTime = null,
                            Status = BookingStatus.Pending,
                            EstimatedFare = 22.00m,
                            ActualFare = null,
                            ServiceType = ServiceType.Ride
                        },
                        new Booking
                        {
                            UserId = customer1.UserId,
                            DriverId = null,
                            VehicleId = null,
                            PickupLocation = "Birmingham Airport",
                            DropoffLocation = "Birmingham City Centre",
                            BookingTime = DateTime.UtcNow.AddDays(-3),
                            PickupTime = null,
                            CompletionTime = null,
                            Status = BookingStatus.Cancelled,
                            EstimatedFare = 35.00m,
                            ActualFare = null,
                            ServiceType = ServiceType.Ride
                        },
                        new Booking
                        {
                            UserId = customer2.UserId,
                            DriverId = driver1.DriverId,
                            VehicleId = vehicle1.VehicleId,
                            PickupLocation = "London Warehouse District",
                            DropoffLocation = "London Residential Area",
                            BookingTime = DateTime.UtcNow.AddDays(-2),
                            PickupTime = DateTime.UtcNow.AddDays(-2).AddMinutes(10),
                            CompletionTime = DateTime.UtcNow.AddDays(-2).AddMinutes(40),
                            Status = BookingStatus.Completed,
                            EstimatedFare = 15.00m,
                            ActualFare = 15.00m,
                            ServiceType = ServiceType.Parcel
                        }
                    };

                    context.Bookings.AddRange(bookings);
                    context.SaveChanges();
                }
            }

            if (!context.Payments.Any())
            {
                var completedBookings = context.Bookings
                    .Where(b => b.Status == BookingStatus.Completed)
                    .ToList();

                var payments = new List<Payment>();

                foreach (var booking in completedBookings)
                {
                    payments.Add(new Payment
                    {
                        BookingId = booking.BookingId,
                        Amount = booking.ActualFare ?? booking.EstimatedFare,
                        PaymentMethod = booking.BookingId % 3 == 0 ? PaymentMethod.Cash :
                                       booking.BookingId % 2 == 0 ? PaymentMethod.Card :
                                       PaymentMethod.Wallet,
                        Status = PaymentStatus.Completed,
                        PaymentDate = booking.CompletionTime ?? DateTime.UtcNow,
                        TransactionId = $"TXN{booking.BookingId:D6}{DateTime.UtcNow.Ticks}"
                    });
                }

                context.Payments.AddRange(payments);
                context.SaveChanges();
            }

            if (!context.Feedbacks.Any())
            {
                var completedBookings = context.Bookings
                    .Where(b => b.Status == BookingStatus.Completed)
                    .ToList();

                var feedbacks = new List<Feedback>();

                if (completedBookings.Count >= 1)
                {
                    feedbacks.Add(new Feedback
                    {
                        BookingId = completedBookings[0].BookingId,
                        UserId = completedBookings[0].UserId,
                        Rating = 5,
                        Comments = "Excellent service! Driver was very professional and arrived on time.",
                        SubmittedDate = completedBookings[0].CompletionTime!.Value.AddMinutes(30)
                    });
                }

                if (completedBookings.Count >= 2)
                {
                    feedbacks.Add(new Feedback
                    {
                        BookingId = completedBookings[1].BookingId,
                        UserId = completedBookings[1].UserId,
                        Rating = 4,
                        Comments = "Good experience overall. Vehicle was clean and comfortable.",
                        SubmittedDate = completedBookings[1].CompletionTime!.Value.AddHours(2)
                    });
                }

                if (completedBookings.Count >= 3)
                {
                    feedbacks.Add(new Feedback
                    {
                        BookingId = completedBookings[2].BookingId,
                        UserId = completedBookings[2].UserId,
                        Rating = 5,
                        Comments = "Perfect ride! Highly recommend this driver.",
                        SubmittedDate = completedBookings[2].CompletionTime!.Value.AddMinutes(45)
                    });
                }

                if (completedBookings.Count >= 4)
                {
                    feedbacks.Add(new Feedback
                    {
                        BookingId = completedBookings[3].BookingId,
                        UserId = completedBookings[3].UserId,
                        Rating = 3,
                        Comments = "Average service. Driver took a longer route.",
                        SubmittedDate = completedBookings[3].CompletionTime!.Value.AddHours(1)
                    });
                }

                context.Feedbacks.AddRange(feedbacks);
                context.SaveChanges();
            }

            if (!context.Maintenances.Any())
            {
                var vehicle1 = context.Vehicles.FirstOrDefault(v => v.RegistrationNumber == "AB12 CDE");
                var vehicle2 = context.Vehicles.FirstOrDefault(v => v.RegistrationNumber == "XY98 ZWQ");
                var vehicle3 = context.Vehicles.FirstOrDefault(v => v.RegistrationNumber == "LM45 NOP");
                var vehicle4 = context.Vehicles.FirstOrDefault(v => v.RegistrationNumber == "FG67 HIJ");

                if (vehicle1 != null && vehicle2 != null && vehicle3 != null && vehicle4 != null)
                {
                    var maintenanceRecords = new List<Maintenance>
                    {
                        new Maintenance
                        {
                            VehicleId = vehicle1.VehicleId,
                            MaintenanceType = MaintenanceType.Service,
                            Description = "Regular 10,000 mile service - oil change, filter replacement",
                            Cost = 150.00m,
                            ScheduledDate = DateTime.UtcNow.AddDays(-45),
                            CompletedDate = DateTime.UtcNow.AddDays(-45),
                            Status = MaintenanceStatus.Completed
                        },
                        new Maintenance
                        {
                            VehicleId = vehicle2.VehicleId,
                            MaintenanceType = MaintenanceType.Inspection,
                            Description = "Annual MOT inspection and brake check",
                            Cost = 80.00m,
                            ScheduledDate = DateTime.UtcNow.AddDays(-20),
                            CompletedDate = DateTime.UtcNow.AddDays(-20),
                            Status = MaintenanceStatus.Completed
                        },
                        new Maintenance
                        {
                            VehicleId = vehicle3.VehicleId,
                            MaintenanceType = MaintenanceType.Repair,
                            Description = "Replace worn brake pads and discs",
                            Cost = 320.00m,
                            ScheduledDate = DateTime.UtcNow.AddDays(-60),
                            CompletedDate = DateTime.UtcNow.AddDays(-58),
                            Status = MaintenanceStatus.Completed
                        },
                        new Maintenance
                        {
                            VehicleId = vehicle4.VehicleId,
                            MaintenanceType = MaintenanceType.Repair,
                            Description = "Engine diagnostic and repair - check engine light",
                            Cost = 450.00m,
                            ScheduledDate = DateTime.UtcNow.AddDays(-2),
                            CompletedDate = null,
                            Status = MaintenanceStatus.InProgress
                        },
                        new Maintenance
                        {
                            VehicleId = vehicle1.VehicleId,
                            MaintenanceType = MaintenanceType.Service,
                            Description = "Tire rotation and alignment check",
                            Cost = 100.00m,
                            ScheduledDate = DateTime.UtcNow.AddDays(15),
                            CompletedDate = null,
                            Status = MaintenanceStatus.Scheduled
                        },
                        new Maintenance
                        {
                            VehicleId = vehicle2.VehicleId,
                            MaintenanceType = MaintenanceType.Service,
                            Description = "Air conditioning service and coolant top-up",
                            Cost = 120.00m,
                            ScheduledDate = DateTime.UtcNow.AddDays(30),
                            CompletedDate = null,
                            Status = MaintenanceStatus.Scheduled
                        }
                    };

                    context.Maintenances.AddRange(maintenanceRecords);
                    context.SaveChanges();
                }
            }
        }
    }
}
