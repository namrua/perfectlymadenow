using System;
using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Actions
{
    /// <summary>
    /// Class action list item
    /// </summary>
    public class ClassActionListItem
    {
        [DisplayName("ID")]
        public long ClassActionId { get; set; }

        [DisplayName("Class ID")]
        public long ClassId { get; set; }

        [DisplayName("Class action type code")]
        public ClassActionTypeEnum ClassActionTypeId { get; set; }

        [DisplayName("Class action type")]
        public string ClassActionType { get; set; }

        public bool IsProcessed { get; set; }

        [DisplayName("Processed")]
        public DateTime? Processed { get; set; }
    }
}