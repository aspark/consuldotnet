using System;
using System.Collections.Generic;
using System.Text;

namespace Consul
{
    public static class Consts
    {
        public static class Names
        {
            public const string Address = "Address";

            public const string Datacenter = "Datacenter";

            public const string ID = "ID";

            /// <summary>
            /// In, Not In, Is Empty, Is Not Empty
            /// </summary>
            public const string NodeMeta = "NodeMeta";

            public const string Node = "Node";

            public const string TaggedAddresses = "TaggedAddresses";

            public const string Service = "Service";

            public const string ServiceID = "ServiceID";

            public const string ServiceKind = "ServiceKind";

            /// <summary>
            /// In, Not In, Is Empty, Is Not Empty
            /// </summary>
            public const string ServiceMeta = "ServiceMeta";

            public const string ServiceName = "ServiceName";

            public const string ServicePort = "ServicePort";

            /// <summary>
            /// In, Not In, Is Empty, Is Not Empty
            /// </summary>
            public const string ServiceTags = "ServiceTags";
        }
    }
}
