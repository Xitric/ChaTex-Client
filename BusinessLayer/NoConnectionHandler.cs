using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class NoConnectionHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound && response.Content.Headers.ContentType == null)
            {
                response.Content = new StringContent("Network error, unable to contact server!\nWe apologize, but your requests has been lost. Please retry again palter. If the error persists, please contact your system administrator.");
            }

            return response;
        }
    }
}
