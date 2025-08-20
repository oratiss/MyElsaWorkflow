using Elsa.Expressions.Models;
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
            List<RequiredFieldValue> requiredFieldValues,
            MemoryBlockReference output,
            [CallerFilePath] string? source = null,
            [CallerLineNumber] int? line = null,
            string? description = null) : base(output, source, line)
        {
        }

        public Step(
            Guid performerUserId,
            UserGroup activePerformerGroup,
            List<RequiredFieldValue> requiredFieldValues,
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

        public List<RequiredFieldValue> RequiredFieldValues { get; set; } = null!;

        public string? Description { get; set; } = null;

        public object? PossibleRequiredData { get; set; }
        
    }
}
