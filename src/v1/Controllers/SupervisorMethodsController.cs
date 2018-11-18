// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.OpcUa.Modules.Twin.v1.Controllers {
    using Microsoft.Azure.IIoT.OpcUa.Modules.Twin.v1.Filters;
    using Microsoft.Azure.IIoT.OpcUa.Modules.Twin.v1.Models;
    using Microsoft.Azure.IIoT.OpcUa.Registry.Models;
    using Microsoft.Azure.IIoT.OpcUa.Registry;
    using Microsoft.Azure.IIoT.OpcUa.Twin;
    using Microsoft.Azure.IIoT.Diagnostics;
    using Microsoft.Azure.IIoT.Module.Framework;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Supervisor method controller
    /// </summary>
    [Version(1)]
    [ExceptionsFilter]
    public class SupervisorMethodsController : IMethodController {

        /// <summary>
        /// Create controller with service
        /// </summary>
        /// <param name="browse"></param>
        /// <param name="discover"></param>
        /// <param name="activator"></param>
        /// <param name="nodes"></param>
        /// <param name="publisher"></param>
        /// <param name="logger"></param>
        public SupervisorMethodsController(IDiscoveryServices discover,
            IBrowseServices<EndpointModel> browse, IActivationServices<string> activator,
            INodeServices<EndpointModel> nodes, IPublishServices<EndpointModel> publisher,
            ILogger logger) {

            _browse = browse ?? throw new ArgumentNullException(nameof(browse));
            _nodes = nodes ?? throw new ArgumentNullException(nameof(nodes));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _discover = discover ?? throw new ArgumentNullException(nameof(discover));
            _activator = activator ?? throw new ArgumentNullException(nameof(activator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Discover application
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> DiscoverAsync(DiscoveryRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            await _discover.DiscoverAsync(request.ToServiceModel());
            return true;
        }

        /// <summary>
        /// Publish
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PublishStartResponseApiModel> PublishStartAsync(
            EndpointApiModel endpoint, PublishStartRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            var result = await _publisher.NodePublishStartAsync(
                endpoint.ToServiceModel(), request.ToServiceModel());
            return new PublishStartResponseApiModel(result);
        }

        /// <summary>
        /// Unpublish
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PublishStopResponseApiModel> PublishStopAsync(
            EndpointApiModel endpoint, PublishStopRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            var result = await _publisher.NodePublishStopAsync(
                endpoint.ToServiceModel(), request.ToServiceModel());
            return new PublishStopResponseApiModel(result);
        }

        /// <summary>
        /// List published nodes
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PublishedNodeListResponseApiModel> PublishListAsync(
            EndpointApiModel endpoint, PublishedNodeListRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            var result = await _publisher.NodePublishListAsync(
                endpoint.ToServiceModel(), request.ToServiceModel());
            return new PublishedNodeListResponseApiModel(result);
        }

        /// <summary>
        /// Browse
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BrowseResponseApiModel> BrowseAsync(
            EndpointApiModel endpoint, BrowseRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            if (endpoint == null) {
                throw new ArgumentNullException(nameof(endpoint));
            }
            var result = await _browse.NodeBrowseFirstAsync(
                endpoint.ToServiceModel(), request.ToServiceModel());
            return new BrowseResponseApiModel(result);
        }

        /// <summary>
        /// Browse next
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BrowseNextResponseApiModel> BrowseNextAsync(
            EndpointApiModel endpoint, BrowseNextRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            if (endpoint == null) {
                throw new ArgumentNullException(nameof(endpoint));
            }
            var result = await _browse.NodeBrowseNextAsync(
                endpoint.ToServiceModel(), request.ToServiceModel());
            return new BrowseNextResponseApiModel(result);
        }

        /// <summary>
        /// Browse by path
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BrowsePathResponseApiModel> BrowsePathAsync(
            EndpointApiModel endpoint, BrowsePathRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            if (endpoint == null) {
                throw new ArgumentNullException(nameof(endpoint));
            }
            var result = await _browse.NodeBrowsePathAsync(
                endpoint.ToServiceModel(), request.ToServiceModel());
            return new BrowsePathResponseApiModel(result);
        }

        /// <summary>
        /// Read value
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ValueReadResponseApiModel> ValueReadAsync(
            EndpointApiModel endpoint, ValueReadRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            if (endpoint == null) {
                throw new ArgumentNullException(nameof(endpoint));
            }
            var result = await _nodes.NodeValueReadAsync(
                endpoint.ToServiceModel(), request.ToServiceModel());
            return new ValueReadResponseApiModel(result);
        }

        /// <summary>
        /// Write value
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ValueWriteResponseApiModel> ValueWriteAsync(
            EndpointApiModel endpoint, ValueWriteRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            if (endpoint == null) {
                throw new ArgumentNullException(nameof(endpoint));
            }
            var result = await _nodes.NodeValueWriteAsync(
                endpoint.ToServiceModel(), request.ToServiceModel());
            return new ValueWriteResponseApiModel(result);
        }

        /// <summary>
        /// Get method meta data
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<MethodMetadataResponseApiModel> MethodMetadataAsync(
            EndpointApiModel endpoint, MethodMetadataRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            if (endpoint == null) {
                throw new ArgumentNullException(nameof(endpoint));
            }
            var result = await _nodes.NodeMethodGetMetadataAsync(
                endpoint.ToServiceModel(), request.ToServiceModel());
            return new MethodMetadataResponseApiModel(result);
        }

        /// <summary>
        /// Call method
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<MethodCallResponseApiModel> MethodCallAsync(
            EndpointApiModel endpoint, MethodCallRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            if (endpoint == null) {
                throw new ArgumentNullException(nameof(endpoint));
            }
            var result = await _nodes.NodeMethodCallAsync(
                endpoint.ToServiceModel(), request.ToServiceModel());
            return new MethodCallResponseApiModel(result);
        }

        /// <summary>
        /// Read attributes
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BatchReadResponseApiModel> BatchReadAsync(
            EndpointApiModel endpoint, BatchReadRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            var result = await _nodes.NodeBatchReadAsync(
                endpoint.ToServiceModel(), request.ToServiceModel());
            return new BatchReadResponseApiModel(result);
        }

        /// <summary>
        /// Write attributes
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BatchWriteResponseApiModel> BatchWriteAsync(
            EndpointApiModel endpoint, BatchWriteRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            var result = await _nodes.NodeBatchWriteAsync(
                endpoint.ToServiceModel(), request.ToServiceModel());
            return new BatchWriteResponseApiModel(result);
        }

        /// <summary>
        /// Read history
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HistoryReadResponseApiModel> HistoryReadAsync(
            EndpointApiModel endpoint, HistoryReadRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            var result = await _nodes.NodeHistoryReadAsync(
               endpoint.ToServiceModel(), request.ToServiceModel());
            return new HistoryReadResponseApiModel(result);
        }

        /// <summary>
        /// Read next history
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HistoryReadNextResponseApiModel> HistoryReadNextAsync(
            EndpointApiModel endpoint, HistoryReadNextRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            var result = await _nodes.NodeHistoryReadNextAsync(
               endpoint.ToServiceModel(), request.ToServiceModel());
            return new HistoryReadNextResponseApiModel(result);
        }

        /// <summary>
        /// Update history
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HistoryUpdateResponseApiModel> HistoryUpdateAsync(
            EndpointApiModel endpoint, HistoryUpdateRequestApiModel request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            var result = await _nodes.NodeHistoryUpdateAsync(
               endpoint.ToServiceModel(), request.ToServiceModel());
            return new HistoryUpdateResponseApiModel(result);
        }

        /// <summary>
        /// Activate endpoint
        /// </summary>
        /// <param name="id"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public async Task<bool> ActivateEndpointAsync(string id, string secret) {
            if (string.IsNullOrEmpty(id)) {
                throw new ArgumentNullException(nameof(id));
            }
            if (string.IsNullOrEmpty(secret)) {
                throw new ArgumentNullException(nameof(secret));
            }
            if (!secret.IsBase64()) {
                throw new ArgumentException("not base64", nameof(secret));
            }
            await _activator.ActivateEndpointAsync(id, secret);
            return true;
        }

        /// <summary>
        /// Deactivate endpoint
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeactivateEndpointAsync(string id) {
            if (string.IsNullOrEmpty(id)) {
                throw new ArgumentNullException(nameof(id));
            }
            await _activator.DeactivateEndpointAsync(id);
            return true;
        }

        private readonly ILogger _logger;
        private readonly IActivationServices<string> _activator;
        private readonly IBrowseServices<EndpointModel> _browse;
        private readonly INodeServices<EndpointModel> _nodes;
        private readonly IPublishServices<EndpointModel> _publisher;
        private readonly IDiscoveryServices _discover;
    }
}
