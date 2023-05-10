using Peasie.Contracts.Interfaces;
using System.Text;

namespace PeasieLib
{
    public static class HTMLTableHelper
    {
        #region Properties
        public static string TableStart { get; set; } = "<table>";
        public static string TableEnd { get; set; } = "</table>";
        public static string TableHeadRowStart { get; set; } = "<thead><tr>";
        public static string TableHeadRowEnd { get; set; } = "</tr></thead>";
        public static string TableHeadColumnStart { get; set; } = "<th>";
        public static string TableHeadColumnEnd { get; set; } = "</th>";
        public static string TableBodyStart { get; set; } = "<tbody>";
        public static string TableBodyEnd { get; set; } = "</tbody>";
        public static string TableRowStart { get; set; } = "<tr>";
        public static string TableRowEnd { get; set; } = "</tr>";
        public static string TableColumnStart { get; set; } = "<td>";
        public static string TableColumnEnd { get; set; } = "</td>";
        #endregion

        #region Methods
        public static string IEnumerableParameterToHtmlTable<T>(IEnumerable<T> enums)
        {
            var type = typeof(T);
            var props = type.GetProperties();
            var html = new StringBuilder(TableStart);

            //Header
            html.Append(TableHeadRowStart);
            foreach (var p in props)
                html.Append(TableHeadColumnStart + p.Name + TableHeadColumnEnd);
            html.Append(TableHeadRowEnd);

            //Body
            html.Append(TableBodyStart);

            foreach (var e in enums)
            {
                html.Append(TableRowStart);
                props.Select(s => s.GetValue(e)).ToList().ForEach(p =>
                {
                    if (p != null && p.GetType().GetInterface(nameof(IToHtmlTable)) != null)
                    {
                        html.Append(TableColumnStart + ParameterToHtmlTable(p) + TableColumnEnd);
                    }
                    else
                    {
                        html.Append(TableColumnStart + p + TableColumnEnd);
                    }
                });
                html.Append(TableRowEnd);
            }

            html.Append(TableBodyEnd);
            html.Append(TableEnd);
            return html.ToString();
        }

        // Extension method with identical name would replace previous definition
        public static string ParameterToHtmlTable<T>(T item)
        {
            if (item == null)
                return "";

            var type = item.GetType();
            var props = type.GetProperties();
            var html = new StringBuilder(TableStart);

            //Header
            html.Append(TableHeadRowStart);
            foreach (var prop in props)
                html.Append(TableHeadColumnStart + prop.Name + TableHeadColumnEnd);
            html.Append(TableHeadRowEnd);

            //Body
            html.Append(TableBodyStart);

            html.Append(TableRowStart);
            props.Select(s => s.GetValue(item)).ToList().ForEach(parameter =>
            {
                Type? elementType = null;
                if (parameter != null && parameter.GetType().GetInterface(nameof(IToHtmlTable)) != null 
                    && parameter.GetType().GetInterfaces().Any(t => t.IsGenericType && (elementType = t.GetGenericTypeDefinition()) == typeof(IEnumerable<>)))
                {                    
                    html.Append(TableColumnStart + ParameterToHtmlTable(parameter) + TableColumnEnd);
                }
                else if(parameter != null && parameter.GetType().GetInterface(nameof(IToHtmlTable)) != null)
                {
                   html.Append(TableColumnStart + ParameterToHtmlTable(parameter) + TableColumnEnd);
                }
                else if(parameter != null)
                {
                    html.Append(TableColumnStart + parameter + TableColumnEnd);
                }
            });
            html.Append(TableRowEnd);

            html.Append(TableBodyEnd);
            html.Append(TableEnd);
            return html.ToString();
        }

        public static string IEnumerableToHtmlTable<T>(this IEnumerable<T> enums)
        {
            return IEnumerableParameterToHtmlTable(enums);
        }

        public static string ToHtmlTable<T>(T item)
        {
            return ParameterToHtmlTable(item);
        }
        #endregion
    }
}
