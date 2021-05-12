using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcCorePracticaDynamoDb.Models;
using MvcCorePracticaDynamoDb.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCorePracticaDynamoDb.Controllers
{
    public class UsuariosController: Controller
    {
        ServiceDynamoDB ServiceDynamoDB;
        ServiceS3 ServiceS3;

        public UsuariosController(ServiceS3 services3, ServiceDynamoDB servicedynamodb)
        {
            this.ServiceDynamoDB = servicedynamodb;
            this.ServiceS3 = services3;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this.ServiceDynamoDB.GetUsuarios());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Usuario usu, String incluirimagenes,
            List<IFormFile> imagenes)
        {
            if (incluirimagenes != null)
            {
                usu.Fotos = new List<Foto>();
                foreach (IFormFile iform in imagenes)
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        iform.CopyTo(m);
                        await this.ServiceS3.UploadFile(m, iform.FileName);
                    }
                    //String img = this.ServiceS3.GetUrlFile(iform.FileName);
                    usu.Fotos.Add(new Foto() { Imagen = this.ServiceS3.GetUrlFile(iform.FileName) });
                }
            }
            usu.FechaAlta = DateTime.Now.ToShortDateString();
            await this.ServiceDynamoDB.CreateUsuario(usu);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int idusu)
        {
            return View(await this.ServiceDynamoDB.GetUsuario(idusu));
        }

        public async Task<IActionResult> Edit(int idusu)
        {
            Usuario u = await this.ServiceDynamoDB.GetUsuario(idusu);
            return View(await this.ServiceDynamoDB.GetUsuario(idusu));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(String masinfo, Usuario usu,
            List<IFormFile> foto, List<String> titulo)
        {
            //Usuario u = await this.ServiceDynamoDB.GetUsuario(usu.IdUsuario);
            //if (masinfo != null)
            //{
            //    if (foto != null)
            //    {
            //        if (u.Fotos != null)
            //        {
            //            usu.Fotos = u.Fotos;
            //        }
            //        else
            //        {
            //            usu.Fotos = new List<Foto>();
            //        }
            //        for (int i = 0; i < foto.Count; i++)
            //        {
            //            using (MemoryStream m = new MemoryStream())
            //            {
            //                foto[i].CopyTo(m);
            //                await this.ServiceS3.UploadFile(m, foto[i].FileName);
            //            }
            //            usu.Fotos.Add(new Foto { Titulo = titulo[i], Imagen = this.ServiceS3.GetUrlFile(foto[i].FileName) });
            //        }
            //    }
            //}
            //await this.ServiceDynamoDB.Update(usu);
            //return View(await this.ServiceDynamoDB.GetUsuario(usu.IdUsuario));
            Usuario u = await this.ServiceDynamoDB.GetUsuario(usu.IdUsuario);
            u.Nombre = usu.Nombre;
            u.Descripcion = usu.Descripcion;
            await this.ServiceDynamoDB.CreateUsuario(usu);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int idusu)
        {
            Usuario u = await this.ServiceDynamoDB.GetUsuario(idusu);
            if (u.Fotos != null)
            {
                foreach (Foto foto in u.Fotos)
                {
                    String imagen = foto.Imagen.Substring(foto.Imagen.LastIndexOf(".com/") + 5, foto.Imagen.Length - foto.Imagen.LastIndexOf(".com/") - 5);
                    await this.ServiceS3.DeleteFile(imagen);
                }
            }
            await this.ServiceDynamoDB.DeleteUsuario(idusu);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteImage(int idusu, String filename)
        {
            String imagen = filename.Substring(filename.LastIndexOf(".com/") + 5, filename.Length - filename.LastIndexOf(".com/") - 5);
            await this.ServiceS3.DeleteFile(imagen);
            Usuario u = await this.ServiceDynamoDB.GetUsuario(idusu);
            foreach (Foto foto in u.Fotos.ToList())
            {
                if (foto.Imagen.Equals(filename))
                {
                    u.Fotos.Remove(foto);
                }
            }
            return RedirectToAction("Details", new { idusu = idusu });
        }
    }
}
