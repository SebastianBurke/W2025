using Microsoft.AspNetCore.Mvc;

namespace HIAAA.Models;

public static class BreadcrumbExtensions
{
    public static List<(string label, string link)> GetBreadcrumbs(this ActionContext context)
    {
        var controller = (context.RouteData.Values["controller"] ?? context.RouteData.Values["area"]).ToString();
        var action = (context.RouteData.Values["action"] ?? context.RouteData.Values["page"]).ToString();
        action = action.Substring(action.LastIndexOf('/') + 1);

        var breadcrumbs = new List<(string, string)>
        {
            (controller, $"/{controller}"),
            (action, "")
        };

        return breadcrumbs;
    }
}