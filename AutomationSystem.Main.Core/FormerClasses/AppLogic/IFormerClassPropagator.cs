using System.Collections.Generic;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic
{


    /// <summary>
    /// Propagates class and class registration into former database
    /// </summary>
    public interface IFormerClassPropagator
    {

        // propagates class and class registrations to former database
        long? PropagateToFormerDatabase(Class cls, List<ClassRegistration> classRegistrations);

    }

}
