using Acme.Todoist.Domain.Security;
using System;

namespace Acme.Todoist.Domain.Models
{
    public sealed class User : EntityBase
    {
        public string Id { get; set; }

        public string DocumentNumber { get; set; }

        public required string Name { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; private set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; private set; }

        public DateTime? BirthDate { get; set; }

        public Role Role { get; set; }

        public string PasswordHash { get; private set; }
        
        public DateTimeOffset? LockedAt { get; private set; }

        public bool IsLocked => LockedAt.HasValue;

        public int AccessFailedCount { get; private set; }

        public int LoginCount { get; private set; }

        public DateTimeOffset? LastLoginAt { get; private set; }

        public void ConfirmEmail()
        {
            EmailConfirmed = true;
        }

        public void ChangePassword(string passwordHashed) => PasswordHash = passwordHashed;

        public void IncreaseAccessCount(DateTimeOffset loginAt)
        {
            LoginCount++;
            LastLoginAt = loginAt;
        }

        public void ResetAccessFailedCount() => AccessFailedCount = 0;

        public void IncreaseAccessFailedCount() => AccessFailedCount++;

        public void Lock(DateTimeOffset lockedAt)
        {
            ResetAccessFailedCount();
            LockedAt = lockedAt;
        }

        public void Unlock()
        {
            ResetAccessFailedCount();
            LockedAt = null;
        }
    }
}
