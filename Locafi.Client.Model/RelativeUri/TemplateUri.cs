using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.RelativeUri
{
    public static class TemplateUri
    {
        public static string ServiceName => "Templates";
        public static string GetTemplates => "GetTemplates";


        public static string GetTemplate(Guid id)
        {
            return $"GetTemplate/{id}";
        }

        public static string GetTemplateFor(TemplateFor templateFor)
        {
            return $"GetTemplatesForType/{Enum.GetName(typeof(TemplateFor), templateFor)}";
        }
    }
}
