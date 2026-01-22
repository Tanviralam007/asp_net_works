using AutoMapper;
using ToolShare.DAL.Entities;
using ToolShare.API.DTOs.User;
using ToolShare.API.DTOs.Category;
using ToolShare.API.DTOs.Tool;
using ToolShare.API.DTOs.BorrowRequest;
using ToolShare.API.DTOs.Payment;
using ToolShare.API.DTOs.Transaction;
using ToolShare.API.DTOs.Review;

namespace ToolShare.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // user mapping
            CreateMap<User, UserResponseDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            CreateMap<CreateUserRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsBlocked, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (UserRole)src.Role));  // Cast byte to UserRole enum

            CreateMap<UpdateUserRequest, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsBlocked, opt => opt.Ignore());

            // catagory mapping
            CreateMap<ToolCategory, CategoryResponseDTO>();

            CreateMap<CreateCategoryRequest, ToolCategory>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            CreateMap<UpdateCategoryRequest, ToolCategory>();

            // tool mapping
            CreateMap<Tool, ToolResponseDTO>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner != null ? src.Owner.Name : string.Empty))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : string.Empty));

            CreateMap<CreateToolRequest, Tool>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
                .ForMember(dest => dest.IsAvailable, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Owner, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.BorrowRequests, opt => opt.Ignore())
                .ForMember(dest => dest.Reviews, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<UpdateToolRequest, Tool>()
                .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Owner, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.BorrowRequests, opt => opt.Ignore())
                .ForMember(dest => dest.Reviews, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            // borrow request mapping
            CreateMap<BorrowRequest, BorrowRequestResponseDTO>()
                .ForMember(dest => dest.ToolName, opt => opt.MapFrom(src => src.Tool != null ? src.Tool.ToolName : string.Empty))
                .ForMember(dest => dest.BorrowerName, opt => opt.MapFrom(src => src.Borrower != null ? src.Borrower.Name : string.Empty))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.EstimatedCost, opt => opt.MapFrom(src =>
                    src.Tool != null ? src.Tool.DailyRate * (src.EndDate - src.StartDate).Days : 0));

            CreateMap<CreateBorrowRequestRequest, BorrowRequest>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BorrowerId, opt => opt.Ignore())
                .ForMember(dest => dest.RequestDate, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovalDate, opt => opt.Ignore())
                .ForMember(dest => dest.Tool, opt => opt.Ignore())
                .ForMember(dest => dest.Borrower, opt => opt.Ignore())
                .ForMember(dest => dest.Payment, opt => opt.Ignore())
                .ForMember(dest => dest.Transaction, opt => opt.Ignore());

            // payment mapping
            CreateMap<Payment, PaymentResponseDTO>()
    .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()));

            CreateMap<CreatePaymentRequest, Payment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentDate, opt => opt.Ignore())
                .ForMember(dest => dest.BorrowRequest, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => (PaymentMethod)src.PaymentMethod)); // Cast byte to PaymentMethod enum

            // Transaction mapping
            CreateMap<ToolTransaction, TransactionResponseDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.ToolName, opt => opt.MapFrom(src =>
                    src.BorrowRequest != null && src.BorrowRequest.Tool != null ? src.BorrowRequest.Tool.ToolName : string.Empty))
                .ForMember(dest => dest.BorrowerName, opt => opt.MapFrom(src =>
                    src.BorrowRequest != null && src.BorrowRequest.Borrower != null ? src.BorrowRequest.Borrower.Name : string.Empty));

            // review mapping
            CreateMap<Review, ReviewResponseDTO>()
                .ForMember(dest => dest.ToolName, opt => opt.MapFrom(src => src.Tool != null ? src.Tool.ToolName : string.Empty))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : string.Empty))
                .ForMember(dest => dest.ReviewType, opt => opt.MapFrom(src => src.ReviewType.ToString()));

            CreateMap<CreateReviewRequest, Review>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewDate, opt => opt.Ignore())
                .ForMember(dest => dest.Tool, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewType, opt => opt.MapFrom(src => (ReviewType)src.ReviewType)); // Cast byte to ReviewType enum
        }
    }
}
