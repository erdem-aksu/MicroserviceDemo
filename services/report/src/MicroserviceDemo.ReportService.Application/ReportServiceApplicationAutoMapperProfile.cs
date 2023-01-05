using AutoMapper;
using MicroserviceDemo.ReportService.Reports;

namespace MicroserviceDemo.ReportService;

public class ReportServiceApplicationAutoMapperProfile : Profile
{
    public ReportServiceApplicationAutoMapperProfile()
    {
        CreateMap<Report, ReportDto>().MapExtraProperties();
        CreateMap<Report, ReportListDto>();
    }
}