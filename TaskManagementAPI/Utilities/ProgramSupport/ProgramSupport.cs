﻿
using TaskManagementAPI.Services.Account;
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
        }
    }
}
