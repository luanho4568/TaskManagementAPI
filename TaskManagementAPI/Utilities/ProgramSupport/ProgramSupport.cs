using TaskManagementAPI.Services.Account;
using TaskManagementAPI.Services.Group;
using TaskManagementAPI.Services.Profile;
using TaskManagementAPI.Utilities.JwtAuthentication;

namespace TaskManagementAPI.Utilities.ProgramSupport
{
    public static class ProgramSupport
    {
        public static void ProgramBuildAddScoped(WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<ProfileService>();
            builder.Services.AddScoped<GroupService>();
            builder.Services.AddScoped<ChatService>();
        }
    }
}
