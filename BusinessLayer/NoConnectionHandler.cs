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

            if (noConnection(response) || siteDisabled(response))
            {
                response.Content = new StringContent("Network error, unable to contact server!\n" +
                    "We apologize, but your request has been lost. Please retry again later. If the error persists, please contact your system administrator.");
            }

            return response;
        }

        private bool noConnection(HttpResponseMessage response)
        {
            return response.StatusCode == System.Net.HttpStatusCode.NotFound && response.Content.Headers.ContentType == null;
        }

        private bool siteDisabled(HttpResponseMessage response)
        {
            return response.StatusCode == System.Net.HttpStatusCode.Forbidden && response.ReasonPhrase.Equals("Site Disabled");
        }
    }
}
