using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Shared.Core.Emails.System.EmailAttachmentProviders
{
    /// <summary>
    /// Maps entities to list of file ids
    /// </summary>
    public class MapEmailAttachmentProvider : IEmailAttachmentProviderWithAdding
    {

        private readonly Dictionary<string, List<long>> map;


        // constructor
        public MapEmailAttachmentProvider()
        {
            map = new Dictionary<string, List<long>>();
        }


        // adds file id to the map
        public void AddFileId(EntityTypeEnum? entityTypeId, long? entityId, long fileId)
        {
            var fileIds = GetFileIdsListByEntity(entityTypeId, entityId, true);
            fileIds.Add(fileId);            
        }

        // gets field ids by entity
        public List<long> GetFileIdsByEntity(EntityTypeEnum? entityTypeId, long? entityId)
        {
            var result = GetFileIdsListByEntity(entityTypeId, entityId);
            return result;
        }

        #region private methods

        // gets list of file ids by key 
        private List<long> GetFileIdsListByEntity(EntityTypeEnum? entityTypeId, long? entityId, bool addToMapIfNotExists = false)
        {            
            var key = GetKey(entityTypeId, entityId);
            if (!map.TryGetValue(key, out var result))
            {
                result = new List<long>();
                if (addToMapIfNotExists)
                    map[key] = result;
            }
            return result;
        }

        // gets key
        private string GetKey(EntityTypeEnum? entityTypeId, long? entityId)
        {
            var result = $"{entityTypeId ?? 0}#{entityId ?? 0}";
            return result;
        }

        #endregion


    }

}
