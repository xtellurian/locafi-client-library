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
        public static string GetTemplates => "GetFilteredTemplates";


        public static string GetTemplate(Guid id)
        {
            return $"GetTemplate/{id}";
        }
    }
}
