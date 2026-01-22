using AutoMapper;
using STFMS.API.DTOs.Analytics;
using STFMS.API.DTOs.Booking;
using STFMS.API.DTOs.Driver;
using STFMS.API.DTOs.Feedback;
using STFMS.API.DTOs.Maintenance;
using STFMS.API.DTOs.Payment;
using STFMS.API.DTOs.User;
using STFMS.API.DTOs.Vehicle;
using STFMS.DAL.Entities;

namespace STFMS.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // USER MAPPINGS
            CreateMap<User, UserResponseDTO>();
            CreateMap<CreateUserRequest, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.RegisteredDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.Driver, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore());

            CreateMap<UpdateUserRequest, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.UserType, opt => opt.Ignore())
                .ForMember(dest => dest.RegisteredDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.Driver, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore());

            CreateMap<User, UserWithDetailsResponseDTO>()
                .IncludeBase<User, UserResponseDTO>()
                .ForMember(dest => dest.Bookings, opt => opt.MapFrom(src => src.Bookings));

            // DRIVER MAPPINGS
            CreateMap<Driver, DriverResponseDTO>()
    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CreateDriverRequest, Driver>()
                .ForMember(dest => dest.DriverId, opt => opt.Ignore())
                .ForMember(dest => dest.Rating, opt => opt.Ignore())
                .ForMember(dest => dest.TotalRides, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.JoinedDate, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Vehicles, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore());

            CreateMap<UpdateDriverRequest, Driver>()
                .ForMember(dest => dest.DriverId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Rating, opt => opt.Ignore())
                .ForMember(dest => dest.TotalRides, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.JoinedDate, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Vehicles, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore());

            CreateMap<Driver, DriverWithDetailsResponseDTO>()
                .IncludeBase<Driver, DriverResponseDTO>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Vehicles, opt => opt.MapFrom(src => src.Vehicles))
                .ForMember(dest => dest.TotalRevenue, opt => opt.Ignore())
                .ForMember(dest => dest.AverageRating, opt => opt.Ignore());

            // VEHICLE MAPPINGS
            CreateMap<Vehicle, VehicleResponseDTO>();
            CreateMap<CreateVehicleRequest, Vehicle>()
                .ForMember(dest => dest.VehicleId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.LastServiceDate, opt => opt.Ignore())
                .ForMember(dest => dest.Driver, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.MaintenanceRecords, opt => opt.Ignore());

            CreateMap<UpdateVehicleRequest, Vehicle>()
                .ForMember(dest => dest.VehicleId, opt => opt.Ignore())
                .ForMember(dest => dest.DriverId, opt => opt.Ignore())
                .ForMember(dest => dest.LastServiceDate, opt => opt.Ignore())
                .ForMember(dest => dest.Driver, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.MaintenanceRecords, opt => opt.Ignore());

            CreateMap<Vehicle, VehicleWithDetailsResponseDTO>()
                .IncludeBase<Vehicle, VehicleResponseDTO>();

            // BOOKING MAPPINGS
            CreateMap<Booking, BookingResponseDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src => src.ServiceType.ToString()));

            CreateMap<CreateBookingRequest, Booking>()
                .ForMember(dest => dest.BookingId, opt => opt.Ignore())
                .ForMember(dest => dest.DriverId, opt => opt.Ignore())
                .ForMember(dest => dest.VehicleId, opt => opt.Ignore())
                .ForMember(dest => dest.BookingTime, opt => opt.Ignore())
                .ForMember(dest => dest.PickupTime, opt => opt.Ignore())
                .ForMember(dest => dest.CompletionTime, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.EstimatedFare, opt => opt.Ignore())
                .ForMember(dest => dest.ActualFare, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Driver, opt => opt.Ignore())
                .ForMember(dest => dest.Vehicle, opt => opt.Ignore())
                .ForMember(dest => dest.Payment, opt => opt.Ignore())
                .ForMember(dest => dest.Feedback, opt => opt.Ignore());

            CreateMap<Booking, BookingWithDetailsResponseDTO>()
                .IncludeBase<Booking, BookingResponseDTO>();

            // PAYMENT MAPPINGS
            CreateMap<Payment, PaymentResponseDTO>();
            CreateMap<ProcessPaymentRequest, Payment>()
                .ForMember(dest => dest.PaymentId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentDate, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionId, opt => opt.Ignore())
                .ForMember(dest => dest.Booking, opt => opt.Ignore());

            // FEEDBACK MAPPINGS
            CreateMap<Feedback, FeedbackResponseDTO>();
            CreateMap<CreateFeedbackRequest, Feedback>()
                .ForMember(dest => dest.FeedbackId, opt => opt.Ignore())
                .ForMember(dest => dest.SubmittedDate, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Booking, opt => opt.Ignore());

            CreateMap<UpdateFeedbackRequest, Feedback>()
                .ForMember(dest => dest.FeedbackId, opt => opt.Ignore())
                .ForMember(dest => dest.BookingId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.SubmittedDate, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Booking, opt => opt.Ignore());

            // MAINTENANCE MAPPINGS
            CreateMap<Maintenance, MaintenanceResponseDTO>();
            CreateMap<CreateMaintenanceRequest, Maintenance>()
                .ForMember(dest => dest.MaintenanceId, opt => opt.Ignore())
                .ForMember(dest => dest.CompletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Vehicle, opt => opt.Ignore());

            CreateMap<UpdateMaintenanceRequest, Maintenance>()
                .ForMember(dest => dest.MaintenanceId, opt => opt.Ignore())
                .ForMember(dest => dest.VehicleId, opt => opt.Ignore())
                .ForMember(dest => dest.CompletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Vehicle, opt => opt.Ignore());

            // ANALYTICS MAPPINGS (Tuple to DTO)
            // Dashboard Stats (10-item tuple to DTO)
            CreateMap<(int, int, int, int, decimal, int, int, int, int, decimal), DashboardStatsResponseDTO>()
                .ForMember(dest => dest.TotalBookings, opt => opt.MapFrom(src => src.Item1))
                .ForMember(dest => dest.CompletedRides, opt => opt.MapFrom(src => src.Item2))
                .ForMember(dest => dest.ActiveBookings, opt => opt.MapFrom(src => src.Item3))
                .ForMember(dest => dest.PendingBookings, opt => opt.MapFrom(src => src.Item4))
                .ForMember(dest => dest.TotalRevenue, opt => opt.MapFrom(src => src.Item5))
                .ForMember(dest => dest.TotalDrivers, opt => opt.MapFrom(src => src.Item6))
                .ForMember(dest => dest.ActiveDrivers, opt => opt.MapFrom(src => src.Item7))
                .ForMember(dest => dest.TotalVehicles, opt => opt.MapFrom(src => src.Item8))
                .ForMember(dest => dest.ActiveVehicles, opt => opt.MapFrom(src => src.Item9))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Item10));

            // Revenue Report (3-item tuple to DTO)
            CreateMap<(DateTime, decimal, int), RevenueReportResponseDTO>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Item1))
                .ForMember(dest => dest.Revenue, opt => opt.MapFrom(src => src.Item2))
                .ForMember(dest => dest.BookingCount, opt => opt.MapFrom(src => src.Item3));

            // Popular Route (4-item tuple to DTO)
            CreateMap<(string, string, int, decimal), PopularRouteResponseDTO>()
                .ForMember(dest => dest.PickupLocation, opt => opt.MapFrom(src => src.Item1))
                .ForMember(dest => dest.DropoffLocation, opt => opt.MapFrom(src => src.Item2))
                .ForMember(dest => dest.TripCount, opt => opt.MapFrom(src => src.Item3))
                .ForMember(dest => dest.AverageFare, opt => opt.MapFrom(src => src.Item4));
        }
    }
}