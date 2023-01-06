using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MicroserviceDemo.ContactService.Contacts;
using MicroserviceDemo.ContactService.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace MicroserviceDemo.Web.Pages.Contacts
{
    [Authorize(ContactServicePermissions.Contacts.Default)]
    public partial class Contacts
    {
        [Inject]
        protected IContactAppService ContactAppService { get; set; }

        [Inject]
        private IValidator<ContactCreateDto> ContactCreateDtoValidator { get; set; }

        [Inject]
        private IValidator<ContactUpdateDto> ContactUpdateDtoValidator { get; set; }

        private MudDataGrid<ContactListDto> Grid { get; set; }
        public PagedResultDto<ContactListDto> GridData { get; set; } = new();

        private bool IsGridLoading { get; set; }

        private ContactCreateDto NewEntity { get; set; } = new();
        private ContactUpdateDto EditingEntity { get; set; } = new();
        private Guid EditingEntityId { get; set; }

        private MudDialog CreateModal { get; set; }
        private bool IsCreateModalVisible { get; set; }

        private MudDialog EditModal { get; set; }
        private bool IsEditModalVisible { get; set; }

        private MudForm CreateForm { get; set; }
        private MudForm EditForm { get; set; }

        private bool HasCreatePermission { get; set; }
        private bool HasUpdatePermission { get; set; }
        private bool HasDeletePermission { get; set; }

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
                new(L["Menu:Contacts"], "/contacts", true)
            };

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            HasCreatePermission = await AuthorizationService.IsGrantedAsync(ContactServicePermissions.Contacts.Create);
            HasUpdatePermission = await AuthorizationService.IsGrantedAsync(ContactServicePermissions.Contacts.Update);
            HasDeletePermission = await AuthorizationService.IsGrantedAsync(ContactServicePermissions.Contacts.Delete);
        }

        private async Task OpenCreateModalAsync()
        {
            try
            {
                CreateForm?.ResetValidation();

                NewEntity = new ContactCreateDto();

                CreateModal?.Show(
                    L["NewContact"],
                    options: new DialogOptions
                    {
                        MaxWidth = MaxWidth.Small
                    }
                );
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
                    await ContactAppService.CreateAsync(NewEntity);

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
                await EditForm.Validate();

                if (EditForm.IsValid)
                {
                    await ContactAppService.UpdateAsync(EditingEntityId, EditingEntity);

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

        private async Task DeleteEntityAsync(ContactListDto entity)
        {
            try
            {
                if (await Message.Confirm(L["DeletionConfirmationMessage", L["Contact"], entity.Name]))
                {
                    await ContactAppService.DeleteAsync(entity.Id);

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

        private async Task OpenEditModalAsync(ContactListDto entity)
        {
            try
            {
                EditForm?.ResetValidation();

                var entityDto = await ContactAppService.GetAsync(entity.Id);

                EditingEntityId = entity.Id;
                EditingEntity = ObjectMapper.Map<ContactDto, ContactUpdateDto>(entityDto);

                await InvokeAsync(
                    () =>
                    {
                        StateHasChanged();
                        EditModal?.Show(
                            L["Edit"],
                            options: new DialogOptions
                            {
                                MaxWidth = MaxWidth.Small
                            }
                        );
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

        private async Task<GridData<ContactListDto>> ReadGridData(GridState<ContactListDto> gridState)
        {
            IsGridLoading = true;

            try
            {
                GridData = await ContactAppService.GetListAsync(
                    new GetContactsInput
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

            return new GridData<ContactListDto>
            {
                Items = GridData.Items,
                TotalItems = (int) GridData.TotalCount
            };
        }
    }
}