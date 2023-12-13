
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecurePasswordManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace SecurePasswordManager.Controllers
{
    public class PasswordRecordsController : Controller
    {
        private readonly SecurePasswordManagerContext context;
        public PasswordRecordsController(SecurePasswordManagerContext context)
        {
            this.context = context;
        }
        //public async Task<IActionResult> Index()
        //{
        //    var securePasswordManagerContext = context.PasswordRecords.Include(p => p.User);
        //    return View(await securePasswordManagerContext.ToListAsync());
        //}
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            string username = null;
            if (HttpContext.Session.GetString("userSession") != null)
            {
                username = HttpContext.Session.GetString("userSession");

            }
            else
            {
                return RedirectToAction("Login");
            }
            var userRecords = await context.PasswordRecords.Where(pr => pr.User.Username == username)
                    .ToListAsync();

            return View(userRecords);
          
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPasswordRecordViewModel AddPasswordRequest)
        {
            // Hashing The Password
            //var salt = CreateSalt(); // Implement CreateSalt method
            //var passwordHash = HashPassword(AddPasswordRequest.Password, salt); // Implement HashPassword method
            string username = null;
            if (HttpContext.Session.GetString("userSession") != null)
            {
                username = HttpContext.Session.GetString("userSession").ToString();
                ViewBag.Session = HttpContext.Session.GetString("userSession").ToString();
            }
            else
            {
                return RedirectToAction("Add");
            }
            var passwordrecord = new PasswordRecord()
            {

                //RecordId = AddPasswordRequest.RecordId,
                Username = username,
                PlatformName = AddPasswordRequest.PlatformName,
                Password = AddPasswordRequest.Password,
                AdditionalInfo = AddPasswordRequest.AdditionalInfo,
                CreateDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,

            };

            await context.PasswordRecords.AddAsync(passwordrecord);
            await context.SaveChangesAsync();
            return RedirectToAction("View");

        }


        [HttpGet]
        public async Task<IActionResult> View(int id) {
            var record = await context.PasswordRecords.FirstOrDefaultAsync(x => x.RecordId == id);
            if (record != null)
            {
                var viewModel = new UpdatePasswordRecordViewModel()
                {
                    RecordId = record.RecordId,
                    PlatformName = record.PlatformName,
                    Password = record.Password,
                    AdditionalInfo = record.AdditionalInfo,
                };
                return await Task.Run (() =>View("View", viewModel));
            }

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdatePasswordRecordViewModel model)
        {
            var record = await context.PasswordRecords.FindAsync(model.RecordId);
            if (record !=null)
            {
                record.PlatformName = model.PlatformName;
                record.Password = model.Password;  
                record.AdditionalInfo= model.AdditionalInfo;
                record.CreateDate = DateTime.UtcNow;
                record.LastModifiedDate = DateTime.UtcNow;
                await context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }

            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await context.PasswordRecords.FirstOrDefaultAsync(x => x.RecordId == id);
            if (record == null)
            {
                return RedirectToAction("Dashboard");
            }

            return View(record);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var record = await context.PasswordRecords.FindAsync(id);
            if (record != null)
            {
                context.PasswordRecords.Remove(record);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Dashboard");
        }


    }


}

