using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http.Features;
using Syncfusion.Blazor.Inputs;
using Kucni_poslovi.Data;
using Microsoft.AspNetCore.Identity;

namespace Kucni_poslovi.Kontroleri
{
    [Route("api/[controller]")]
    public partial class UploaderController : Controller
    {
        
        private IWebHostEnvironment hostingEnv;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public UploaderController(IWebHostEnvironment env, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.hostingEnv = env;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Route("Save")]
        public void Save(IList<IFormFile> UploadFiles)
        {
            var guid = Request.Form["id"];
            var prethodnaSlika = Request.Form["prethodna"];
            String putanjaDoPrethodneSlike = prethodnaSlika.ToString();
            long size = 0;
            try
            {
                var filename = ContentDispositionHeaderValue
                                    .Parse(UploadFiles[0].ContentDisposition)
                                    .FileName
                                    .Trim('"');
                filename = guid + "_" + filename;
                filename = @"wwwroot/KorisnickeSlike/" + filename;
                size += UploadFiles[0].Length;
                if (System.IO.File.Exists(putanjaDoPrethodneSlike))
                {
                    System.IO.File.Delete(putanjaDoPrethodneSlike);
                }

                using (FileStream fs = System.IO.File.Create(filename))
                {
                    UploadFiles[0].CopyTo(fs);
                    fs.Flush();
                }
                    
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }

        [HttpPost("Remove")]
        public void Remove()
        {
            try
            {
                var putanjaDoSlike = Request.Form["slika"];
                if (System.IO.File.Exists(@"wwwroot/" + putanjaDoSlike))
                {
                    System.IO.File.Delete(@"wwwroot/" + putanjaDoSlike);
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed successfully";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }

        [HttpPost("ZameniSifru")]
        public async Task<bool> ZameniSifru([FromBody] Sifra podaci)
        {
            bool uspelo;
            var user = await _userManager.FindByNameAsync(podaci.userName);
            var rez = await _userManager.ChangePasswordAsync(user, podaci.trenutnaSifra, podaci.novaSifra);
            if (rez.Succeeded)
                uspelo = true;
            else
                uspelo = false;
            return uspelo;
        }

        public IActionResult DefaultFunctionalities()
        {
            return View();
        }
    }
}