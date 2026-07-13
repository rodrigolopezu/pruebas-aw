using Microsoft.AspNetCore.Mvc.ApplicationModels;
using eb17953u202421866.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;

namespace eb17953u202421866.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;

/// <summary>
///     Replaces the default [controller] route token with its kebab-case form, so e.g.
///     "StartupIncorporationsController" maps to /api/v1/startup-incorporations automatically
///     when you use [Route("api/v1/[controller]")] instead of a hardcoded literal route.
/// </summary>
/// <remarks>
///     Author: __YOUR_NAME__
/// </remarks>
public class KebabCaseRouteNamingConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        foreach (var selector in controller.Selectors)
            selector.AttributeRouteModel = ReplaceControllerTemplate(selector, controller.ControllerName);

        foreach (var selector in controller.Actions.SelectMany(a => a.Selectors))
            selector.AttributeRouteModel = ReplaceControllerTemplate(selector, controller.ControllerName);
    }

    private static AttributeRouteModel? ReplaceControllerTemplate(SelectorModel selector, string name)
    {
        return selector.AttributeRouteModel != null
            ? new AttributeRouteModel
            {
                Template = selector.AttributeRouteModel.Template?.Replace("[controller]", name.ToKebabCase())
            }
            : null;
    }
}
