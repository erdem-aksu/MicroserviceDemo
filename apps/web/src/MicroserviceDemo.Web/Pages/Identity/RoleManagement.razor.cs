using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroserviceDemo.AdministrationService.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace MicroserviceDemo.Web.Pages.Identity
{
    public partial class RoleManagement
    {
        [Inject]
        protected IIdentityRoleAppService RoleAppService { get; set; }

        protected const string PermissionProviderName = "R";

        private MudDataGrid<IdentityRoleDto> Grid { get; set; }
        private PagedResultDto<IdentityRoleDto> GridData { get; set; } = new();

        private bool IsGridLoading { get; set; }

        private IdentityRoleCreateDto NewEntity { get; set; } = new();
        private IdentityRoleUpdateDto EditingEntity { get; set; } = new();
        private Guid EditingEntityId { get; set; }

        private MudDialog CreateModal { get; set; }
        private MudDialog EditModal { get; set; }
        private PermissionManagementModal PermissionManagementModal { get; set; }

        private EditForm CreateForm { get; set; }
        private EditForm EditForm { get; set; }

        private bool HasCreatePermission { get; set; }
        private bool HasUpdatePermission { get; set; }
        private bool HasDeletePermission { get; set; }
        private bool HasManagePermissionsPermission { get; set; }

        private string SearchText { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await InvokeAsync(StateHasChanged);
        }

        protected override ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems = new List<BreadcrumbItem>
            {
                new(L["Menu:Home"], "/"),
                new(L["Menu:IdentityManagement"], null, true),
                new(L["Menu:IdentityManagement.Roles"], "/roles", true)
            };

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            HasCreatePermission = await AuthorizationService.IsGrantedAsync(AdministrationServicePermissions.Identity.Roles.Create);
            HasUpdatePermission = await AuthorizationService.IsGrantedAsync(AdministrationServicePermissions.Identity.Roles.Update);
            HasDeletePermission = await AuthorizationService.IsGrantedAsync(AdministrationServicePermissions.Identity.Roles.Delete);
            HasManagePermissionsPermission = await AuthorizationService.IsGrantedAsync(AdministrationServicePermissions.Identity.Roles.ManagePermissions);
        }

        private async Task OpenCreateModalAsync()
        {
            try
            {
                NewEntity = new IdentityRoleCreateDto();

                CreateModal?.Show(L["NewRole"]);
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
                if (CreateForm.EditContext?.Validate() ?? false)
                {
                    await RoleAppService.CreateAsync(NewEntity);

                    CreateModal.Close();
                    await Grid.ReloadServerData();
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task UpdateEntityAsync()
        {
            try
            {
                if (EditForm.EditContext?.Validate() ?? false)
                {
                    await RoleAppService.UpdateAsync(EditingEntityId, EditingEntity);

                    EditModal.Close();
                    await Grid.ReloadServerData();
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task DeleteEntityAsync(IdentityRoleDto entity)
        {
            try
            {
                if (await Message.Confirm(L["RoleDeletionConfirmationMessage", entity.Name]))
                {
                    await RoleAppService.DeleteAsync(entity.Id);

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
        }

        private async Task OpenEditModalAsync(IdentityRoleDto entity)
        {
            try
            {
                var entityDto = await RoleAppService.GetAsync(entity.Id);

                EditingEntityId = entity.Id;
                EditingEntity = ObjectMapper.Map<IdentityRoleDto, IdentityRoleUpdateDto>(entityDto);

                await InvokeAsync(
                    () =>
                    {
                        StateHasChanged();
                        EditModal?.Show(L["Edit"] + " - " + EditingEntity.Name);
                    }
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void CloseEditModal()
        {
            EditModal.Close();
        }

        private async Task OpenPermissionsModalAsync(IdentityRoleDto role)
        {
            await PermissionManagementModal.OpenAsync(PermissionProviderName, role.Id.ToString());
        }

        private async Task<GridData<IdentityRoleDto>> ReadGridData(GridState<IdentityRoleDto> gridState)
        {
            IsGridLoading = true;

            try
            {
                GridData = await RoleAppService.GetListAsync(
                    new GetIdentityRolesInput
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

            return new GridData<IdentityRoleDto>
            {
                Items = GridData.Items,
                TotalItems = (int) GridData.TotalCount
            };
        }
    }
}