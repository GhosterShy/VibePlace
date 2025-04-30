namespace VibePlace.AppMiddleWare
{
	public class IEMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly string _errorMessage;

		public IEMiddleware(RequestDelegate next, IConfiguration config)
		{
			_next = next;
			_errorMessage = config["IEMiddleware:ErrorMessage"] ?? "Ваш браузер устарел!";
		}

		public async Task InvokeAsync(HttpContext context)
		{
			string userAgent = context.Request.Headers["User-Agent"].ToString();

			if (userAgent.Contains("MSIE") || userAgent.Contains("Trident/"))
			{
				context.Response.StatusCode = 403;
				context.Response.ContentType = "text/plain; charset=utf-8";
				await context.Response.WriteAsync(_errorMessage);
				return;
			}

			await _next(context);
		}
	}
}
