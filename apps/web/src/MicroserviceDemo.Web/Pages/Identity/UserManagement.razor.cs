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
    public partial class UserManagement
    {
        [Inject]
        protected IIdentityUserAppService UserAppService { get; set; }

        protected const string PermissionProviderName = "U";

        protected IReadOnlyList<IdentityRoleDto> Roles;

        protected AssignedRoleViewModel[] NewUserRoles;

        protected AssignedRoleViewModel[] EditUserRoles;

        private MudDataGrid<IdentityUserDto> Grid { get; set; }
        public PagedResultDto<IdentityUserDto> GridData { get; set; } = new();

        private bool IsGridLoading { get; set; }

        private IdentityUserCreateDto NewEntity { get; set; } = new();
        private IdentityUserUpdateDto EditingEntity { get; set; } = new();
        private Guid EditingEntityId { get; set; }

        private MudDialog CreateModal { get; set; }
        private bool IsCreateModalVisible;

        private MudDialog EditModal { get; set; }
        private bool IsEditModalVisible;

        private PermissionManagementModal PermissionManagementModal { get; set; }

        private EditForm CreateForm { get; set; }
        private EditForm EditForm { get; set; }

        private bool HasCreatePermission { get; set; }
        private bool HasUpdatePermission { get; set; }
        private bool HasDeletePermission { get; set; }
        private bool HasManagePermissionsPermission { get; set; }
        private bool HasImpersonationPermission { get; set; }

        private string SearchText { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();

            try
            {
                Roles = (await UserAppService.GetAssignableRolesAsync()).Items;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }

            await InvokeAsync(StateHasChanged);
        }

        protected override ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems = new List<BreadcrumbItem>
            {
                new(L["Menu:Home"], "/"),
                new(L["Menu:IdentityManagement"], null, true),
                new(L["Menu:IdentityManagement.Users"], "/users", true)
            };

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            HasCreatePermission = await AuthorizationService.IsGrantedAsync(AdministrationServicePermissions.Identity.Users.Create);
            HasUpdatePermission = await AuthorizationService.IsGrantedAsync(AdministrationServicePermissions.Identity.Users.Update);
            HasDeletePermission = await AuthorizationService.IsGrantedAsync(AdministrationServicePermissions.Identity.Users.Delete);
            HasManagePermissionsPermission = await AuthorizationService.IsGrantedAsync(AdministrationServicePermissions.Identity.Users.ManagePermissions);
            HasImpersonationPermission = await AuthorizationService.IsGrantedAsync(AdministrationServicePermissions.Identity.Users.Impersonation);
        }

        private async Task OpenCreateModalAsync()
        {
            try
            {
                NewEntity = new IdentityUserCreateDto();
                NewUserRoles = Roles.Select(
                        x => new AssignedRoleViewModel
                        {
                            Name = x.Name,
                            IsAssigned = x.IsDefault
                        }
                    )
                    .ToArray();

                CreateModal?.Show(L["NewUser"]);
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
                NewEntity.RoleNames = NewUserRoles.Where(x => x.IsAssigned).Select(x => x.Name).ToArray();

                if (CreateForm.EditContext?.Validate() ?? false)
                {
                    await UserAppService.CreateAsync(NewEntity);

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

        private async Task UpdateEntityAsync()
        {
            try
            {
                EditingEntity.RoleNames = EditUserRoles.Where(x => x.IsAssigned).Select(x => x.Name).ToArray();

                if (EditForm.EditContext?.Validate() ?? false)
                {
                    await UserAppService.UpdateAsync(EditingEntityId, EditingEntity);

                    EditModal.Close();
                    IsEditModalVisible = false;

                    await Grid.ReloadServerData();
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task DeleteEntityAsync(IdentityUserDto entity)
        {
            try
            {
                if (await Message.Confirm(L["UserDeletionConfirmationMessage", entity.UserName]))
                {
                    await UserAppService.DeleteAsync(entity.Id);

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

        private async Task OpenEditModalAsync(IdentityUserDto entity)
        {
            try
            {
                var userRoleNames = (await UserAppService.GetRolesAsync(entity.Id)).Items.Select(r => r.Name).ToList();

                EditUserRoles = Roles.Select(
                        x => new AssignedRoleViewModel
                        {
                            Name = x.Name,
                            IsAssigned = userRoleNames.Contains(x.Name)
                        }
                    )
                    .ToArray();

                var entityDto = await UserAppService.GetAsync(entity.Id);

                EditingEntityId = entity.Id;
                EditingEntity = ObjectMapper.Map<IdentityUserDto, IdentityUserUpdateDto>(entityDto);

                await InvokeAsync(
                    () =>
                    {
                        StateHasChanged();
                        EditModal?.Show(L["Edit"] + " - " + EditingEntity.UserName);
                        IsEditModalVisible = true;
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
            IsEditModalVisible = false;
        }

        private async Task OpenPermissionsModalAsync(IdentityUserDto user)
        {
            await PermissionManagementModal.OpenAsync(PermissionProviderName, user.Id.ToString());
        }

        private async Task<GridData<IdentityUserDto>> ReadGridData(GridState<IdentityUserDto> gridState)
        {
            IsGridLoading = true;

            try
            {
                GridData = await UserAppService.GetListAsync(
                    new GetIdentityUsersInput
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

            return new GridData<IdentityUserDto>
            {
                Items = GridData.Items,
                TotalItems = (int) GridData.TotalCount
            };
        }
    }

    public class AssignedRoleViewModel
    {
        public string Name { get; set; }

        public bool IsAssigned { get; set; }
    }
}