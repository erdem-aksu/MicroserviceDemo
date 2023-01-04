using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Localization;

namespace MicroserviceDemo.Web.Pages.Identity
{
    public partial class PermissionManagementModal
    {
        [Inject]
        private IPermissionAppService PermissionAppService { get; set; }

        [Inject]
        private ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; }

        private MudDialog Modal { get; set; }

        private string ProviderName { get; set; }
        private string ProviderKey { get; set; }

        private string EntityDisplayName { get; set; }
        private List<PermissionGroupDto> Groups { get; set; }

        private List<PermissionGrantInfoDto> DisabledPermissions { get; set; } = new();

        private string SelectedTabName { get; set; }

        private int GrantedPermissionCount { get; set; }
        private int NotGrantedPermissionCount { get; set; }

        private bool GrantAll
        {
            get
            {
                if (NotGrantedPermissionCount == 0)
                {
                    return true;
                }

                return false;
            }
            set
            {
                if (Groups == null)
                {
                    return;
                }

                GrantedPermissionCount = 0;
                NotGrantedPermissionCount = 0;

                foreach (var permission in Groups.SelectMany(x => x.Permissions))
                {
                    if (!IsDisabledPermission(permission))
                    {
                        permission.IsGranted = value;

                        if (value)
                        {
                            GrantedPermissionCount++;
                        }
                        else
                        {
                            NotGrantedPermissionCount++;
                        }
                    }
                }
            }
        }

        public PermissionManagementModal()
        {
            LocalizationResource = typeof(AbpPermissionManagementResource);
        }

        public async Task OpenAsync(string providerName, string providerKey, string entityDisplayName = null)
        {
            try
            {
                ProviderName = providerName;
                ProviderKey = providerKey;

                var result = await PermissionAppService.GetAsync(ProviderName, ProviderKey);

                EntityDisplayName = entityDisplayName ?? result.EntityDisplayName;
                Groups = result.Groups;

                GrantedPermissionCount = 0;
                NotGrantedPermissionCount = 0;
                foreach (var permission in Groups.SelectMany(x => x.Permissions))
                {
                    if (permission.IsGranted && permission.GrantedProviders.All(x => x.ProviderName != ProviderName))
                    {
                        DisabledPermissions.Add(permission);
                        continue;
                    }

                    if (permission.IsGranted)
                    {
                        GrantedPermissionCount++;
                    }
                    else
                    {
                        NotGrantedPermissionCount++;
                    }
                }

                SelectedTabName = GetNormalizedGroupName(Groups.First().Name);

                Modal.Show(
                    options: new DialogOptions
                    {
                        MaxWidth = MaxWidth.Medium
                    }
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void CloseModal()
        {
            Modal.Close();
        }

        private async Task SaveAsync()
        {
            try
            {
                var updateDto = new UpdatePermissionsDto
                {
                    Permissions = Groups
                        .SelectMany(g => g.Permissions)
                        .Select(p => new UpdatePermissionDto { IsGranted = p.IsGranted, Name = p.Name })
                        .ToArray()
                };

                await PermissionAppService.UpdateAsync(ProviderName, ProviderKey, updateDto);

                await CurrentApplicationConfigurationCacheResetService.ResetAsync();

                Modal.Close();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private string GetNormalizedGroupName(string name)
        {
            return "PermissionGroup_" + name.Replace(".", "_");
        }

        private void GroupGrantAllChanged(bool value, PermissionGroupDto permissionGroup)
        {
            foreach (var permission in permissionGroup.Permissions)
            {
                if (!IsDisabledPermission(permission))
                {
                    SetPermissionGrant(permission, value);
                }
            }
        }

        private void PermissionChanged(bool value, PermissionGroupDto permissionGroup, PermissionGrantInfoDto permission)
        {
            SetPermissionGrant(permission, value);

            if (value && permission.ParentName != null)
            {
                var parentPermission = GetParentPermission(permissionGroup, permission);

                SetPermissionGrant(parentPermission, true);
            }
            else if (value == false)
            {
                var childPermissions = GetChildPermissions(permissionGroup, permission);

                foreach (var childPermission in childPermissions)
                {
                    SetPermissionGrant(childPermission, false);
                }
            }
        }

        private void SetPermissionGrant(PermissionGrantInfoDto permission, bool value)
        {
            if (permission.IsGranted == value)
            {
                return;
            }

            if (value)
            {
                GrantedPermissionCount++;
                NotGrantedPermissionCount--;
            }
            else
            {
                GrantedPermissionCount--;
                NotGrantedPermissionCount++;
            }

            permission.IsGranted = value;
        }

        private PermissionGrantInfoDto GetParentPermission(PermissionGroupDto permissionGroup, PermissionGrantInfoDto permission)
        {
            return permissionGroup.Permissions.First(x => x.Name == permission.ParentName);
        }

        private List<PermissionGrantInfoDto> GetChildPermissions(PermissionGroupDto permissionGroup, PermissionGrantInfoDto permission)
        {
            return permissionGroup.Permissions.Where(x => x.Name.StartsWith(permission.Name)).ToList();
        }

        private bool IsDisabledPermission(PermissionGrantInfoDto permissionGrantInfo)
        {
            return DisabledPermissions.Any(x => x == permissionGrantInfo);
        }

        private string GetShownName(PermissionGrantInfoDto permissionGrantInfo)
        {
            if (!IsDisabledPermission(permissionGrantInfo))
            {
                return permissionGrantInfo.DisplayName;
            }

            return string.Format(
                "{0} ({1})",
                permissionGrantInfo.DisplayName,
                permissionGrantInfo.GrantedProviders
                    .Where(p => p.ProviderName != ProviderName)
                    .Select(p => p.ProviderName)
                    .JoinAsString(", ")
            );
        }
    }
}