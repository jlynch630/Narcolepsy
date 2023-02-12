namespace Narcolepsy.Core.Http;

public record HttpResponse(DateTime RequestDate, TimeSpan ExecutionTime, int StatusCode, string StatusText, HttpResponseHeader[] ResponseHeaders, byte[] ResponseBody, RequestExecutionError? Error);