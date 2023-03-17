using EASendMail;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SheetSeller.Models.Domain;
using SheetSeller.Models.DTO;
using SheetSeller.Repositories.Abstract;
using System.Linq;

namespace SheetSeller.Repositories.Implement
{
    public class SheetService : ISheetService
    {
        private readonly DBContext ctx;
        private readonly IFileService fileService;
        public SheetService(DBContext ctx, IFileService fileService)
        {
            this.ctx = ctx;
            this.fileService = fileService;
        }
        public async Task<Status> CreateSheetAsync(Sheet model)
        {
            try
            {
                var user = model.Author;
                await ctx.Sheets.AddAsync(model);
                user.CreatedSheets.Add(model);
                ctx.Users.Update(user);
                await ctx.SaveChangesAsync();
                return new Status() { StatusCode = 1};
            }
            catch
            {
                return new Status() { StatusCode = 0, Message = "Error" };
            }
        }

        public async Task<Status> DeleteSheetAsync(Sheet model)
        {
            try
            {
                ctx.Sheets.Remove(model);
                await ctx.SaveChangesAsync();
                return new Status() { StatusCode = 1 };
            }
            catch
            {
                return new Status() { StatusCode = 0, Message="Error" };
            }
        }

        public async Task<Status> UpdateSheetAsync(EditSheet model)
        {
            try
            {
                var sheet = ctx.Sheets.Where(x => x.ID == model.ID).FirstOrDefault();
                sheet.Title=model.Title;
                sheet.Price=model.Price;
                sheet.Description=model.Description;
                ctx.Sheets.Update(sheet);
                await ctx.SaveChangesAsync();
                return new Status() { StatusCode = 1 };
            }
            catch
            {
                return new Status() { StatusCode = 0, Message = "Error" };
            }
        }
        public async Task<Status> UploadFileAsync(IFormFile File, int id)
        {
            try
            {
                var sheet = ctx.Sheets.Where(s=>s.ID==id).FirstOrDefault();
                var res = new Status();
                if (sheet.File != null)
                {
                    res = fileService.DeleteFile(sheet.File);
                }
                res = fileService.SavePDF(File, id.ToString());
                if (res.StatusCode == 0)
                {
                    return res;
                }
                sheet.File = res.Message;
                ctx.Sheets.Update(sheet);
                await ctx.SaveChangesAsync();
                return new Status() { StatusCode = 1, Message = "File saved succesfully" };
            }
            catch
            {
                return new Status() { StatusCode = 0, Message = "Error has occured" };
            }
        }
        public async Task<Status> Own(int SheetID,string username)
        {
            var sheet = ctx.Sheets.Where(s => s.ID == SheetID).FirstOrDefault();
            if (sheet == null) 
            { 
                return new Status() { StatusCode=0,Message="Sheet didn`t exist"};
            }
            var user = ctx.Users.Where(u=>u.UserName== username).Include("OwnedSheets").FirstOrDefault();
            if (user == null)
            {
                return new Status() { StatusCode = 0, Message = "User didn`t exist" };
            }
            user.OwnedSheets.Add(sheet);
            ctx.Users.Update(user);
            await ctx.SaveChangesAsync();

            return new Status { StatusCode = 1 };
        }
        public List<Sheet> GetSheets(string username)
        {
            var sheets = ctx.Sheets.Where(s=>s.Author.UserName==username).ToList();
            return sheets;
        }
        public Sheet GetSheet(int ID)
        {
            var sheets = ctx.Sheets.Include("Author")
                .Include("OwnedBy")
                .Where(s => s.ID == ID).ToList();
            var sheet = sheets.FirstOrDefault();
            if (sheets!=null)
            { return sheet; }
            return null;
        }
        public SheetList GetSheetList(string term = "", bool paging = false, int currentPage = 0)
        {
            var list = new SheetList();
            var sheets = ctx.Sheets.Include("OwnedBy").ToList();

            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                sheets = sheets.Where(a => a.Title.ToLower().Contains(term)).ToList();
            }

            if (paging)
            {
                int pageSize = 5;
                int TotalPages = (int)Math.Ceiling(sheets.Count / (double)pageSize);
                sheets = sheets.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                list.PageSize = pageSize;
                list.CurrentPage = currentPage;
                list.TotalPages = TotalPages;
            }
            list.Sheets = sheets.AsQueryable();
            return list;
        }
    }
}
