using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Security;
using Acme.Todoist.Infrastructure.Data;
using Dapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Data.Repositories;

public sealed class UserRepository : Repository, IUserRepository
{
    private const string BaseSelectCommandText = @"
            SELECT u.user_id as id
                 , u.name
                 , u.email
                 , u.birth_date
	             , u.phone_number
	             , u.role_id as role
	             , u.password_hash
	             , u.last_login_at
                 , u.login_count
                 , u.access_failed_count
                 , u.is_locked
                 , u.created_at
                 , u.updated_at
                 , creator.user_id as id
                 , creator.name
                 , modifier.user_id as id
                 , modifier.name
              FROM ""user"" u
         LEFT JOIN ""user"" creator
                ON creator.user_id = u.created_by
         LEFT JOIN ""user"" modifier
                ON modifier.user_id = u.updated_by";

    public UserRepository(IDbConnector dbConnector) : base(dbConnector) { }

    /// <inheritdoc />
    public async Task<User> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        const string commandText = $"{BaseSelectCommandText} WHERE u.user_id = @Id";

        var query = await base.Connection.QueryAsync<User, Membership, Membership, User>(
            sql: commandText,
            map: MapProperties,
            param: new { Id = id },
            transaction: base.Transaction);

        return query.FirstOrDefault();
    }

    public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        const string commandText = $"{BaseSelectCommandText} WHERE u.email = @Email";

        var query = await base.Connection.QueryAsync<User, Membership, Membership, User>(
            sql: commandText,
            map: MapProperties,
            param: new { Email = email },
            transaction: base.Transaction);

        return query.FirstOrDefault();
    }

    public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken)
    {
        const string commandText =
            @"SELECT 1 FROM ""user"" WHERE user_id = @Id;";

        return ExistsWithTransactionAsync(commandText, new { id }, cancellationToken);
    }

    public Task<bool> ExistByEmailAsync(string email, CancellationToken cancellationToken)
    {
        const string commandText =
            @"SELECT 1 FROM ""user"" WHERE email = @Email;";

        return ExistsWithTransactionAsync(commandText, new { email }, cancellationToken);
    }

    public Task<bool> ExistByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        const string commandText =
            @"SELECT 1 FROM ""user"" WHERE phone_number = @PhoneNumber;";

        return ExistsWithTransactionAsync(commandText, new { phoneNumber }, cancellationToken);
    }

    public Task CreateAsync(User user, CancellationToken cancellationToken)
    {
        const string commandText = @"
                INSERT INTO ""user""
                (
                    user_id,
                    name,
                    email,
                    birth_date,
	                phone_number,
	                role_id,
	                password_hash,
                    created_at,
                    created_by
                ) 
                VALUES 
                (
                    @Id,
                    @Name,
                    @Email,
                    @BirthDate,
	                @PhoneNumber,
	                @Role,
                    @PasswordHash,
                    @CreatedAt,
                    @CreatedBy
                );";

        return ExecuteWithTransactionAsync(commandText, new
        {
            Id = user.Id,
            Name = user.Name,
            BirthDate = user.BirthDate,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            PasswordHash = user.PasswordHash,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            CreatedBy = user.CreatedBy?.Id
        }, cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        const string commandText = @"
            UPDATE ""user""
               SET name = @Name
                 , birth_date = @BirthDate
				 , email = @Email
                 , phone_number = @PhoneNumber
                 , is_locked = @IsLocked
                 , login_count = @LoginCount
                 , last_login_at = @LastLoginAt
                 , access_failed_count = @AccessFailedCount
                 , updated_by = @UpdatedBy
                 , updated_at = @UpdatedAt
             WHERE user_id = @Id;";

        return ExecuteWithTransactionAsync(commandText, new
        {
            Id = user.Id,
            Name = user.Name,
            BirthDate = user.BirthDate,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            LoginCount = user.LoginCount,
            LastLoginAt = user.LastLoginAt,
            IsLocked = user.IsLocked,
            AccessFailedCount = user.AccessFailedCount,
            UpdatedAt = user.UpdatedAt,
            UpdatedBy = user.UpdatedBy?.Id
        }, cancellationToken);
    }
    
    public Task ChangePasswordAsync(string id, string passwordHash, CancellationToken cancellationToken)
    {
        const string commandText = @"
			UPDATE ""user""
               SET password_hash = @PasswordHash
             WHERE user_id = @Id";

        return ExecuteWithTransactionAsync(commandText, new
        {
            Id = id,
            PasswordHash = passwordHash
        }, cancellationToken);
    }

    private static User MapProperties(User user, Membership creator, Membership updater)
    {
        user.CreatedBy = creator;
        user.UpdatedBy = updater;

        return user;
    }
}