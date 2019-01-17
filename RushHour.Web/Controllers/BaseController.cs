namespace RushHour.Web.Controllers
{
    using AutoMapper;
    using Data.Contracts;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RushHour.Web.Infrastructure.Extensions;
    using Service.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Web.Models;

    public abstract class BaseController<TEntity, TViewModel> : Controller where TEntity : class, IEntity where TViewModel : ViewModel
    {
        protected readonly IService<TEntity> service;
        private readonly UserManager<User> userManager;
        protected readonly IMapper mapper;
        private const string IndexAction = nameof(Index);

        public BaseController(IService<TEntity> service, UserManager<User> userManager, IMapper mapper)
        {
            this.service = service;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        protected abstract string ItemName { get; set; }

        protected abstract TViewModel SendFormData(TEntity item, TViewModel viewModel);

        protected abstract Task<TEntity> GetEntityAsync(TViewModel viewModel, int id);

        protected virtual async Task FillViewModelProps(IEnumerable<TViewModel> items)
        {
        }

        protected async Task<User> GetCurrentUserAsync()
            => await userManager.GetUserAsync(User);

        private RedirectToActionResult RedirectToIndex()
        {
            return RedirectToAction(IndexAction);
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            IEnumerable<TViewModel> items = mapper.Map<IEnumerable<TViewModel>>(await service.GetFilteredItemsAsync(await GetCurrentUserAsync()));

            await FillViewModelProps(items);

            return View(items);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            TViewModel viewModel = SendFormData(null, null);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(TViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddErrorMessage("You didn't fill the fields properly");

                SendFormData(null, viewModel);
                return View(viewModel);
            }

            TEntity item = await GetEntityAsync(viewModel, 0);

            if (service.IsItemDuplicate(item))
            {
                TempData.AddErrorMessage($"{ItemName} with such data already exists!");

                SendFormData(null, viewModel);
                return View(viewModel);
            }

            await service.InsertAsync(item);
            TempData.AddSuccessMessage($"You successfully created new {ItemName}!");

            return RedirectToIndex();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            TEntity item = await service.GetByIdAsync(id);

            if (item == null)
            {
                TempData.AddErrorMessage($"{ItemName} not found!");
                return RedirectToIndex();
            }

            if (!await service.IsUserAuthorized(item, await GetCurrentUserAsync()))
            {
                TempData.AddWarningMessage($"You don't have rights to edit this {ItemName}!");
                return RedirectToIndex();
            }

            TViewModel viewModel = mapper.Map<TEntity, TViewModel>(item);

            SendFormData(item, viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(TViewModel viewModel, int id)
        {
            TEntity item = await GetEntityAsync(viewModel, id);

            if (!ModelState.IsValid)
            {
                TempData.AddErrorMessage("You didn't fill the fields properly!");

                SendFormData(item, viewModel);
                return View(viewModel);
            }            

            if (!await service.IsUserAuthorized(item, await GetCurrentUserAsync()))
            {
                TempData.AddWarningMessage($"You don't have rights to edit this {ItemName}!");
                return RedirectToIndex();
            }

            //if (service.IsItemDuplicate(item))
            //{
            //    TempData.AddErrorMessage($"{ItemName} with such data already exists!");

            //    SendFormData(item, viewModel);
            //    return View(viewModel);
            //}

            await service.UpdateAsync(item);

            TempData.AddSuccessMessage($"You successfully edited the {ItemName}!");
            return RedirectToIndex();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            TEntity item = await service.GetByIdAsync(id);

            if (item == null)
            {
                TempData.AddErrorMessage($"{ItemName} not found!");
                return RedirectToIndex();
            }

            if (!await service.IsUserAuthorized(item, await GetCurrentUserAsync()))
            {
                TempData.AddWarningMessage($"You don't have rights to edit this {ItemName}!");
                return RedirectToIndex();
            }

            await service.Delete(item);
            return RedirectToIndex();
        }
    }
}
