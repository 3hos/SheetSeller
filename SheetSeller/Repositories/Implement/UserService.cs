using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SheetSeller.Models.Domain;
using SheetSeller.Models.DTO;
using SheetSeller.Repositories.Abstract;

namespace SheetSeller.Repositories.Implement
{
    public class UserService : IUserService
    {
        private readonly IFileService fileService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly DBContext ctx;
        public UserService(IFileService fileService,UserManager<ApplicationUser> userManager, DBContext ctx)
        {
            this.fileService = fileService;
            this.userManager = userManager;
            this.ctx = ctx;
        }
        public async Task<ApplicationUser> GetUserByName(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            return user;
        }
        public async Task<Status> SetImgProfile(IFormFile imageFile,string username)
        {
            try
            {
                var user = await userManager.FindByNameAsync(username);
                var res = new Status();
                if (user.ImageProfile!=null)
                {
                    res = fileService.DeleteFile(user.ImageProfile);
                }
                res = fileService.SaveImage(imageFile, user.Id);
                if (res.StatusCode==0)
                {
                    return res;
                }
                user.ImageProfile = res.Message;
                ctx.Users.Update(user);
                await ctx.SaveChangesAsync();
                return new Status() { StatusCode = 1, Message = "Profile image has set" };
            }
            catch {
                return new Status() { StatusCode = 0, Message = "Error has occured" };
            }
        }
        public async Task<Status> DeleteImgProfile(string username)
        {
            try
            {
                var user = await userManager.FindByNameAsync(username);
                var res = fileService.DeleteFile(user.ImageProfile);
                if (res.StatusCode == 0)
                {
                    return res;
                }
                user.ImageProfile = null;
                ctx.Users.Update(user);
                ctx.SaveChanges();
                return new Status() { StatusCode = 1, Message = "Profile image has set" };
            }
            catch
            {
                return new Status() { StatusCode = 0, Message = "Error has occured" };
            }
        }
    }
}
