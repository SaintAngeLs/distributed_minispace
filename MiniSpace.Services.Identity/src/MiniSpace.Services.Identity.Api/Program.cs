﻿using System;
using System.Threading.Tasks;
using Paralax;
using Paralax.Auth;
using Paralax.Secrets.Vault;
using Paralax.Logging;
using Paralax.Types;
using Paralax.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Identity.Application;
using MiniSpace.Services.Identity.Application.Commands;
using MiniSpace.Services.Identity.Application.Queries;
using MiniSpace.Services.Identity.Application.Services;
using MiniSpace.Services.Identity.Infrastructure;
using Paralax.Core;

namespace MiniSpace.Services.Identity.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddParalax()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UseEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetUser>("users/{userId}", (query, ctx) => GetUserAsync(query.UserId, ctx))
                        .Get("me", async ctx =>
                        {
                            var userId = await ctx.AuthenticateUsingJwtAsync();
                            if (userId == Guid.Empty)
                            {
                                ctx.Response.StatusCode = 401;
                                return;
                            }

                            await GetUserAsync(userId, ctx);
                        })
                        .Post<SignIn>("sign-in", async (cmd, ctx) =>
                        {
                            var token = await ctx.RequestServices.GetService<IIdentityService>().SignInAsync(cmd);
                            await ctx.Response.WriteJsonAsync(token);
                        })
                        .Post<SignUp>("sign-up", async (cmd, ctx) =>
                        {
                            await ctx.RequestServices.GetService<IIdentityService>().SignUpAsync(cmd);
                            await ctx.Response.Created("identity/me");
                        })
                        .Post<RevokeAccessToken>("access-tokens/revoke", async (cmd, ctx) =>
                        {
                            await ctx.RequestServices.GetService<IAccessTokenService>().DeactivateAsync(cmd.AccessToken);
                            ctx.Response.StatusCode = 204;
                        })
                        .Post<UseRefreshToken>("refresh-tokens/use", async (cmd, ctx) =>
                        {
                            var token = await ctx.RequestServices.GetService<IRefreshTokenService>().UseAsync(cmd.RefreshToken);
                            await ctx.Response.WriteJsonAsync(token);
                        })
                        .Post<RevokeRefreshToken>("refresh-tokens/revoke", async (cmd, ctx) =>
                        {
                            await ctx.RequestServices.GetService<IRefreshTokenService>().RevokeAsync(cmd.RefreshToken);
                            ctx.Response.StatusCode = 204;
                        })
                        .Post<BanUser>("users/{userId}/ban", async (cmd, ctx) =>
                        {
                            await ctx.RequestServices.GetService<IIdentityService>().BanUserAsync(cmd);
                            ctx.Response.StatusCode = 204;
                        })
                        .Delete<UnbanUser>("users/{userId}/ban", async (cmd, ctx) =>
                        {
                            await ctx.RequestServices.GetService<IIdentityService>().UnbanUserAsync(cmd);
                            ctx.Response.StatusCode = 204;
                        })
                        .Post<ForgotPassword>("password/forgot", async (cmd, ctx) =>
                        {
                            await ctx.RequestServices.GetService<IIdentityService>().ForgotPasswordAsync(cmd);
                            ctx.Response.StatusCode = 204;
                        })
                        .Post<ResetPassword>("password/reset", async (cmd, ctx) =>
                        {
                            await ctx.RequestServices.GetService<IIdentityService>().ResetPasswordAsync(cmd);
                            ctx.Response.StatusCode = 204;
                        })
                         .Post<VerifyEmail>("email/verify", async (cmd, ctx) =>
                        {
                            await ctx.RequestServices.GetService<IIdentityService>().VerifyEmailAsync(cmd);
                            ctx.Response.StatusCode = 204;
                        })
                        .Post<EnableTwoFactor>("2fa/enable", async (cmd, ctx) =>
                        {
                            await ctx.RequestServices.GetService<IIdentityService>().EnableTwoFactorAsync(cmd);
                            ctx.Response.StatusCode = 204;
                        })
                        .Post<DisableTwoFactor>("2fa/disable", async (cmd, ctx) =>
                        {
                            await ctx.RequestServices.GetService<IIdentityService>().DisableTwoFactorAsync(cmd);
                            ctx.Response.StatusCode = 204;
                        })
                        .Post<GenerateTwoFactorSecret>("2fa/generate-secret", async (cmd, ctx) =>
                        {
                            var secret = await ctx.RequestServices.GetService<IIdentityService>().GenerateTwoFactorSecretAsync(cmd);
                            await ctx.Response.WriteJsonAsync(new { Secret = secret });
                        })
                        .Post<VerifyTwoFactorCode>("2fa/verify-code", async (cmd, ctx) =>
                        {
                            var token = await ctx.RequestServices.GetService<IIdentityService>().VerifyTwoFactorCodeAsync(cmd);
                            await ctx.Response.WriteJsonAsync(token);
                        })
                        .Put<UpdateUserStatus>("users/status", async (cmd, ctx) =>
                        {
                            await ctx.RequestServices.GetService<IIdentityService>().UpdateUserStatusAsync(cmd);
                            ctx.Response.StatusCode = 204;
                        })
                    ))
                .UseLogging()
                .Build()
                .RunAsync();

        private static async Task GetUserAsync(Guid id, HttpContext context)
        {
            var user = await context.RequestServices.GetService<IIdentityService>().GetAsync(id);
            Console.WriteLine(user.ToString());
            if (user is null)
            {
                context.Response.StatusCode = 404;
                return;
            }

             await context.Response.WriteJsonAsync(new
            {
                Id = user.Id, // Ensure this is directly a Guid, not wrapped
                user.Name,
                user.Email,
                Role = user.Role.ToString(),
                user.CreatedAt,
                Permissions = user.Permissions,
                user.IsEmailVerified,
                user.EmailVerifiedAt,
                user.IsTwoFactorEnabled,
                user.TwoFactorSecret,
                user.IsOnline,
                user.DeviceType,
                user.LastActive,
                user.IpAddress
            });
        }
    }
}
