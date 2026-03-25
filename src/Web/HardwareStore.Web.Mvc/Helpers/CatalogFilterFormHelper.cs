namespace HardwareStore.Web.Mvc.Helpers
{
    using System.Text.Json.Nodes;
    using Microsoft.AspNetCore.Http;

    public static class CatalogFilterFormHelper
    {
        private static readonly HashSet<string> IgnoredFormKeys = new(StringComparer.OrdinalIgnoreCase)
        {
            "Order",
            "__RequestVerificationToken",
            "category",
            "keyword",
            "pageTitle",
        };

        public static string BuildFilterJson(IFormCollection form)
        {
            var orderStr = form["Order"].FirstOrDefault();
            var order = 1;
            if (!string.IsNullOrEmpty(orderStr) && int.TryParse(orderStr, out var parsed))
            {
                order = parsed;
            }

            var root = new JsonObject { ["Order"] = order };

            foreach (var pair in form)
            {
                if (IgnoredFormKeys.Contains(pair.Key))
                {
                    continue;
                }

                var values = pair.Value.Where(v => !string.IsNullOrEmpty(v)).ToList();
                if (values.Count == 0)
                {
                    continue;
                }

                if (values.Count == 1)
                {
                    root[pair.Key] = values[0];
                }
                else
                {
                    var arr = new JsonArray();
                    foreach (var v in values)
                    {
                        arr.Add(v);
                    }

                    root[pair.Key] = arr;
                }
            }

            return root.ToJsonString();
        }

        public static Dictionary<string, IReadOnlyList<string>> ParseSelectedFilters(IFormCollection form)
        {
            var dict = new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase);
            foreach (var pair in form)
            {
                if (IgnoredFormKeys.Contains(pair.Key))
                {
                    continue;
                }

                var values = pair.Value.Where(v => !string.IsNullOrEmpty(v)).Select(v => v!).ToList();
                if (values.Count > 0)
                {
                    dict[pair.Key] = values;
                }
            }

            return dict;
        }
    }
}
