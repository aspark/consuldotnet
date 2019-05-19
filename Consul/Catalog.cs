// -----------------------------------------------------------------------
//  <copyright file="Catalog.cs" company="PlayFab Inc">
//    Copyright 2015 PlayFab Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//  </copyright>
// -----------------------------------------------------------------------

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Consul
{
    public class Node
    {
        //public Guid ID { get; set; }

        // Cannot be "Node" as in the Go API because in C#, properties cannot
        // have the same name as their enclosing class.
        [JsonProperty(PropertyName = "Node")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Datacenter { get; set; }
        public Dictionary<string, string> TaggedAddresses { get; set; }
        public Dictionary<string, string> Meta { get; set; }
    }

    public class CatalogService
    {
        //public Guid ID { get; set; }

        /// <summary>
        /// Node is the name of the Consul node on which the service is registered
        /// </summary>
        public string Node { get; set; }

        /// <summary>
        /// Address is the IP address of the Consul node on which the service is registered.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Datacenter is the data center of the Consul node on which the service is registered.
        /// </summary>
        public string Datacenter { get; set; }

        /// <summary>
        /// TaggedAddresses is the list of explicit LAN and WAN IP addresses for the agent
        /// </summary>
        public IDictionary<string, string> TaggedAddresses { get; set; }

        /// <summary>
        /// NodeMeta is a list of user-defined metadata key/value pairs for the node
        /// </summary>
        public IDictionary<string, string> NodeMeta { get; set; }

        /// <summary>
        /// CreateIndex is an internal index value representing when the service was created
        /// </summary>
        public string CreateIndex { get; set; }

        /// <summary>
        /// ModifyIndex is the last index that modified the service
        /// </summary>
        public string ModifyIndex { get; set; }

        /// <summary>
        /// ServiceID is a unique service instance identifier
        /// </summary>
        public string ServiceID { get; set; }

        /// <summary>
        /// ServiceName is the name of the service
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// ServiceAddress is the IP address of the service host — if empty, node address should be used
        /// </summary>
        public string ServiceAddress { get; set; }

        /// <summary>
        /// ServicePort is the port number of the service
        /// </summary>
        public int ServicePort { get; set; }

        /// <summary>
        /// ServiceTags is a list of tags for the service
        /// </summary>
        public string[] ServiceTags { get; set; }

        /// <summary>
        /// ServiceKind is the kind of service, usually "". See the Agent registration API for more information.
        /// </summary>
        public string ServiceKind { get; set; }

        /// <summary>
        /// ServiceEnableTagOverride indicates whether service tags can be overridden on this service
        /// </summary>
        public bool ServiceEnableTagOverride { get; set; }

        /// <summary>
        /// ServiceMeta is a list of user-defined metadata key/value pairs for the service
        /// </summary>
        public IDictionary<string, string> ServiceMeta { get; set; }

        ///// <summary>
        ///// ServiceProxy is the proxy config as specified in Connect Proxies.
        ///// </summary>
        //public IDictionary<string, string> ServiceProxy { get; set; }//todo:use class

        ///// <summary>
        ///// ServiceConnect are the Connect settings. The value of this struct is equivalent to the Connect field for service registration.
        ///// </summary>
        //public IDictionary<string, string> ServiceConnect { get; set; }//todo:use class


    }

    public class NodeService
    {
        public string ID { get; set; }
        public string Service { get; set; }
        public string[] Tags { get; set; }
        public int Port { get; set; }
        public string Address { get; set; }
        public IDictionary<string, string> Meta { get; set; }


    }

    public class CatalogNode
    {
        public Node Node { get; set; }
        public Dictionary<string, AgentService> Services { get; set; }

        public CatalogNode()
        {
            Services = new Dictionary<string, AgentService>();
        }
    }

    public class CatalogRegistration
    {
        public string Node { get; set; }
        public string Address { get; set; }
        public string Datacenter { get; set; }
        public AgentService Service { get; set; }
        public AgentCheck Check { get; set; }

        /// <summary>
        /// SkipNodeUpdate (bool: false) - Specifies whether to skip updating the node's information in the registration. 
        /// This is useful in the case where only a health check or service entry on a node needs to be updated or when a register request is intended to update a service entry or health check. In both use cases, node information will not be overwritten, if the node is already registered. 
        /// Note, if the paramater is enabled for a node that doesn't exist, it will still be created.
        /// </summary>
        public bool SkipNodeUpdate { get; set; }
    }

    public class CatalogDeregistration
    {
        public string Node { get; set; }
        public string Address { get; set; }
        public string Datacenter { get; set; }
        public string ServiceID { get; set; }
        public string CheckID { get; set; }
    }

    /// <summary>
    /// Catalog can be used to query the Catalog endpoints
    /// </summary>
    public class Catalog : ICatalogEndpoint
    {
        private readonly ConsulClient _client;

        internal Catalog(ConsulClient c)
        {
            _client = c;
        }

        /// <summary>
        /// Register a new catalog item
        /// </summary>
        /// <param name="reg">A catalog registration</param>
        /// <returns>An empty write result</returns>
        public Task<WriteResult> Register(CatalogRegistration reg, CancellationToken ct = default(CancellationToken))
        {
            return Register(reg, WriteOptions.Default, ct);
        }

        /// <summary>
        /// Register a new catalog item
        /// </summary>
        /// <param name="reg">A catalog registration</param>
        /// <param name="q">Customized write options</param>
        /// <returns>An empty write result</returns>
        public Task<WriteResult> Register(CatalogRegistration reg, WriteOptions q, CancellationToken ct = default(CancellationToken))
        {
            return _client.Put("/v1/catalog/register", reg, q).Execute(ct);
        }

        /// <summary>
        /// Deregister an existing catalog item
        /// </summary>
        /// <param name="reg">A catalog deregistration</param>
        /// <returns>An empty write result</returns>
        public Task<WriteResult> Deregister(CatalogDeregistration reg, CancellationToken ct = default(CancellationToken))
        {
            return Deregister(reg, WriteOptions.Default, ct);
        }

        /// <summary>
        /// Deregister an existing catalog item
        /// </summary>
        /// <param name="reg">A catalog deregistration</param>
        /// <param name="q">Customized write options</param>
        /// <returns>An empty write result</returns>
        public Task<WriteResult> Deregister(CatalogDeregistration reg, WriteOptions q, CancellationToken ct = default(CancellationToken))
        {
            return _client.Put("/v1/catalog/deregister", reg, q).Execute(ct);
        }

        /// <summary>
        /// Datacenters is used to query for all the known datacenters
        /// </summary>
        /// <returns>A list of datacenter names</returns>
        public Task<QueryResult<string[]>> Datacenters(CancellationToken ct = default(CancellationToken))
        {
            return _client.Get<string[]>("/v1/catalog/datacenters").Execute(ct);
        }

        /// <summary>
        /// Nodes is used to query all the known nodes
        /// </summary>
        /// <returns>A list of all nodes</returns>
        public Task<QueryResult<Node[]>> Nodes(CancellationToken ct = default(CancellationToken))
        {
            return Nodes(QueryOptions.Default, ct);
        }

        /// <summary>
        /// Nodes is used to query all the known nodes
        /// </summary>
        /// <param name="q">Customized query options</param>
        /// <param name="ct">Cancellation token for long poll request. If set, OperationCanceledException will be thrown if the request is cancelled before completing</param>
        /// <returns>A list of all nodes</returns>
        public Task<QueryResult<Node[]>> Nodes(QueryOptions q, CancellationToken ct = default(CancellationToken))
        {
            return _client.Get<Node[]>("/v1/catalog/nodes", q).Execute(ct);
        }

        /// <summary>
        /// Services is used to query for all known services
        /// </summary>
        /// <returns>A list of all services</returns>
        public Task<QueryResult<Dictionary<string, string[]>>> Services(CancellationToken ct = default(CancellationToken))
        {
            return Services(QueryOptions.Default, ct);
        }

        /// <summary>
        /// Services is used to query for all known services
        /// </summary>
        /// <param name="q">Customized query options</param>
        /// <param name="ct">Cancellation token for long poll request. If set, OperationCanceledException will be thrown if the request is cancelled before completing</param>
        /// <returns>A list of all services</returns>
        public Task<QueryResult<Dictionary<string, string[]>>> Services(QueryOptions q, CancellationToken ct = default(CancellationToken))
        {
            return _client.Get<Dictionary<string, string[]>>("/v1/catalog/services", q).Execute(ct);
        }

        /// <summary>
        /// Service is used to query catalog entries for a given service
        /// </summary>
        /// <param name="service">The service ID</param>
        /// <param name="ct">Cancellation token for long poll request. If set, OperationCanceledException will be thrown if the request is cancelled before completing</param>
        /// <returns>A list of service instances</returns>
        public Task<QueryResult<CatalogService[]>> Service(string service, CancellationToken ct = default(CancellationToken))
        {
            return Service(service, string.Empty, QueryOptions.Default, ct);
        }

        /// <summary>
        /// Service is used to query catalog entries for a given service
        /// </summary>
        /// <param name="service">The service ID</param>
        /// <param name="tag">A tag to filter on</param>
        /// <param name="ct">Cancellation token for long poll request. If set, OperationCanceledException will be thrown if the request is cancelled before completing</param>
        /// <returns>A list of service instances</returns>
        public Task<QueryResult<CatalogService[]>> Service(string service, string tag, CancellationToken ct = default(CancellationToken))
        {
            return Service(service, tag, QueryOptions.Default, ct);
        }

        /// <summary>
        /// Service is used to query catalog entries for a given service
        /// </summary>
        /// <param name="service">The service ID</param>
        /// <param name="tag">A tag to filter on</param>
        /// <param name="q">Customized query options</param>
        /// <param name="ct">Cancellation token for long poll request. If set, OperationCanceledException will be thrown if the request is cancelled before completing</param>
        /// <returns>A list of service instances</returns>
        public Task<QueryResult<CatalogService[]>> Service(string service, string tag, QueryOptions q, CancellationToken ct)
        {
            var req = _client.Get<CatalogService[]>(string.Format("/v1/catalog/service/{0}", service), q);
            if (!string.IsNullOrEmpty(tag))
            {
                req.Params["tag"] = tag;
            }
            return req.Execute(ct);
        }

        /// <summary>
        /// Service is used to query catalog entries for a given service
        /// </summary>
        /// <param name="service">The service ID</param>
        /// <param name="q">Customized query options</param>
        /// <param name="ct">Cancellation token for long poll request. If set, OperationCanceledException will be thrown if the request is cancelled before completing</param>
        /// <returns>A list of service instances</returns>
        public Task<QueryResult<CatalogService[]>> Service(string service, FilterOptions q, CancellationToken ct)
        {
            var req = _client.Get<CatalogService[]>(string.Format("/v1/catalog/service/{0}", service), q);

            return req.Execute(ct);
        }

        /// <summary>
        /// Node is used to query for service information about a single node
        /// </summary>
        /// <param name="node">The node name</param>
        /// <param name="ct">Cancellation token for long poll request. If set, OperationCanceledException will be thrown if the request is cancelled before completing</param>
        /// <returns>The node information including a list of services</returns>
        public Task<QueryResult<CatalogNode>> Node(string node, CancellationToken ct = default(CancellationToken))
        {
            return Node(node, QueryOptions.Default, ct);
        }

        /// <summary>
        /// Node is used to query for service information about a single node
        /// </summary>
        /// <param name="node">The node name</param>
        /// <param name="q">Customized query options</param>
        /// <param name="ct">Cancellation token for long poll request. If set, OperationCanceledException will be thrown if the request is cancelled before completing</param>
        /// <returns>The node information including a list of services</returns>
        public Task<QueryResult<CatalogNode>> Node(string node, QueryOptions q, CancellationToken ct = default(CancellationToken))
        {
            return _client.Get<CatalogNode>(string.Format("/v1/catalog/node/{0}", node), q).Execute(ct);
        }

        /// <summary>
        /// Node is used to query for service information about a single node
        /// </summary>
        /// <param name="node">The node name</param>
        /// <param name="q">Customized query options</param>
        /// <param name="ct">Cancellation token for long poll request. If set, OperationCanceledException will be thrown if the request is cancelled before completing</param>
        /// <returns>The node information including a list of services</returns>
        public Task<QueryResult<CatalogNode>> Node(string node, FilterOptions q, CancellationToken ct = default(CancellationToken))
        {
            return _client.Get<CatalogNode>(string.Format("/v1/catalog/node/{0}", node), q).Execute(ct);
        }
    }

    public partial class ConsulClient : IConsulClient
    {
        private Lazy<Catalog> _catalog;

        /// <summary>
        /// Catalog returns a handle to the catalog endpoints
        /// </summary>
        public ICatalogEndpoint Catalog
        {
            get { return _catalog.Value; }
        }
    }
}
