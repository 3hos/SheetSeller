using Microsoft.AspNetCore.Identity;
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
                await ctx.Sheets.AddAsync(model);
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
        public async Task<List<Sheet>> GetSheetsAsync(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            return user.CreatedSheets.ToList();
        }
    }
}
