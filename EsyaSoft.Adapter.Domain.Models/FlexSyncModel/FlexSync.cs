using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.Domain.Models
{
    public class MTFlexSyncAMIReqRoot
    {
        public MT_FlexSyncAMI_Req MT_FlexSyncAMI_Req { get; set; }
    }
    public class MT_FlexSyncAMI_Req
        {
            public SDPSyncMessage SDPSyncMessage { get; set; }
        }

        public class SDPSyncMessage
        {
            public Header Header { get; set; }
            public Payload Payload { get; set; }
        }

        public class Header
        {
            public string Verb { get; set; }
            public string Noun { get; set; }
            public int Revision { get; set; }
            public DateTime DateTime { get; set; }
            public string Source { get; set; }
            public string MessageID { get; set; }
            public string AsyncReplyTo { get; set; }
            public string SyncMode { get; set; }
        }

        public class Payload
        {
            public Account Account { get; set; }
            public ServicePoint ServicePoint { get; set; }
            public Consumer Consumer { get; set; }
            public ConsumerAddress ConsumerAddress { get; set; }
            public ServicePointGroup ServicePointGroup { get; set; }
            public ServicePointServicePointGroupAssociation ServicePointServicePointGroupAssociation { get; set; }
            public AccountServicePointAssociation AccountServicePointAssociation { get; set; }
            public ConsumerAccountAssociation ConsumerAccountAssociation { get; set; }
            public ConsumerConsumerAddressAssociation ConsumerConsumerAddressAssociation { get; set; }
        }

        public class Account
        {
            public string MRID { get; set; }
            public string Name { get; set; }
            public string AccountType { get; set; }
            public string AccountClass { get; set; }
            public string Status { get; set; }
            public List<Parameter> Parameter { get; set; }
        }

        public class ServicePoint
        {
            public string MRID { get; set; }
            public string ClassName { get; set; }
            public string Type { get; set; }
            public string Status { get; set; }
            public PremiseId PremiseId { get; set; }
            public string LocInfo { get; set; }
            public List<Parameter> Parameter { get; set; }
        }

        public class PremiseId
        {
            public string MRID { get; set; }
            public string Description { get; set; }
        }

        public class Consumer
        {
            public string MRID { get; set; }
            public string Description { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Status { get; set; }
        }

        public class ConsumerAddress
        {
            public string MRID { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string City { get; set; }
            public string StateOrProvince { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
            public string Timezone { get; set; }
        }

        public class ServicePointGroup
        {
            public string MRID { get; set; }
            public string Name { get; set; }
            public string RouteType { get; set; }
            public string RouteSubType { get; set; }
            public string Status { get; set; }
            public List<Parameter> Parameter { get; set; }
        }

        public class ServicePointServicePointGroupAssociation
        {
            public DateTime StartDate { get; set; }
            public ServicePointId ServicePointId { get; set; }
            public ServicePointGroupId ServicePointGroupId { get; set; }
        }

        public class ServicePointId
        {
            public string MRID { get; set; }
            public string Type { get; set; }
        }

        public class ServicePointGroupId
        {
            public string MRID { get; set; }
            public string Type { get; set; }
        }

        public class AccountServicePointAssociation
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public AccountId AccountId { get; set; }
            public ServicePointId ServicePointId { get; set; }
        }

        public class AccountId
        {
            public string MRID { get; set; }
            public string Description { get; set; }
        }

        public class ConsumerAccountAssociation
        {
            public ConsumerId ConsumerId { get; set; }
            public AccountId AccountId { get; set; }
            public string Status { get; set; }
            public string RelType { get; set; }
        }

        public class ConsumerId
        {
            public string MRID { get; set; }
            public string Description { get; set; }
        }

        public class ConsumerConsumerAddressAssociation
        {
            public ConsumerId ConsumerId { get; set; }
            public ConsumerAddressId ConsumerAddressId { get; set; }
            public string Status { get; set; }
            public string AddressRole { get; set; }
        }

        public class ConsumerAddressId
        {
            public string MRID { get; set; }
        }

        public class Parameter
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public DateTime StartDate { get; set; }
        }
    }
