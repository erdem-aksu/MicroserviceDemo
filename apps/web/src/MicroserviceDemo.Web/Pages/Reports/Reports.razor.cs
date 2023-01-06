using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MicroserviceDemo.AdministrationService.Blob;
using MicroserviceDemo.ReportService;
using MicroserviceDemo.ReportService.Permissions;
using MicroserviceDemo.ReportService.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Volo.Abp.Application.Dtos;

namespace MicroserviceDemo.Web.Pages.Reports
{
    [Authorize(ReportServicePermissions.Reports.Default)]
    public partial class Reports : IAsyncDisposable
    {
        [Inject]
        protected IReportAppService ReportAppService { get; set; }

        [Inject]
        protected IFileAppService FileAppService { get; set; }

        [Inject]
        private IValidator<ReportCreateDto> ReportCreateDtoValidator { get; set; }

        [Inject]
        private IJSRuntime JS { get; set; }

        private IJSObjectReference JsModule;

        private MudDataGrid<ReportListDto> Grid { get; set; }
        public PagedResultDto<ReportListDto> GridData { get; set; } = new();

        private bool IsGridLoading { get; set; }

        private ReportCreateDto NewEntity { get; set; } = new();

        private MudDialog CreateModal { get; set; }
        private bool IsCreateModalVisible { get; set; }

        private MudForm CreateForm { get; set; }

        private bool HasCreatePermission { get; set; }
        private bool HasDeletePermission { get; set; }

        private string SearchText { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await InvokeAsync(StateHasChanged);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                JsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/Reports/Reports.razor.js");
            }
        }

        protected override ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems = new List<BreadcrumbItem>
            {
                new(L["Menu:Home"], "/"),
                new(L["Menu:Reports"], "/reports", true)
            };

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            HasCreatePermission = await AuthorizationService.IsGrantedAsync(ReportServicePermissions.Reports.Create);
            HasDeletePermission = await AuthorizationService.IsGrantedAsync(ReportServicePermissions.Reports.Delete);
        }

        private async Task OpenCreateModalAsync()
        {
            try
            {
                CreateForm?.ResetValidation();

                NewEntity = new ReportCreateDto();

                CreateModal?.Show(L["NewReport"]);
                IsCreateModalVisible = true;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CreateEntityAsync()
        {
            try
            {
                await CreateForm.Validate();

                if (CreateForm.IsValid)
                {
                    await ReportAppService.CreateAsync(NewEntity);

                    CreateModal.Close();
                    IsCreateModalVisible = false;
                    await Grid.ReloadServerData();
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task DeleteEntityAsync(ReportListDto entity)
        {
            try
            {
                if (await Message.Confirm(L["DeletionConfirmationMessage", L["Report"], entity.Name]))
                {
                    await ReportAppService.DeleteAsync(entity.Id);

                    await Grid.ReloadServerData();
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void CloseCreateModal()
        {
            CreateModal.Close();
            IsCreateModalVisible = false;
        }

        private async Task<GridData<ReportListDto>> ReadGridData(GridState<ReportListDto> gridState)
        {
            IsGridLoading = true;

            try
            {
                GridData = await ReportAppService.GetListAsync(
                    new GetReportsInput
                    {
                        MaxResultCount = gridState.PageSize,
                        SkipCount = gridState.Page * gridState.PageSize,
                        Sorting = gridState.SortDefinitions.Select(d => d.SortBy + " " + (d.Descending ? "DESC" : "ASC")).JoinAsString(","),
                        Filter = SearchText
                    }
                );
            }
            catch (Exception e)
            {
                await HandleErrorAsync(e);
            }

            IsGridLoading = false;

            return new GridData<ReportListDto>
            {
                Items = GridData.Items,
                TotalItems = (int) GridData.TotalCount
            };
        }

        private async Task DownloadReportAsync(ReportListDto entity)
        {
            if (entity.Status != ReportStatus.Completed)
            {
                return;
            }

            try
            {
                var blob = await FileAppService.GetBlobAsync(
                    new GetBlobRequestDto
                    {
                        Name = entity.Name
                    }
                );

                using var streamRef = new DotNetStreamReference(stream: new MemoryStream(blob.Content));

                await JS.InvokeVoidAsync("downloadFileFromStream", blob.Name, streamRef);
            }
            catch (Exception e)
            {
                await HandleErrorAsync(e);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (JsModule != null)
            {
                await JsModule.DisposeAsync();
            }
        }
    }
}