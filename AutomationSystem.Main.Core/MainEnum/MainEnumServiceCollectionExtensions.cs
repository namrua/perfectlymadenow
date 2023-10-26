using System;
using System.Collections.Generic;
using AutoMapper;
using AutomationSystem.Main.Core.MainEnum.AppLogic.MappingProfiles;
using AutomationSystem.Shared.Contract.Enums.Data;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Main.Core.MainEnum
{
    public static class MainEnumServiceCollectionExtensions
    {
        public static List<Profile> CreateProfiles(IServiceProvider provider)
        {
            return new List<Profile>
            {
                new MainEnumProfile(provider.GetService<IEnumDatabaseLayer>())
            };
        }
    }
}
