using System.Threading.Tasks;
using System.Threading;
using BlazeUTS.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.JSInterop;

namespace BlazeUTS.Service
{
	public class AuthMessageHandler : DelegatingHandler
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthMessageHandler(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
			if (!string.IsNullOrEmpty(authHeader))
			{
				request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authHeader.Replace("Bearer ", ""));
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}

}