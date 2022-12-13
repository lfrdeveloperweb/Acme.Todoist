using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Commons.Models.Security;
using Acme.Todoist.Domain.Models;
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
                 , u.password
                 , u.created_at
                 , u.updated_at
                 , creator.user_id as id
                 , creator.name
                 , modifier.user_id as id
                 , modifier.name
              FROM ""user"" u
        INNER JOIN ""user"" creator
                ON creator.user_id = u.created_by
        INNER JOIN ""user"" modifier
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
        const string commandText = $"{BaseSelectCommandText} WHERE u.email = @Id";

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

        return ExistsWithTransactionAsync(commandText, new { Id = id }, cancellationToken);
    }

    public Task CreateAsync(User user, CancellationToken cancellationToken)
    {
        const string commandText = @"
                INSERT INTO ""user""
                (
                    user_id,
                    name,
                    email,
                    password,
                    created_at,
                    created_by
                ) 
                VALUES 
                (
                    @Id,
                    @Name,
                    @Email,
                    @Password,
                    @CreatedAt,
                    @CreatedBy
                );";

        return ExecuteWithTransactionAsync(commandText, new
        {
            Id = user.Id,
            Title = user.Name,
            Email = user.Email,
            Password = user.Password,
            CreatedAt = user.CreatedAt,
            CreatedBy = user.CreatedBy?.Id
        }, cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        const string commandText = @"
                UPDATE ""user""
                   SET name = @Title
                     , email = @Description
                     , password = @ProjectId
                     , updated_by = @UpdatedBy
                     , updated_at = @UpdatedAt
                 WHERE user_id = @Id;";

        return ExecuteWithTransactionAsync(commandText, new
        {
            Id = user.Id,
            Title = user.Name,
            Email = user.Email,
            Password = user.Password,
            UpdatedAt = user.UpdatedAt,
            UpdatedBy = user.UpdatedBy?.Id
        }, cancellationToken);
    }

    private static User MapProperties(User user, Membership creator, Membership updater)
    {
        user.CreatedBy = creator;
        user.UpdatedBy = updater;

        return user;
    }
}