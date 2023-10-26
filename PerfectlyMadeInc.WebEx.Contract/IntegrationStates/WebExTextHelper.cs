using System;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;

namespace PerfectlyMadeInc.WebEx.Contract.IntegrationStates
{
    public static class WebExTextHelper
    {
       
        // attendee missing message text
        public const string AttendeeIdMissingMessage = "There is no information about attendee ID and therefore WebEx state of registration cannot be resolved.";

        // gets InconsistencyType text
        public static string GetInconsistencyTypeText(InconsistencyType? inconsistencyType)
        {
            if (!inconsistencyType.HasValue)
                return null;

            switch (inconsistencyType.Value)
            {
                case InconsistencyType.None:
                    return "WebEx is consistent with the registration.";

                case InconsistencyType.NotInWebEx:
                    return "The approved registration is missing in the WebEx.";

                case InconsistencyType.NotInSystem:
                    return "There is unapproved, canceled or unknown person in the WebEx.";

                case InconsistencyType.InconsistentData:
                    return "WebEx data is inconsistent with the registration.";

                case InconsistencyType.DuplicitEmail:
                    return "The registration has duplicit email.";

                default:
                    throw new ArgumentOutOfRangeException(nameof(inconsistencyType));
            }

        }

    }

}
