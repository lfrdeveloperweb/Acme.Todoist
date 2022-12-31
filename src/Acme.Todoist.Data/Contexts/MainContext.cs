using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Data.Repositories;
using Acme.Todoist.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Internal;
using Npgsql;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Data.Contexts
{
    public sealed class MainContext : DbContext, IUnitOfWork, IHealthCheck
    {
        private readonly ISystemClock _systemClock;

        private IUserRepository _userRepository;
        private IProjectRepository _projectRepository;
        private ITodoRepository _todoRepository;

        public MainContext(DbContextOptions<MainContext> options, ISystemClock systemClock)
            : base(options)
        {
            // Database.Log = (sql) => Debug.Write(sql);
            //Configuration.LazyLoadingEnabled = true;
            //Configuration.ProxyCreationEnabled = false;
            //Configuration.AutoDetectChangesEnabled = true;

            _systemClock = systemClock;
        }


        public IUserRepository UserRepository => _userRepository ??= new UserRepository(this);

        public IProjectRepository ProjectRepository => _projectRepository ??= new ProjectRepository(this);

        public ITodoRepository TodoRepository => _todoRepository ??= new TodoRepository(this);

        public IDbConnection CreateConnection() => new NpgsqlConnection(base.Database.GetDbConnection().ConnectionString);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Magazine>().HasData
            //(
            //    new Magazine { MagazineId = 1, Name = "BASTA! Magazine" }
            //);

            // modelBuilder.Properties<datetime>().Configure(c => c.HasColumnType("datetime2").HasPrecision(4));

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            modelBuilder.Owned<Membership>();

            base.OnModelCreating(modelBuilder);
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            throw new NotImplementedException();
        }

        public Task CommitTransactionAsync()
        {
            //foreach (var entityEntry in ChangeTracker.Entries())
            //{
            //    switch (entityEntry.State)
            //    {
            //        case EntityState.Added when entityEntry.Entity is EntityBase createdEntity:
            //            createdEntity.CreatedAt = _dateTimeProvider.UtcNow;
            //            break;
            //        case EntityState.Added when entityEntry.Entity is EntityBase updatedEntity:
            //            updatedEntity.UpdatedAt = _dateTimeProvider.UtcNow;
            //            break;
            //    }
            //}

            return SaveChangesAsync();
        }

        public void RollbackTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
