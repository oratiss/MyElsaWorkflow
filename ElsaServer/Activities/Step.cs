using Elsa.Expressions.Models;
using Elsa.Workflows.Runtime.Activities;
using ElsaServer.Models;
using System.Runtime.CompilerServices;

namespace ElsaServer.Activities
{
    public class Step : RunTask
    {
        public Step(
            long toEntityStateId,
            PerformerGroup activePerformerGroup,
            Dictionary<string, object> requiredFields,
            Dictionary<Step, PerformerGroup> nextPossibleSteps,
            MemoryBlockReference output,
            [CallerFilePath] string? source = null,
            [CallerLineNumber] int? line = null,
            string? description = null) : base(output, source, line)
        {
        }

        public Step(
            long toEntityStateId,
            PerformerGroup activePerformerGroup,
            Dictionary<string, object> requiredFields,
            Dictionary<Step, PerformerGroup> nextPossibleSteps,
            string taskName,
            [CallerFilePath] string? source = null,
            [CallerLineNumber] int? line = null,
            string? description = null) : base(taskName, source, line)
        {
        }

        public Step(string taskName,
            [CallerFilePath] string? source = null,
            [CallerLineNumber] int? line = null,
            string? description = null):base(taskName, source, line) 
        {
            
        }

        public PerformerGroup performerGroup { get; set; } = null!;

        public Dictionary<Step, PerformerGroup> NextPossibleSteps { get; set; } = null!;

        public Dictionary<string, object> RequiredFields { get; set; } = null!;

        public string? Description { get; set; } = null;

        public long ToEntityStateId { get; set; }
    }
}
