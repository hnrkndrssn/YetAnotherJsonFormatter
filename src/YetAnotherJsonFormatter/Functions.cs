using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace YetAnotherJsonFormatter
{
    public static class Functions
    {
        [FunctionName("Format")]
        public static async Task<IActionResult> Format(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", "options", Route = "json")] HttpRequest req,
            ILogger log)
        {
            if (req.Method == HttpMethods.Options) return new OkResult();

            var json = await req.ReadAsStringAsync();

            var qs = req.Query;
            int indent = int.TryParse(qs["indent"], out int qsIndent) ? qsIndent : 2;
            char indentChar = qs.TryGetValue("indentChar", out var svIndentChar) ? char.TryParse(svIndentChar[0], out char qsIndentChar) ? qsIndentChar : ' ' : ' ';
            bool compact = bool.TryParse(qs["compact"], out bool qsCompact) ? qsCompact : false;
            bool validate = bool.TryParse(qs["validate"], out bool qsValidate) ? qsValidate : true;
            char quoteChar = char.TryParse(qs["quoteChar"], out char qsQuoteChar) ? qsQuoteChar : '"';
            bool quoteNames = bool.TryParse(qs["quoteNames"], out bool qsQuoteNames) ? qsQuoteNames : true;

            if (!JsonHelper.ParseJson(json, validate, out var parsedJson, out var error))
            {
                log.LogError(error, error.Message);
                return new BadRequestObjectResult($"Oh spiffy, input is not valid JSON.{Environment.NewLine}{error.Message}");
            }

            try
            {
                var formattedJson = JsonHelper.FormatJson(parsedJson, indent, indentChar, compact, quoteChar, quoteNames);
                return new JsonResult(formattedJson);
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return new UnprocessableEntityObjectResult($"Oh snap, something went wrong when formatting the JSON.{Environment.NewLine}{ex.Message}");
            }
        }

        [FunctionName("Validate")]
        public static async Task<IActionResult> Validate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", "options", Route = "validate")] HttpRequest req,
            ILogger log)
        {
            if (req.Method == HttpMethods.Options) return new OkResult();

            var json = await req.ReadAsStringAsync();
            if (JsonHelper.ParseJson(json, true, out var _, out var error))
                return new JsonResult(new { success = true, message = "Input is valid JSON" });

            log.LogError(error, error.Message);
            return new BadRequestObjectResult(new { success = false, message = $"Wah wah wah, input is not valid JSON.{Environment.NewLine}{error.Message}" });
        }
    }
}
