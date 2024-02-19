using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Framework.AspNetCore.Response;

public static class ModelStateExtensions
{
    public static List<string> GetAllErrors(this ModelStateDictionary modelState)
    {
        var result = new List<string>();
        var erroneousFields = modelState.Where(ms => ms.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });

        foreach (var erroneousField in erroneousFields)
        {
            var fieldKey = erroneousField.Key;
            var fieldErrors = erroneousField.Errors
                .Select(error => error.ErrorMessage);
            result.AddRange(fieldErrors);
        }

        return result;
    }
}