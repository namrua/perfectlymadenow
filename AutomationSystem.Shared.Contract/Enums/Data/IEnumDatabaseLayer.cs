using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Enums.Data.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.Enums.Data
{
    /// <summary>
    /// Interface for enum database layer
    /// </summary>
    public interface IEnumDatabaseLayer
    {
        // gets all enum types
        List<EnumType> GetEnumTypes();
        
        // gets enum type by id
        EnumType GetEnumTypeById(EnumTypeEnum enumTypeId);

        // get enum by id
        IEnumItem GetItemById(EnumTypeEnum enumTypeId, int id);

        // gets enum by filter [cached]
        List<IEnumItem> GetItemsByFilter(EnumTypeEnum enumTypeId, EnumItemFilter filter = null);
       
        // gets enum map by filter [cached]
        Dictionary<int, IEnumItem> GetMapByFilter(EnumTypeEnum enumTypeId, EnumItemFilter filter);
    }   
    
}
