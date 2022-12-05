using System.Reflection;

namespace Acme.Todoist.Infrastructure
{
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    }
}
