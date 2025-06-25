using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsyaSoft.Adapter.Domain.Models
{
    //internal class ConfirmationClass
    //{
    //}
    public class GenericConfirmationBulk
    {
        public GenericConfirmationHeader MessageHeader { get; set; }
        public List<GenericConfirmationSingle> ChildMessage { get; set; }
        public GenericConfirmationLog Log { get; set; }
    }

    public class GenericConfirmationSingle
    {
        public GenericConfirmationHeader MessageHeader { get; set; }
        public GenericConfirmationUtilitiesDevice UtilitiesDevice { get; set; }
        public GenericConfirmationLog Log { get; set; }
    }
    public class GenericConfirmationUtilitiesDevice
    {
        public string? ID { get; set; }
    }
    public class GenericConfirmationHeader
    {
        public Guid? UUID { get; set; }
        public Guid? ReferenceUUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string? RecipientBusinessSystemID { get; set; }
    }
    public class GenericConfirmationLog
    {
        public int BusinessDocumentProcessingResultCode { get; set; }
        public int MaximumLogItemSeverityCode { get; set; }
        public GenericItem Item { get; set; }

    }
    public class GenericItem
    {
        public string? TypeID { get; set; }
        public int SeverityCode { get; set; }
        public string? Note { get; set; }
    }
}
