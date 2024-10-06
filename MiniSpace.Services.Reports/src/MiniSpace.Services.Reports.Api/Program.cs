using System.Collections.Generic;
using System.Threading.Tasks;
using Paralax;
using Paralax.Logging;
using Paralax.WebApi;
using Microsoft.AspNetCore;
using MiniSpace.Services.Reports.Application;
using MiniSpace.Services.Reports.Application.Commands;
using MiniSpace.Services.Reports.Application.DTO;
using MiniSpace.Services.Reports.Application.Queries;
using MiniSpace.Services.Reports.Application.Services;
using MiniSpace.Services.Reports.Core.Wrappers;
using MiniSpace.Services.Reports.Infrastructure;
using Paralax.CQRS.WebApi;
using Paralax.Core;

namespace MiniSpace.Services.Reports.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddParalax()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        
                        .Get<SearchReports, PagedResponse<ReportDto>>("reports/search")
                        
                        .Post<CreateReport>("reports", afterDispatch: (cmd, ctx) 
                            => ctx.Response.Created($"reports/{cmd.ReportId}"))
                        .Delete<DeleteReport>("reports/{reportId}")
                        .Post<CancelReport>("reports/{reportId}/cancel")
                        .Post<StartReportReview>("reports/{reportId}/start-review")
                        .Post<ResolveReport>("reports/{reportId}/resolve")
                        .Post<RejectReport>("reports/{reportId}/reject")
                        .Get<GetUserReports, PagedResponse<ReportDto>>("reports/students/{studentId}")
                        ))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
