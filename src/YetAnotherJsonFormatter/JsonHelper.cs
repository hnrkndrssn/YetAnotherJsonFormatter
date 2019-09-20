using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace YetAnotherJsonFormatter
{
    public static class JsonHelper
    {
        public static string FormatJson(dynamic parsedJson, int indentation, char indentChar, bool compact, char quoteChar, bool quoteNames)
        {
            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms))
            using (var jw = new JsonTextWriter(sw))
            {
                jw.Formatting = compact ? Formatting.None : Formatting.Indented;
                jw.Indentation = indentation;
                jw.IndentChar = indentChar;
                jw.QuoteChar = quoteChar;
                jw.QuoteName = quoteNames;

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, parsedJson);
                jw.Flush();

                ms.Position = 0;

                var formattedJson = new StreamReader(ms).ReadToEnd();
                return formattedJson;
            }
        }

        public static bool ParseJson(string json, bool validate, out dynamic parsedJson, out Exception error)
        {
            try
            {
                parsedJson = JToken.Parse(json);
                error = null;
                return true;
            }
            catch (JsonReaderException jrex)
            {
                parsedJson = string.Empty;

                if (!validate)
                {
                    parsedJson = null;
                }

                error = jrex;
                return false;
            }
        }
    }
}
