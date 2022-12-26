using System;
using Acme.Todoist.Domain.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acme.Todoist.Domain.Security;

namespace Acme.Todoist.Domain.Models
{
    public class EntityBase
    {
        private Collection<IEvent> _events;

        /// <summary>
        /// Identifier of user that created the record.
        /// </summary>
        public Membership CreatedBy { get; set; }

        /// <summary>
        /// Date and time of record creation.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Identifier of user that done last modification of the record.
        /// </summary>
        public Membership UpdatedBy { get; set; }

        /// <summary>
        /// Date and time of last modification of the record.
        /// </summary>
        public DateTimeOffset? UpdatedAt { get; set; }

        public IReadOnlyCollection<IEvent> Events => _events?.AsReadOnly();

        public void AddEvent(IEvent @event)
        {
            _events ??= new Collection<IEvent>();
            _events.Add(@event);
        }

        public void RemoveEvent(IEvent eventItem) => _events?.Remove(eventItem);

        public void ClearEvents() => _events?.Clear();
    }
}
