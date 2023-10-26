using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.System.Model
{
    public class ClassWithClassForm
    {
        public Class Class { get; set; }
        public ClassForm ClassForm { get; set; }

        public ClassWithClassForm(Class cls, ClassForm classForm)
        {
            Class = cls;
            ClassForm = classForm;
        }
    }
}
