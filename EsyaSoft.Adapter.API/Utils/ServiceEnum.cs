using EsyaSoft.Adapter.API.Controllers;
using System.ComponentModel;

namespace EsyaSoft.Adapter.API.Utils
{
    enum ServiceEnum
    {
        /*
            Deployed and Tested by Adani Team
            1.1.1	>> Device Create (Individual)
            1.1.2	>> Device Create Confirmation (Individual)
            1.2.1	>> Location Notification (Individual)
            1.2.2	>> Location Notification (Bulk)
            1.3.1	>> Register Create (Individual)
            1.3.2	>> Register Create Confirmation (Individual)

            On Development
            3.2	33	>> Meter Read Bulk Request [Dev done and Tested in Local]
            3.4	35	>> Meter Read Bulk Confirmation [Dev In Progress, Should be tested together, 1st Bulk work]

        */
        //1.1.1	OUT 1	Single  Meter Create    Device Create (Individual)
        [Description("Device Create (Individual) - 1.1.1")]
        DeviceCreateSingle = 1,

        //1.1.3	OUT 3	Bulk    Meter Create Bulk   Device Create (Bulk)
        [Description("Device Create (Bulk) - 1.1.3")]
        DeviceCreateBulk = 3,

        [Description("Device Create Confirmation (Individual) - 1.1.2")]
        DeviceCreateConfirmationSingle = 2,

        //1.1.7	OUT 7	Single	Device Change (Single)
        [Description("Device Change (Single) - 1.1.7")]
        DeviceChangeSingle = 7,

        //1.1.8	- IN 8	Single	- Meter Change - Device Change Confirmation (Single)
        [Description("Device Change Confirmation (Single) - 1.1.8")]
        DeviceChangeConfirmationSingle = 8,
        //***************
        //1.1.9 - IN 9 Bulk	Meter Change	Device Change (Bulk)
        [Description("Device Change (Bulk) - 1.1.9")]
        DeviceChangeBulk = 9,

        //1.1.10 - OUT - 10	Bulk	Meter Change	Device Change Confirmation (Bulk)
        [Description("Device Change Confirmation (Bulk) - 1.1.10")]
        DeviceChangeConfirmationBulk =10,
        //*******************

        //1.2.1	OUT 11	Single  Meter Installation/ Meter Removal   Location Notification (Individual)
        [Description("Location Notification (Individual) - 1.2.1")]
        LocationNotificationSingle = 11,
        
        //1.2.2	12	Bulk    Meter Replacement   Location Notification (Bulk)
        [Description("Location Notification (Bulk) - 1.2.2")]
        LocationNotificationBulk  = 12,

        //1.3.1	OUT 13	Single	Meter Installation/Replacement	Register Create (Individual)
        [Description("Register Create (Individual) - 1.3.1")]
        RegisterCreateSingle = 13,

        //1.3.2	IN 14	Single	Meter Installation/Replacement	Register Create Confirmation (Individual)
        [Description("Register Create Confirmation (Individual) - 1.3.2")]
        RegisterCreateConfirmationSingle = 14,

        //1.3.3	OUT 15	Bulk	Meter Installation/Replacement	Register Create (Bulk)
        [Description("Register Create (BULK) - 1.3.3")]
        RegisterCreateBulk = 15,

        //1.3.4	IN 16	Bulk	Meter Installation/Replacement  Register Create Confirmation (Bulk)
        [Description("Register Create Confirmation (Bulk) - 1.3.4")]
        RegisterCreateConfirmationBulk = 16,

        //************** Change **************************
        //1.3.5	17	OUT Single	Meter Removal Register Change (Individual)
        [Description("Register Change (Individual) - 1.3.5")]
        RegisterChangeSingle = 17,

        //1.3.6	18	IN Single	Meter Remova Register Change Confirmation (Individual)
        [Description("Register Change Confirmation (Individual) - 1.3.6")]
        RegisterChangeConfirmationSingle = 18,

        //1.3.7	19	OUT Bulk	Meter Removal/ Replacement Register Change (Bulk)
        [Description("Register Change (Bulk) - 1.3.7")]
        RegisterChangeBulk = 19,

        //1.3.8	20	IN Bulk	Meter Removal/ Replacement Register Change Confirmation (Bulk)
        [Description("Register Change Confirmation (Bulk) - 1.3.8")]
        RegisterChangeConfirmationBulk = 20,


        //********************* Measurement CTPT ************
        //1.4.1 - OUT 21 Single Measurement Task Assignment for CT/PT (Individual)
        [Description("Measurement Task Assignment for CT/PT (Individual) - 1.4.1")]
        MeasurementCTPTSingle = 21,

        //********************* Change ***********************
        //1.5.1	OUT 22	Bulk        Replication Device (Bulk)
        [Description("Replication Device (Bulk) - 1.5.1")]
        ReplicationDeviceBulk = 22,

        //1.5.2	OUT 23	Bulk        Replication Device (Bulk) Confirmation
        [Description("Replication Device Confirmation (Bulk) - 1.5.2")]
        ReplicationDeviceBulkConfirmation = 23,


        [Description("Register Create Confirmation (Individual) - 1.3.2")]
        RegisterCreateConfirmationIndividual = 6,

        //3.2 OUT	33	Bulk    Billing Meter read Request/On Demand Meter read request Meter Read Bulk Request
        [Description("Meter Read Bulk Request - 3.2")]
        MeterReadBulkRequest = 33,

        //3.1 OUT	32	Single	Billing Meter read Request/ On Demand Meter Read Request	Meter Read Request
        [Description("Meter Read Single Request - 3.1")]
        MeterReadSingleRequest = 32,

        //3.4	35	Bulk    Meter Read Bulk Confirmation	"Meter Read Bulk Confirmation" Confirmation is sent to SAP indicating that the meter read request was valid and will be processed."
        //Pair to Enum 3
        [Description("Meter Read Bulk Confirmation - 3.4")]
        MeterReadBulkConfirmation = 35,

        //3.3	34	Single	Billing Meter read Request	Meter Read Confirmation
        //Pair to Enum 32
        [Description("Meter Read Confirmation Single - 3.3")]
        MeterReadSingleConfirmation = 34,
        

        //3.6	37	Bulk	Billing Meter read Request/On Demand Meter read request	Meter Read Bulk Result
        [Description("Meter Read Bulk Result - 3.7")]
        MeterReadBulkResult = 37,

        //3.5	36	Single	Billing Meter read Request	Meter Read Result
        [Description("Meter Read Single Result - 3.5")]
        MeterReadSingleResult = 36,

        //3.7 OUT 38	Single  Billing Meter read Request  Meter Read SAP Result Confirmation
        [Description("Billing Meter read Request Single - 3.7")]
        BillingMeterReadRequestSingle = 38,

        //3.8	OUT 39	Bulk	Billing Meter read Request/On Demand Meter read request	Meter Read SAP Bulk Result Confirmation
        [Description("Billing Meter read Request Bulk - 3.8")]
        BillingMeterReadRequestBulk = 39,

        //2.2 IN 25	Single	Connection/Disconnection Request	Connect/Disconnect Confirmation
        [Description("Connect-Disconnect Request Confirmation Single - 2.1")]
        ConnectionDisconnectionRequestConfirmationSingle = 25,

        //2.1 OUT	24	Single  Connection/Disconnection Request    Connect/Disconnect
        [Description("Connection-Disconnection Request Single - 2.1")]
        ConnectionDisconnectionRequestSingle = 24,

        //2.3 OUT 26	Bulk    Connection/Disconnection Request    Connect/Disconnect Bulk
        [Description("Connection-Disconnection Request Bulk - 2.3")]
        ConnectionDisconnectionRequestBulk = 26,

        //2.5 OUT 28	Single  Connection/Disconnection cancellation   Connect/Disconnect Cancellation
        [Description("Connection-Disconnection cancellation - 2.5")]
        ConnectionDisconnectioncancellationRequest = 28,

        //2.7 OUT	30	Single  Operational status  Operational Status Check Request
        [Description("Operational Status Check Request Single - 2.7")]
        OperationalStatusCheckRequestSingle = 30,


        //3.9 OUT	40	Single	Meter Read Cancel	Meter Read Cancellation
        [Description("Manual Meter Reads Create Bulk - 3.9")]
        MeterReadCancelSingle = 40,

        //3.11 IN	42	SINGLE Meter Read Cancellation Confirmation
        [Description("Meter Read Cancellation Confirmation Single - 3.11")]
        MeterReadCancellationConfirmationSingle = 42,

        //3.10 OUT 41	Bulk	Meter Read Cancel	Meter Read Bulk Cancellation
        [Description("Manual Meter Reads Create Single - 3.10")]
        MeterReadCancelBulk = 41,

        //3.12	IN 43 Meter Read Bulk Cancellation Confirmation
        [Description("Meter Read Bulk Cancellation Confirmation - 3.10")]
        MeterReadBulkCancellationConfirmationBulk = 43,

        //3.13 OUT	Bulk    Manual Meter Reads from SAP Bulk Manual Meter Reads Create
        [Description("Manual Meter Reads Create Bulk - 3.13")]
        ManualMeterReadsCreateBulk = 44,

        //3.14 OUT	Single  Manual Meter Reads from SAP Manual Meter Reads Create
        [Description("Manual Meter Reads Create Single - 3.14")]
        ManualMeterReadsCreateSingle = 45,

        //3.15	46	Bulk Manual Meter Reads Create Confirmation 
        [Description("Bulk Manual Meter Reads Create Confirmation - 3.15")]
        ManualMeterReadsCreateConfirmationBulk = 46,

        //3.16	47	Single	Manual Meter Reads Create Confirmation
        [Description("Manual Meter Reads Create Single - 3.14")]
        ManualMeterReadsCreateConfirmationSingle = 47,



        //Other than SAP ENUMs
        [Description("Read FlexSync Request")]
        ReadFlexSyncRequest = 1001


    }
}
