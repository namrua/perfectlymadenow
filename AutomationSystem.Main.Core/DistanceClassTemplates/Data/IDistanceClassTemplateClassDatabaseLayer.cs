using System;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.Data
{
    public interface IDistanceClassTemplateClassDatabaseLayer
    {
        List<long> GetFilledDistanceTemplateIdsForDistanceProfileId(long distanceProfileId, DateTime fromRegistrationEnd);

        List<long> GetFilledDistanceProfileIdsForDistanceClassTemplateId(long distanceClassTemplateId);

        List<long> GetFilledClassIdsByDistanceClassTemplateId(long distanceClassTemplateId);

        void InsertDistanceClassTemplateClass(long distanceTemplateId, long distanceProfileId, long classId);
    }
}
