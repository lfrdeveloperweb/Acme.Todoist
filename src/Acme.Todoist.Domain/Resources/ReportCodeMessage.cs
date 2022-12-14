using System.Runtime.CompilerServices;

namespace Acme.Todoist.Domain.Resources
{
    public static class ReportCodeMessage
    {
        public static string GetMessage(ReportCodeType code) => Resource.ResourceManager.GetString(code.ToString()) ?? code.ToString();

        public static string GetMessage(ReportCodeType code, params object[] parameters) => 
            FormattableStringFactory.Create(GetMessage(code), parameters).ToString();
    }
}
