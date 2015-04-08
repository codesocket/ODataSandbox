//-----------------------------------------------------------------------
// <copyright file="AtomicODataBatchHandler.cs" company="EY">
//     Copyright EY. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData.Batch;


namespace ODataSandbox.Data
{
    /// <summary>
    /// Unit work work batch handler
    /// </summary>
    public class AtomicODataBatchHandler : DefaultODataBatchHandler
    {
        public AtomicODataBatchHandler(HttpServer httpServer)
            : base(httpServer)
        {
        }

        /// <summary>
        /// Execute request messages
        /// </summary>
        /// <param name="requests">requests</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>A list of responses</returns>
        public override async Task<IList<ODataBatchResponseItem>> ExecuteRequestMessagesAsync(IEnumerable<ODataBatchRequestItem> requests, CancellationToken cancellationToken)
        {
            var responses = new List<ODataBatchResponseItem>();

            try
            {
                foreach (ODataBatchRequestItem request in requests)
                {
                    var changeSetRequestItem = request as ChangeSetRequestItem;
                    if (changeSetRequestItem != null)
                    {
                        var relativeRequests = changeSetRequestItem.Requests.Where(r => r.RequestUri.OriginalString.StartsWith("$")).ToList();
                        relativeRequests.ForEach(r => r.Properties.Add("ATF_Parent-Content-ID", r.RequestUri.OriginalString.Split('/').First()));

                        var response = await request.SendRequestAsync(base.Invoker, cancellationToken) as ChangeSetResponseItem;
                        responses.Add(response);

                        if (response.Responses.All(r => r.IsSuccessStatusCode))
                        {

                        }
                    }
                    else
                    {
                        responses.Add(await request.SendRequestAsync(base.Invoker, cancellationToken));
                    }
                }
            }
            catch
            {
                foreach (var response in responses)
                {
                    if (response != null)
                    {
                        response.Dispose();
                    }
                }

                throw;
            }

            return responses;
        }

        public override Task<IList<ODataBatchRequestItem>> ParseBatchRequestsAsync(System.Net.Http.HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.ParseBatchRequestsAsync(request, cancellationToken);
        }

        public override Task<System.Net.Http.HttpResponseMessage> ProcessBatchAsync(System.Net.Http.HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.ProcessBatchAsync(request, cancellationToken);
        }

        public override Task<System.Net.Http.HttpResponseMessage> CreateResponseMessageAsync(IEnumerable<ODataBatchResponseItem> responses, System.Net.Http.HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.CreateResponseMessageAsync(responses, request, cancellationToken);
        }

        public override Uri GetBaseUri(System.Net.Http.HttpRequestMessage request)
        {
            return base.GetBaseUri(request);
        }

        public override void ValidateRequest(System.Net.Http.HttpRequestMessage request)
        {
            base.ValidateRequest(request);
        }
    }
}
