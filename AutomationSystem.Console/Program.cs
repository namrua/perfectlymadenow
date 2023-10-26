using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Console.ConsoleCommands;
using AutomationSystem.Console.ConsoleCommands.Models;
using AutomationSystem.Main.Core;
using AutomationSystem.Shared.Contract.Identities.System.Models;
using AutomationSystem.Shared.Core;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.DesignTools;
using PerfectlyMadeInc.WebEx;

namespace AutomationSystem.Console
{
    class Program
    {
        private static readonly IServiceCollection serviceCollection = new ServiceCollection();
        private static IServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            InitializeComponents();

            System.Console.WriteLine("Done");
            System.Console.ReadLine();
        }

        #region  initialization

        // initializes components
        private static void InitializeComponents()
        {
            // registers components for backward compatibility   
            serviceCollection.AddDesignToolsServices();
            serviceCollection.AddSharedServices();
            serviceCollection.AddWebExServices();
            serviceCollection.AddMainServices();
            serviceCollection.AddConsoleCommands();
            serviceProvider = serviceCollection.BuildServiceProvider();

            // set admin identity
            var userIdentity = new ClaimsIdentity();
            userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, CoreIdentityConstants.DefaultIdForClaims.ToString()));
            userIdentity.AddClaim(new Claim(ClaimTypes.Name, CoreIdentityConstants.DefaultNameForClaims));
            userIdentity.AddClaim(new Claim(ClaimTypes.Email, CoreIdentityConstants.DefaultEmailForClaims));
            userIdentity.AddClaim(new Claim(ClaimTypes.Role, ((int)UserRoleTypeEnum.Administrator).ToString()));
            Thread.CurrentPrincipal = new ClaimsPrincipal(userIdentity);

            // Enable TLS 1.2
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol &= ~(SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11);
        }

        #endregion

    }
}
