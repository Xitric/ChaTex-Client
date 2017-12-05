using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class TokenHandler : DelegatingHandler
    {
        private static readonly string tokenIdentifier = "token";

        private static string token;
        public static string Token { set => token = value; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (token != null)
            {
                request.Headers.Add(tokenIdentifier, token);
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
