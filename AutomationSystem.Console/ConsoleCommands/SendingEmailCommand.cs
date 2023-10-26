using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Console.ConsoleCommands.Models;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;

namespace AutomationSystem.Console.ConsoleCommands
{
    public class SendingEmailCommand : IConsoleCommand<SendingEmailParameters>
    {
        private readonly ICoreAsyncRequestManager coreAsyncRequestManager;

        public SendingEmailCommand(ICoreAsyncRequestManager coreAsyncRequestManager)
        {
            this.coreAsyncRequestManager = coreAsyncRequestManager;
        }

        public void Execute(SendingEmailParameters commandParameters)
        {
            List<long> emailIds;
            using (var context = new CoreEntities())
            {
                emailIds = context.Emails
                    .Active().Where(x => !x.IsSent)
                    .Where(x => !commandParameters.FromDate.HasValue || commandParameters.FromDate.Value <= x.Created)
                    .Where(x => !commandParameters.ToDate.HasValue ||  x.Created < commandParameters.ToDate.Value)
                    .Select(x => x.EmailId).ToList();
            }

            var count = emailIds.Count;
            var sentItems = 0;
            System.Console.WriteLine($"{count} emails to resend");
            foreach (var emailId in emailIds)
            {
                coreAsyncRequestManager.AddSendEmailRequest(emailId, (int) SeverityEnum.Fatal);
                System.Console.WriteLine($"({++sentItems}/{count}) - Email with id {emailId} was sent");
            }

            System.Console.WriteLine("Sending completed");
        }
    }
}
