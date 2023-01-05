using System;
using Volo.Abp.Domain.Entities.Events.Distributed;

namespace MicroserviceDemo.ReportService.Reports
{
    [Serializable]
    public class ReportRequestedEto : EtoBase
    {
        public Guid ReportId { get; set; }

        public string Location { get; set; }
    }
}