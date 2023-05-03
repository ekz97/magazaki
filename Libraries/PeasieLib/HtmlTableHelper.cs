using Peasie.Contracts.Interfaces;
using System.Text;

namespace PeasieLib
{
    public static class HTMLTableHelper
    {
        public static string IEnumerableParameterToHtmlTable<T>(IEnumerable<T> enums)
        {
            var type = typeof(T);
            var props = type.GetProperties();
            var html = new StringBuilder("<table>");

            //Header
            html.Append("<thead><tr>");
            foreach (var p in props)
                html.Append("<th>" + p.Name + "</th>");
            html.Append("</tr></thead>");

            //Body
            html.Append("<tbody>");
            foreach (var e in enums)
            {
                html.Append("<tr>");
                props.Select(s => s.GetValue(e)).ToList().ForEach(p =>
                {
                    if (p != null && p.GetType().GetInterface(nameof(IToHtmlTable)) != null)
                    {
                        html.Append("<td>" + ParameterToHtmlTable(p) + "</td>");
                    }
                    else
                    {
                        html.Append("<td>" + p + "</td>");
                    }
                });
                html.Append("</tr>");
            }

            html.Append("</tbody>");
            html.Append("</table>");
            return html.ToString();
        }

        public static string IEnumerableToHtmlTable<T>(this IEnumerable<T> enums)
        {
            return IEnumerableParameterToHtmlTable(enums);
        }

        // Extension method with identical name would replace previous definition
        public static string ParameterToHtmlTable<T>(T item)
        {
            if (item == null)
                return "";

            var type = item.GetType();
            var props = type.GetProperties();
            var html = new StringBuilder("<table>");

            //Header
            html.Append("<thead><tr>");
            foreach (var prop in props)
                html.Append("<th>" + prop.Name + "</th>");
            html.Append("</tr></thead>");

            html.Append("<tr>");
            props.Select(s => s.GetValue(item)).ToList().ForEach(parameter =>
            {
                Type elementType = null;
                if (parameter != null && parameter.GetType().GetInterface(nameof(IToHtmlTable)) != null 
                    && parameter.GetType().GetInterfaces().Any(t => t.IsGenericType && (elementType = t.GetGenericTypeDefinition()) == typeof(IEnumerable<>)))
                {                    
                    html.Append("<td>" + ParameterToHtmlTable(parameter) + "</td>");
                }
                else if(parameter != null && parameter.GetType().GetInterface(nameof(IToHtmlTable)) != null)
                {
                   html.Append("<td>" + ParameterToHtmlTable(parameter) + "</td>");
                }
                else if(parameter != null)
                {
                    html.Append("<td>" + parameter + "</td>");
                }
            });
            html.Append("</tr>");

            //Body
            html.Append("<tbody>");

            html.Append("</tbody>");
            html.Append("</table>");
            return html.ToString();
        }

        public static string ToHtmlTable<T>(T item)
        {
            return ParameterToHtmlTable(item);
        }
    }
}
