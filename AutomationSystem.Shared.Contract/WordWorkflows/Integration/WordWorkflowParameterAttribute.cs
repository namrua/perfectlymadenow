using System;

namespace AutomationSystem.Shared.Contract.WordWorkflows.Integration
{
    /// <summary>
    /// Annotates Word workflow parameter property
    /// </summary>
    public class WordWorkflowParameterAttribute : Attribute
    {

        // name of parameter
        public string Name { get; set; }

        // constructor
        public WordWorkflowParameterAttribute(string name)
        {
            Name = name;
        }

    }
}
