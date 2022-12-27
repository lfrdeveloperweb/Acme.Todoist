﻿using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Security;
using Acme.Todoist.Domain.Specs.Core;
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
                 , u.birth_date
                 , u.email
                 , u.email_confirmed
	             , u.phone_number
                 , u.phone_number_confirmed
	             , u.role_id as role
                 , u.user_name
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
    
    public async Task<User> GetAsync(Specification<User> spec, CancellationToken cancellationToken)
    {
        return new User();
    }

    /// <inheritdoc />
    public async Task<User> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        const string commandText = $"{BaseSelectCommandText} WHERE u.user_id = @Id";

        var query = await base.Connection.QueryAsync<User, Membership, Membership, User>(
            sql: commandText,
            map: MapProperties,
            param: new { id },
            transaction: base.Transaction);

        return query.FirstOrDefault();
    }

    public async Task<User> GetByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken)
    {
        const string commandText = $"{BaseSelectCommandText} WHERE u.document_number = @DocumentNumber";

        var query = await base.Connection.QueryAsync<User, Membership, Membership, User>(
            sql: commandText,
        map: MapProperties,
            param: new { documentNumber },
            transaction: base.Transaction);

        return query.FirstOrDefault();
    }

    public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        const string commandText = $"{BaseSelectCommandText} WHERE u.email = @Email";

        var query = await base.Connection.QueryAsync<User, Membership, Membership, User>(
            sql: commandText,
            map: MapProperties,
            param: new { email },
            transaction: base.Transaction);

        return query.FirstOrDefault();
    }

    public async Task<User> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        const string commandText = $"{BaseSelectCommandText} WHERE u.user_name = @UserName";

        var query = await base.Connection.QueryAsync<User, Membership, Membership, User>(
            sql: commandText,
            map: MapProperties,
            param: new { userName },
            transaction: base.Transaction);

        return query.FirstOrDefault();
    }

    public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken)
    {
        const string commandText =
            @"SELECT 1 FROM ""user"" WHERE user_id = @Id;";

        return ExistsWithTransactionAsync(commandText, new { id }, cancellationToken);
    }

    public Task<bool> ExistByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken)
    {
        const string commandText =
            @"SELECT 1 FROM ""user"" WHERE document_number = @DocumentNumber;";

        return ExistsWithTransactionAsync(commandText, new { DocumentNumber = documentNumber }, cancellationToken);
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
    
    public Task<bool> ExistByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        const string commandText =
            @"SELECT 1 FROM ""user"" WHERE user_name = @UserName;";

        return ExistsWithTransactionAsync(commandText, new { userName }, cancellationToken);
    }

    public Task CreateAsync(User user, CancellationToken cancellationToken)
    {
        const string commandText = @"
                INSERT INTO ""user""
                (
                    user_id,
                    document_number,
                    name,
                    email,
                    birth_date,
	                phone_number,
	                role_id,
                    user_name,
	                password_hash,
                    created_at,
                    created_by
                ) 
                VALUES 
                (
                    @Id,
                    @DocumentNumber,
                    @Name,
                    @Email,
                    @BirthDate,
	                @PhoneNumber,
	                @Role,
                    @UserName,
                    @PasswordHash,
                    @CreatedAt,
                    @CreatedBy
                );";

        return ExecuteWithTransactionAsync(commandText, new
        {
            user.Id,
            user.DocumentNumber,
            user.Name,
            user.BirthDate,
            user.Email,
            user.PhoneNumber,
            user.UserName,
            user.PasswordHash,
            user.Role,
            user.CreatedAt,
            CreatedBy = user.CreatedBy?.Id
        }, cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        const string commandText = @"
            UPDATE ""user""
               SET document_number = @DocumentNumber
                 , name = @Name
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
            user.Id,
            user.DocumentNumber,
            user.Name,
            user.BirthDate,
            user.Email,
            user.PhoneNumber,
            user.LoginCount,
            user.LastLoginAt,
            user.IsLocked,
            user.AccessFailedCount,
            user.UpdatedAt,
            UpdatedBy = user.UpdatedBy.Id
        }, cancellationToken);
    }
    
    public Task ChangePasswordAsync(User user, CancellationToken cancellationToken)
    {
        const string commandText = @"
			UPDATE ""user""
               SET password_hash = @PasswordHash
                 , updated_by = @UpdatedBy
                 , updated_at = @UpdatedAt
             WHERE user_id = @Id";

        return ExecuteWithTransactionAsync(commandText, new
        {
            user.Id,
            user.PasswordHash,
            user.UpdatedAt,
            UpdatedBy = user.UpdatedBy.Id
        }, cancellationToken);
    }

    private static User MapProperties(User user, Membership creator, Membership updater)
    {
        user.CreatedBy = creator;
        user.UpdatedBy = updater;

        return user;
    }
}