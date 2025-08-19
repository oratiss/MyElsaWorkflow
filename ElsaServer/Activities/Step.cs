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
            long performerUserId,
            PerformerGroup activePerformerGroup,
            Input<List<RequiredFieldValue>> requiredFieldValues,
            MemoryBlockReference output,
            long? ToEntityStateId = null,
            [CallerFilePath] string? source = null,
            [CallerLineNumber] int? line = null,
            string? description = null) : base(output, source, line)
        {
        }

        public Step(
            long performerUserId,
            PerformerGroup activePerformerGroup,
            Input<List<RequiredFieldValue>> requiredFieldValues,
            string taskName,
            long? ToEntityStateId = null,
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

        public PerformerGroup performerGroup { get; set; } = null!;

        public long PerformerUserId { get; set; }

        public Input<List<RequiredFieldValue>> RequiredFieldValues { get; set; } = null!;

        public string? Description { get; set; } = null;

        public long? ToEntityStateId { get; set; }

        public object? PossibleRequiredData { get; set; }
        
    }
}
