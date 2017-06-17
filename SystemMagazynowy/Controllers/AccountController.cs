using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SystemMagazynowy.Models;
using SystemMagazynowy.DAL;
using System.Collections.Generic;
using System.Net;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SystemMagazynowy.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        WarehouseService service = new WarehouseService();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            IQueryable<UserViewModel> users = from user in db.Users
                                              orderby user.UserName ascending
                                              select new UserViewModel()
                                              {
                                                  UserName = user.UserName,
                                                  Email = user.Email,
                                                  PhoneNumber = user.PhoneNumber
                                              };

            

            return View(users.ToList());
        }


        public ActionResult Edit(string id)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                                  
            ApplicationUser user = UserManager.Users.Where(m => m.UserName.Contains(id)).Single();
            var roles = GetRoles(user.Roles);
            EditUserViewModel model = new EditUserViewModel
            {
                ID = user.UserName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };


            if (IsInRole(roles, "canManageAssortment")) model.canManageAssortment=true;
            if (IsInRole(roles, "canManageCategories")) model.canManageCategories= true;
            if (IsInRole(roles, "canManageContractors")) model.canManageContractors =true;
            if (IsInRole(roles, "canManageUsers")) model.canManageUsers= true;
            if (IsInRole(roles, "canManageWarehouses")) model.canManageWarehouses = true;
            if (IsInRole(roles, "canOperateWithAssortment")) model.canOperateWithAssortment = true;
            if (IsInRole(roles, "canPerformStocktaking")) model.canPerformStocktaking = true;

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(EditUserViewModel model)
        {
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userToUpdate = db.Users.Where(u=>u.UserName.Contains(model.ID)).Single();
            var roles = GetRoles(userToUpdate.Roles);

            List<string> userRoles = new List<string>();
            foreach(var item in roles)
            {
                userRoles.Add(item.Name);
            }


            List<string> rolesToAdd = new List<string>();
            rolesToAdd = EditCheckboxToList(model, rolesToAdd);
            List<string> rolesToDelete = userRoles;

            foreach (string role in rolesToAdd)
            {
                if (rolesToDelete.Contains(role))
                {
                    rolesToDelete.Remove(role);
                }
            }




            foreach (var role in roles)
            {
                
                if(rolesToAdd.Contains(role.Name))
                {
                    rolesToAdd.Remove(role.Name);
                }
                
            }


            if (TryUpdateModel(userToUpdate,new string[]{"UserName","PhoneNumber","Email"}))
            {
                try
                {
                    
                    await UserManager.AddToRolesAsync(userToUpdate.Id, rolesToAdd.ToArray());

                    await UserManager.RemoveFromRolesAsync(userToUpdate.Id, rolesToDelete.ToArray());

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Błąd w czasie próby zapisu. Spróbuj jeszcze raz później lub skontaktuj się z administratorem.");
                }
            }
            
            return View(userToUpdate);
        }


       
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, false, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Nieudana próba logowania.");
                    return View(model);
            }
        }

      
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                List<string> roles = new List<string>();



                //if (model.canManageAssortment) roles.Add("canManageAssortment");
                //if (model.canManageCategories) roles.Add("canManageCategories");
                //if (model.canManageContractors) roles.Add("canManageContractors");
                //if (model.canManageUsers) roles.Add("canManageUsers");
                //if (model.canManageWarehouses) roles.Add("canManageWarehouses");
                //if (model.canOperateWithAssortment) roles.Add("canOperateWithAssortment");
                //if (model.canPerformStocktaking) roles.Add("canPerformStocktaking");

                RegisterCheckboxToList(model, roles);

                var user = new ApplicationUser { UserName = model.UserName, PhoneNumber = model.PhoneNumber, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRolesAsync(user.Id, roles.ToArray());
                    
                    return RedirectToAction("Index", "Account");
                }
                AddErrors(result);
            }

            
            return View(model);
        }

     
       

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        private ICollection<IdentityRole> GetRoles(ICollection<IdentityUserRole> roles)
        {
            ICollection<IdentityRole> tmp = new List<IdentityRole>();
            //var allRoles
            foreach(IdentityUserRole role in roles)
            {
                tmp.Add(db.Roles.Single(r => r.Id == role.RoleId));
            }

            return tmp;
        }


        private bool IsInRole(ICollection<IdentityRole> roles, string roleName)
        {
            if (roles.Where(r=>r.Name==roleName).Count()>0)
                return true;

            return false;
        }

        private List<string> EditCheckboxToList(EditUserViewModel model, List<string> list)
        {
            try
            {
                if (model.canManageAssortment) list.Add("canManageAssortment");
                if (model.canManageCategories) list.Add("canManageCategories");
                if (model.canManageContractors) list.Add("canManageContractors");
                if (model.canManageUsers) list.Add("canManageUsers");
                if (model.canManageWarehouses) list.Add("canManageWarehouses");
                if (model.canOperateWithAssortment) list.Add("canOperateWithAssortment");
                if (model.canPerformStocktaking) list.Add("canPerformStocktaking");
            }
            catch
            {
                return list;
            }
            return list;
        }

        private List<string> RegisterCheckboxToList(RegisterViewModel model, List<string> list)
        {
            try
            {
                if (model.canManageAssortment) list.Add("canManageAssortment");
                if (model.canManageCategories) list.Add("canManageCategories");
                if (model.canManageContractors) list.Add("canManageContractors");
                if (model.canManageUsers) list.Add("canManageUsers");
                if (model.canManageWarehouses) list.Add("canManageWarehouses");
                if (model.canOperateWithAssortment) list.Add("canOperateWithAssortment");
                if (model.canPerformStocktaking) list.Add("canPerformStocktaking");
            }
            catch
            {
                return list;
            }
            return list;
        }

        #endregion
    }
}