// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.OpcUa.Services.Gateway.v1.Controllers {
    using Microsoft.Azure.IIoT.OpcUa.Services.Gateway.v1.Models;
    using Microsoft.Azure.IIoT.Hub;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Status api controller
    /// </summary>
    [Route(VersionInfo.PATH + "/status")]
    [Produces(ContentEncodings.MimeTypeJson)]
    public class StatusController : Controller {

        /// <summary>
        /// Create controller
        /// </summary>
        /// <param name="hub"></param>
        public StatusController(IIoTHubTwinServices hub) {
            _hub = hub;
        }

        /// <summary>
        /// Return the service status in the form of the service status
        /// api model.
        /// </summary>
        /// <returns>Status object</returns>
        [HttpGet]
        public StatusResponseApiModel Get() {
            // TODO: check if the dependencies are healthy
            return new StatusResponseApiModel(true, "Alive and well");
        }

        private readonly IIoTHubTwinServices _hub;
    }
}
