using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Todoist.Domain.ValueObjects
{
    public sealed record JwtToken(string Token, string TokenType, int ExpiresAt);
}
