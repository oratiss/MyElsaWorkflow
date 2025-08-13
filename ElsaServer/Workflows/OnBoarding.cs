using Acornima.Ast;
using Elsa.Api.Client.Extensions;
using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Runtime.Activities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading;

namespace ElsaServer.Workflows
{
    public class Some
    {
        public string Name { get; set; } = null!;
    }

    public class Other
    {
        public decimal Amount { get; set; }
    }

    public class OnBoarding : WorkflowBase
    {
        protected override void Build(IWorkflowBuilder builder)
        {
            var employee = builder.WithVariable<object>();

            var previousRunTasKResultAsInput = builder.WithVariable<Some?>();

            builder.Root = new Sequence
            {
                Activities =
                {
                    new Start(),
                    new SetVariable
                    {
                        Variable = employee,
                        Value = new (context => context.GetInput("Employee"))
                    },
                    new RunTask("Create Email Account")
                    {
                        Payload = new (context =>
                        {
                            Other sampleOther = new()
                            {
                                Amount = 100_000m
                            };

                            return new Dictionary<string, object>
                            {
                                ["Employee"] = employee.Get(context)!,
                                ["Description"] = "Create an email account for the new employee.",  
                                ["Other"] = sampleOther
                            };
                        })

                    },

                    new SetVariable
                    {
                        Variable = previousRunTasKResultAsInput,
                        Value = new (context =>
                        {
                            var input = context.GetWorkflowExecutionContext().Input;
                            var runTaskInput = input["RunTaskInput"];
                            var some =  runTaskInput.ConvertTo<Some>();
                            return some;
                        })
                    },

                    new WriteLine(context =>
                    {
                        var currentValue = previousRunTasKResultAsInput.Get(context);

                        return $"Name is: {currentValue?.Name}";
                    }),

                    new RunTask("Create Slack Account")
                    {
                        Payload = new (context =>
                        {
                            Other sampleOther = new()
                            {
                                Amount = 30_000_000_000m
                            };
                             
                            return new Dictionary<string, object>
                            {
                                ["Employee"] = employee.Get(context)!,
                                ["Description"] = "Create a Slack account for the new employee.",
                                ["Other"] = sampleOther
                            };
                        })
                    },


                    new Elsa.Workflows.Activities.Parallel
                    {
                        Activities =
                        {
                            //new RunTask("Create Slack Account")
                            //{
                            //    Payload = new (context => new Dictionary<string, object>
                            //    {
                            //        ["Employee"] = employee.Get(context)!,
                            //        ["Description"] = "Create a Slack account for the new employee."
                            //    })
                            //},
                            new RunTask("Create GitHub Account")
                            {
                                Payload = new(context => new Dictionary<string, object>
                                {
                                    ["Employee"] = employee.Get(context)!,
                                    ["Description"] = "Create a GitHub account for the new employee."
                                })
                            },
                            new RunTask("Add to HR System")
                            {
                                Payload = new(context => new Dictionary<string, object>
                                {
                                    ["Employee"] = employee.Get(context)!,
                                    ["Description"] = "Add the new employee to the HR system."
                                })
                            }
                        }
                    },
                    new End()

                }
            };
        }
    }
}
