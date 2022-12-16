﻿using System;
using Acme.Todoist.Domain.Commons;

namespace Acme.Todoist.Domain.Models
{
    public sealed class User : EntityBase
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public DateTime? BirthDate { get; set; }

        public Role Role { get; set; }

        public string PasswordHash { get; private set; }

        public bool IsLocked { get; private set; }

        public int AccessFailedCount { get; private set; }

        public int LoginCount { get; private set; }

        public DateTimeOffset? LastLoginAt { get; private set; }

        public void SetPassword(string passwordHashed) => PasswordHash = passwordHashed;

        public void IncreaseAccessCount(DateTimeOffset loginAt)
        {
            LoginCount++;
            LastLoginAt = loginAt;
        }

        public void ResetAccessFailedCount() => AccessFailedCount = 0;

        public void IncreaseAccessFailedCount() => AccessFailedCount++;

        public void Lock()
        {
            ResetAccessFailedCount();
            IsLocked = true;
        }

        public void Unlock()
        {
            ResetAccessFailedCount();
            IsLocked = false;
        }
    }
}
