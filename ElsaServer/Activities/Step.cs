using Elsa.Expressions.Models;
using Elsa.Workflows;
using Elsa.Workflows.Models;
using Elsa.Workflows.Runtime.Activities;
using Rts.Common;
using System.Runtime.CompilerServices;

namespace ElsaServer.Activities
{
    public class Step : RunTask
    {
        public Step(
            Guid performerUserId,
            UserGroup activePerformerGroup,
            List<Dictionary<string, object>> requiredFieldValues,
            MemoryBlockReference output,
            [CallerFilePath] string? source = null,
            [CallerLineNumber] int? line = null,
            string? description = null) : base(output, source, line)
        {
        }

        public Step(
            Guid performerUserId,
            UserGroup activePerformerGroup,
            List<Dictionary<string, object>> requiredFieldValues,
            string taskName,
            [CallerFilePath] string? source = null,
            [CallerLineNumber] int? line = null,
            string? description = null) : base(taskName, source, line)
        {
        }

        public Step(string taskName,
            [CallerFilePath] string? source = null,
            [CallerLineNumber] int? line = null,
            string? description = null) : base(taskName, source, line)
        {

        }



        public UserGroup performerGroup { get; set; } = null!;

        public Guid PerformerUserId { get; set; }

        public Dictionary<string, object> RequiredFieldValues { get; set; } = null!;

        public string? Description { get; set; } = null;

        public object? PossibleRequiredData { get; set; }

        protected override ValueTask ExecuteAsync(ActivityExecutionContext context)
        {
            var userWorkflowConfig = context.Variables.FirstOrDefault(x => x.Name == "userWorkflowConfig")!.Value as UserWorkflowConfig;
            PerformerUserId = userWorkflowConfig!.FirstActivityConfig.CurrentPerformerUser.Id;
            performerGroup = userWorkflowConfig!.FirstActivityConfig.CurrentPerformerGroup;
            RequiredFieldValues = userWorkflowConfig!.FirstActivityConfig.RequiredFieldValues!;
            return base.ExecuteAsync(context);
        }
        
    }
}
