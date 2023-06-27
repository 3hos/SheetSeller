using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SheetSeller.Models.Domain;
using SheetSeller.Models.DTO;
using SheetSeller.Repositories.Abstract;

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
                return new Status() { StatusCode = 1 };
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
                return new Status() { StatusCode = 0, Message = "Error" };
            }
        }

        public async Task<Status> UpdateSheetAsync(EditSheet model)
        {
            try
            {
                var sheet = ctx.Sheets.Where(x => x.ID == model.ID).FirstOrDefault();
                sheet.Title = model.Title;
                sheet.Price = model.Price;
                sheet.Description = model.Description;
                ctx.Sheets.Update(sheet);
                await ctx.SaveChangesAsync();
                return new Status() { StatusCode = 1 };
            }
            catch
            {
                return new Status() { StatusCode = 0, Message = "Error" };
            }
        }

        public async Task<Status> AddTag(string tag, int id)
        {
            try
            {
                var sheet = ctx.Sheets.Where(s => s.ID == id).FirstOrDefault();
                if (sheet.Tags.Any(t => t.Name == tag))
                {
                    return new Status() { Message = "Sheet already have this tag", StatusCode = 0 };
                }
                if (sheet.Tags.Count() >= 4)
                {
                    return new Status() { Message = "Too many tags", StatusCode = 0 };
                }
                var db_tag = ctx.Tags.Where(t => t.Name == tag).FirstOrDefault();
                if (db_tag == null)
                    sheet.Tags.Add(new Tag() { Name = tag });
                else
                    sheet.Tags.Add(db_tag);
                ctx.Sheets.Update(sheet);
                await ctx.SaveChangesAsync();
                return new Status() { StatusCode = 1 };
            }
            catch
            {
                return new Status() { StatusCode = 0 };
            }
        }

        public async Task<Status> RemoveTag(string tag, int id)
        {
            var sheet = ctx.Sheets.Where(s => s.ID == id).FirstOrDefault();
            if (!sheet.Tags.Any(t => t.Name == tag))
            {
                return new Status() { Message = "Sheet didn`t have this tag", StatusCode = 0 };
            }
            sheet.Tags.RemoveAll(t => t.Name == tag);
            ctx.Sheets.Update(sheet);
            await ctx.SaveChangesAsync();
            return new Status() { StatusCode = 1 };
        }

        public async Task<Status> UploadFileAsync(IFormFile File, int id)
        {
            try
            {
                var sheet = ctx.Sheets.Where(s => s.ID == id).FirstOrDefault();
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

        public async Task<Status> Own(int SheetID, string username)
        {
            var sheet = ctx.Sheets.Where(s => s.ID == SheetID).FirstOrDefault();
            if (sheet == null)
            {
                return new Status() { StatusCode = 0, Message = "Sheet didn`t exist" };
            }
            var user = ctx.Users.Where(u => u.UserName == username).Include("OwnedSheets").FirstOrDefault();
            if (user == null)
            {
                return new Status() { StatusCode = 0, Message = "User didn`t exist" };
            }
            user.OwnedSheets.Add(sheet);
            ctx.Users.Update(user);
            await ctx.SaveChangesAsync();

            return new Status { StatusCode = 1 };
        }

        public async Task<Status> DeOwn(int SheetID, string username)
        {
            try
            {
                var sheet = ctx.Sheets.Where(s => s.ID == SheetID).FirstOrDefault();
                sheet.OwnedBy.RemoveAll(u => u.UserName == username);
                ctx.Sheets.Update(sheet);
                await ctx.SaveChangesAsync();
                return new Status { StatusCode = 1 };
            }
            catch { return new Status { StatusCode = 0 }; }
        }

        public IQueryable<Sheet> GetSheets(string username)
        {
            var sheets = ctx.Sheets.Where(s => s.Author.UserName == username);
            return sheets;
        }
        public IQueryable<Sheet> GetSheets(ApplicationUser user)
        {
            var sheets = ctx.Sheets.Where(s => s.Author == user);
            return sheets;
        }
        public IQueryable<Sheet> OwnedSheets(ApplicationUser user)
        {
            var sheets = ctx.Sheets.Where(s => s.OwnedBy.Contains(user));
            return sheets;
        }
        public Sheet GetSheet(int ID)
        {
            var sheets = ctx.Sheets.Include("Author")
                .Where(s => s.ID == ID).ToList();
            var sheet = sheets.FirstOrDefault();
            if (sheets != null)
            { return sheet; }
            return null;
        }
        public SheetList GetSheetList(string term = "",string tag="", bool paging = false, int currentPage = 0, string sorting = "")
        {
            var list = new SheetList();
            IQueryable<Sheet> sheets = ctx.Sheets;

            if (!string.IsNullOrEmpty(term))
            {
                list.Term = term;
                term = term.ToLower();
                sheets = sheets.Where(a => a.Title.ToLower().Contains(term));
            }

            if (!string.IsNullOrEmpty(tag))
            {
                list.Tag = tag;
                tag = tag.ToLower();
                sheets = sheets.Where(a => a.Tags.Any(t=>t.Name==tag));
            }

            list.Sorting= sorting;
            switch (sorting)
            {
                case "priceUp":
                    sheets = sheets.OrderBy(s => s.Price);
                    break;
                case "priceDown":
                    sheets = sheets.OrderByDescending(s => s.Price);
                    break;
                default:
                    sheets = sheets.OrderByDescending(s => s.OwnedBy.Count);
                    list.Sorting = "Popularity";
                    break;
            }


            if (paging)
            {
                int pageSize = 8;
                int TotalPages = (int)Math.Ceiling(sheets.Count() / (double)pageSize);
                sheets = sheets.Skip((currentPage - 1) * pageSize).Take(pageSize);
                list.PageSize = pageSize;
                list.CurrentPage = currentPage;
                list.TotalPages = TotalPages;
            }
            list.selectListItems = new List<SelectListItem>
            {
                new SelectListItem { Value = "priceUp", Text = "Price Up" },
                new SelectListItem { Value = "priceDown", Text = "Price Down" },
                new SelectListItem { Value = "popularity", Text = "Popularity" }
            };

            list.Sheets = sheets;
            return list;
        }
        public IQueryable<Tag> GetTags(string term = "", int maxlen = 200)
        {
            var tags = ctx.Tags.Include("TaggedSheets").Where(t => t.Name.Contains(term)).OrderByDescending(a => a.TaggedSheets.Count).Take(maxlen); 
            return tags;
        }
    }
}
