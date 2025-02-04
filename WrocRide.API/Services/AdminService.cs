namespace WrocRide.API.Services
{
    public interface IAdminService
    {
        Task<PagedList<DocumentDto>> GetDocuments(DocumentQuery query);
        Task UpdateDocument(int id, UpdateDocumentDto dto);
        Task<PagedList<UserDto>> GetAll(UserQuery query);
        Task UpdateUser(int id, UpdateUserDto dto);
        Task<PagedList<ReportDto>> GetReports(ReportQuery query);
        Task UpdateReport(int id, UpdateReportDto dto);
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

        public async Task<PagedList<DocumentDto>> GetDocuments(DocumentQuery query)
        {
            IQueryable<Document> baseQuery = _dbContext.Documents;

            if (query.DocumentStatus != null)
            {
                baseQuery = baseQuery.Where(d => d.DocumentStatus == query.DocumentStatus);
            }

            var totalItemsCount = await baseQuery.CountAsync();
            
            var documents = await baseQuery
                .Select(d => new DocumentDto()
                {
                    Id = d.Id,
                    DocumentStatus = d.DocumentStatus,
                    FileLocation = d.FileLocation,
                    RequestDate = d.RequestDate
                })
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToListAsync();

            var result = new PagedList<DocumentDto>(documents, query.PageSize, query.PageNumber, totalItemsCount);

            return result;
        }

        public async Task UpdateDocument(int id, UpdateDocumentDto dto)
        {
            var userId = _userContextService.GetUserId;
            var admin = await _dbContext.Admins.FirstOrDefaultAsync(u => u.UserId == userId);

            if (admin == null)
            {
                throw new NotFoundException("Admin not found");
            }

            await using var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var document = await _dbContext.Documents.FirstOrDefaultAsync(d => d.Id == id);

                if (document == null)
                {
                    throw new NotFoundException("Document not found");
                }

                document.DocumentStatus = dto.DocumentStatus;
                document.ExaminationDate = DateTime.UtcNow;
                document.AdminId = admin.Id;

                if (dto.DocumentStatus == DocumentStatus.Accepted)
                {
                    var driver = await _dbContext.Drivers.FirstOrDefaultAsync(d => d.DocumentId == id);

                    if (driver == null)
                    {
                        throw new NotFoundException("Driver not found");
                    }

                    driver.DriverStatus = DriverStatus.Offline;

                    var userDriver = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == driver.UserId);
                    
                    if (userDriver == null)
                    {
                        throw new NotFoundException("User not found");
                    }

                    userDriver.IsActive = true;
                }

                await _dbContext.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync();
                throw;
            }       
        }

        public async Task<PagedList<UserDto>> GetAll(UserQuery query)
        {
            IQueryable<User> baseQuery = _dbContext.Users;

            if (query.RoleId != null)
            {
                baseQuery = baseQuery.Where(u => u.RoleId == query.RoleId);
            }

            var totalItemsCount = await baseQuery.CountAsync();
            
            var users = await baseQuery
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
                .ToListAsync();

            var result = new PagedList<UserDto>(users, query.PageSize, query.PageNumber, totalItemsCount);

            return result;
        }

        public async Task UpdateUser(int id, UpdateUserDto dto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(d => d.Id == id);

            if (user == null)
            {
                throw new NotFoundException("User not found");
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

            await _dbContext.SaveChangesAsync();
        }

        public async Task<PagedList<ReportDto>> GetReports(ReportQuery query)
        {
            IQueryable<Report> baseQuery = _dbContext.Reports;

            if (query.ReportStatus != null)
            {
                baseQuery = baseQuery.Where(r => r.ReportStatus == query.ReportStatus);
            }

            var totalItemsCount = await baseQuery.CountAsync();

            var reports = await baseQuery
                .Select(r => new ReportDto()
                {
                    Id = r.Id,
                    CreatedAt = r.CreatedAt,
                    ReportStatus = r.ReportStatus,
                    Reason = r.Reason,
                    ReporterUserId = r.ReporterUserId,
                    ReportedUserId = r.ReportedUserId,
                    RideId = r.RideId,
                    AdminId = r.AdminId
                })
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToListAsync();

            var result = new PagedList<ReportDto>(reports, query.PageSize, query.PageNumber, totalItemsCount);

            return result;
        }

        public async Task UpdateReport(int id, UpdateReportDto dto)
        {
            var userId = _userContextService.GetUserId;
            var admin = await _dbContext.Admins.FirstOrDefaultAsync(u => u.UserId == userId);

            if (admin == null)
            {
                throw new ForbidException("User is not an admin.");
            }

            await using var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var report = await _dbContext.Reports.FirstOrDefaultAsync(r => r.Id == id);

                if (report == null)
                {
                    throw new NotFoundException("Report not found");
                }

                report.ReportStatus = dto.ReportStatus;
                report.AdminId = admin.Id;

                if (dto.ReportStatus == ReportStatus.Accepted)
                {
                    var status = new UpdateUserDto
                    {
                        IsActive = false
                    };

                    await UpdateUser(report.ReportedUserId, status);
                }

                await _dbContext.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync();
                throw;
            }
        }
    }
}
