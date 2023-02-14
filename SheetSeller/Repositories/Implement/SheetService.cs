﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SheetSeller.Models.Domain;
using SheetSeller.Models.DTO;
using SheetSeller.Repositories.Abstract;

namespace SheetSeller.Repositories.Implement
{
    public class SheetService : ISheetService
    {
        private readonly DBContext ctx;
        private readonly UserManager<ApplicationUser> userManager;
        public SheetService(DBContext ctx, UserManager<ApplicationUser> userManager)
        {
            this.ctx = ctx;
            this.userManager = userManager;
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
                return new Status() { StatusCode = 0, Message="Error" };
            }
        }

        public async Task<Status> UpdateSheetAsync(Sheet model)
        {
            try
            {
                ctx.Sheets.Update(model);
                await ctx.SaveChangesAsync();
                return new Status() { StatusCode = 1 };
            }
            catch
            {
                return new Status() { StatusCode = 0, Message = "Error" };
            }
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
    }
}
