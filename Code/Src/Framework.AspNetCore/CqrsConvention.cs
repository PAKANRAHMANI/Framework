using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Framework.AspNetCore
{
    public class CqrsConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers.Where(a => a.ControllerType.Name.Contains("Query",StringComparison.OrdinalIgnoreCase)))
            {
                foreach (var model in controller.Selectors.Where(b=>b.AttributeRouteModel != null))
                {
                    var controllerName = GetControllerName(controller.ControllerType.Name, "Query");
                    model.AttributeRouteModel = new AttributeRouteModel()
                    {
                        Template = $"api/{controllerName}"
                    };
                }
            }
        }
        private static string GetControllerName(string source, string term)
        {
            var index = source.IndexOf(term, StringComparison.OrdinalIgnoreCase);
            var apiRoute = RemoveControllerFromControllerTypeName(source);
            return apiRoute.Remove(index, term.Length);
        }

        private static string RemoveControllerFromControllerTypeName(string source)
        {
            var index = source.IndexOf("Controller", StringComparison.OrdinalIgnoreCase);
            return source.Remove(index,10);
        }
    }
}
