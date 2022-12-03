using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.Todoist.Commons.Models;

namespace Acme.Todoist.Infrastructure.Queries
{
    public record ListQueryResult<TData>(IEnumerable<TData> Data, int StatusCode, ICollection<Report> Reports = null)
        : QueryResult<IEnumerable<TData>>(Data, StatusCode, Reports);
}
