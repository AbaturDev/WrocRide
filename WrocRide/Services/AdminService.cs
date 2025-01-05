using Microsoft.AspNetCore.Mvc;
using WrocRide.Entities;
using WrocRide.Helpers;
using WrocRide.Models;
using WrocRide.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WrocRide.Services
{
    public interface IAdminService
    {
        PagedList<DocumentDto> GetDocuments(DocumentQuery query);
        void UpdateDocument(int id, UpdateDocumentDto dto);
        DocumentDto GetDocumentByDriverId(int id);
        PagedList<UserDto> GetAll(UserQuery query);
        void UpdateUser(int id, UpdateUserDto dto);
    }
    public class AdminService : IAdminService
    {
        private readonly WrocRideDbContext _dbContext;
        private readonly IUserContextService _userContextService;
        private readonly IDriverService _driverService;
        private readonly IPasswordHasher<User> _passwordHasher;
        public AdminService(WrocRideDbContext dbContext, IUserContextService userContextService, IDriverService driverService, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _userContextService = userContextService;
            _driverService = driverService;
            _passwordHasher = passwordHasher;
        }

        public PagedList<DocumentDto> GetDocuments(DocumentQuery query)
        {
            IQueryable<Document> baseQuery = _dbContext.Documents;

            if (query.DocumentStatus != null)
            {
                baseQuery = baseQuery.Where(d => d.DocumentStatus == query.DocumentStatus);
            }

            var documents = baseQuery
                .Select(d => new DocumentDto()
                {
                    Id = d.Id,
                    DocumentStatus = d.DocumentStatus,
                    FileLocation = d.FileLocation,
                    RequestDate = d.RequestDate
                })
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var result = new PagedList<DocumentDto>(documents, query.PageSize, query.PageNumber, documents.Count);

            return result;
        }

        public void UpdateDocument(int id, UpdateDocumentDto dto)
        {
            var userId = _userContextService.GetUserId;
            var admin = _dbContext.Admins.FirstOrDefault(u => u.UserId == userId);

            if (admin == null)
            {
                throw new NotFoundException("Admin not found");
            }

            using var dbContextTransaction = _dbContext.Database.BeginTransaction();
            try
            {
                var document = _dbContext.Documents.FirstOrDefault(d => d.Id == id);

                if (document == null)
                {
                    throw new NotFoundException("Document not found");
                }

                document.DocumentStatus = dto.DocumentStatus;
                document.ExaminationDate = DateTime.UtcNow;
                document.AdminId = admin.Id;

                if (dto.DocumentStatus == Models.Enums.DocumentStatus.Accepted)
                {
                    var status = new UpdateDriverStatusDto
                    {
                        DriverStatus = Models.Enums.DriverStatus.Offline
                    };
                    var driver = _dbContext.Drivers.FirstOrDefault(d => d.DocumentId == id);

                    if (driver == null)
                    {
                        throw new NotFoundException("Driver not found");
                    }

                    var driverId = driver.Id;
                    _driverService.UpdateStatus(driverId, status);

                    var userDriver = _dbContext.Users.FirstOrDefault(u => u.Id == driver.UserId);
                    
                    if (userDriver == null)
                    {
                        throw new NotFoundException("User not found");
                    }

                    userDriver.IsActive = true;
                }

                _dbContext.SaveChanges();
                dbContextTransaction.Commit();
            }
            catch (Exception)
            {
                dbContextTransaction.Rollback();
                throw new Exception();
            }       
        }

        public DocumentDto GetDocumentByDriverId(int id)
        {
            var driver = _dbContext.Drivers.FirstOrDefault(d => d.Id == id);

            if (driver == null)
            {
                throw new NotFoundException("Driver not found");
            }

            var document = _dbContext.Documents.FirstOrDefault(doc => doc.Id == driver.DocumentId);

            if (document == null)
            {
                throw new NotFoundException("Document not found");
            }

            var result = new DocumentDto()
            {
                Id = document.Id,
                DocumentStatus = document.DocumentStatus,
                FileLocation = document.FileLocation,
                RequestDate = document.RequestDate
            };

            return result;
        }
        public PagedList<UserDto> GetAll(UserQuery query)
        {
            IQueryable<User> baseQuery = _dbContext.Users;

            if (query.RoleId != null)
            {
                baseQuery = baseQuery.Where(u => u.RoleId == query.RoleId);
            }

            var users = baseQuery
                .Select(u => new UserDto()
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surename = u.Surename,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    JoinAt = u.JoinAt,
                    RoleId = u.RoleId,
                    IsActive = u.IsActive,
                    Balance = u.Balance
                })
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var result = new PagedList<UserDto>(users, query.PageSize, query.PageNumber, users.Count);

            return result;
        }

        public void UpdateUser(int id, UpdateUserDto dto)
        {
            var user = _dbContext.Users.FirstOrDefault(d => d.Id == id);

            if (user == null)
            {
                throw new NotFoundException("Document not found");
            }

            if (!string.IsNullOrEmpty(dto.Name))
            {
                user.Name = dto.Name;
            }

            if (!string.IsNullOrEmpty(dto.Surename))
            {
                user.Surename = dto.Surename;
            }

            if (!string.IsNullOrEmpty(dto.Email))
            {
                user.Email = dto.Email;
            }

            if (!string.IsNullOrEmpty(dto.PhoneNumber))
            {
                user.PhoneNumber = dto.PhoneNumber;
            }

            if (!string.IsNullOrEmpty(dto.Password))
            {
                var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
                user.PasswordHash = hashedPassword;
            }

            if (dto.IsActive.HasValue)
            {
                user.IsActive = (bool)dto.IsActive;
            }

            _dbContext.SaveChanges();
        }
    }
}
