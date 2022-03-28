using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using mbit.common.logging;
using oiat.saferinternetbot.Business.Identity;
using oiat.saferinternetbot.DataAccess.Entities;
using oiat.saferinternetbot.web.Models;

namespace oiat.saferinternetbot.web.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private static readonly IMcLogger _logger = McLogFactory.GetCurrentLogger();

        private readonly ApplicationUserManager _userManager;
        private readonly IMapper _mapper;

        public UserController(ApplicationUserManager userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(_mapper.Map<IEnumerable<UserViewModel>>(users));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new UserCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    PushWarning("Benutzer erstellen", "Bitte Eingaben überprüfen");
                    return View();
                }

                var entity = new ApplicationUser();
                _mapper.Map(model, entity);
                var result = await _userManager.CreateAsync(entity, model.Password);

                if (!result.Succeeded)
                {
                    PushError("Benutzer erstellen", "Fehler beim Erstellen des Benutzers");
                    return View();
                }

                PushSuccess("Benutzer erstellen", "Benutzer erfolgreich erstellt");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                PushError("Benutzer erstellen", "Fehler beim Erstellen des Benutzers");
                _logger.Error(ex, "Error while creating user");
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return View(_mapper.Map<UserCreateViewModel>(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, UserCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var entity = await _userManager.FindByIdAsync(id);
                _mapper.Map(model, entity);

                var result = await _userManager.UpdateAsync(entity);

                if (!result.Succeeded)
                {
                    return View();
                }

                result = await _userManager.RemovePasswordAsync(id);

                if (!result.Succeeded)
                {
                    return View();
                }

                result = await _userManager.AddPasswordAsync(id, model.Password);

                if (!result.Succeeded)
                {
                    return View();
                }

                PushSuccess("Benutzer bearbeiten", "Benutzer erfolgreich gespeichert");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error while updating user {id}");
                return View();
            }
        }
    }
}