using Rts.Common;

namespace TaskManagementApplication.Entities
{
    public record StepPayload(UserWorkflowConfig UserWorkflowConfig, string Description, NextActivityTransistionType NextActivityTransistionType);

}
