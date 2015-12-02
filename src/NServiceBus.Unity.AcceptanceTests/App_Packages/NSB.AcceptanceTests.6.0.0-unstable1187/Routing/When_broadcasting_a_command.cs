﻿namespace NServiceBus.AcceptanceTests.Routing
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using NServiceBus.AcceptanceTesting;
    using NServiceBus.AcceptanceTests.EndpointTemplates;
    using NServiceBus.Routing;
    using NUnit.Framework;

    public class When_broadcasting_a_command : NServiceBusAcceptanceTest
    {
        [Test]
        public async Task Should_send_it_to_all_instances()
        {
            var context = await Scenario.Define<Context>()
                .WithEndpoint<Sender>(b => b.When(c => c.EndpointsStarted, (bus, c) => bus.Send(new Request())))
                .WithEndpoint<Receiver1>()
                .WithEndpoint<Receiver2>()
                .Done(c => c.Receiver1TimesCalled > 0 && c.Receiver2TimesCalled > 0)
                .Run();

            Assert.AreEqual(1, context.Receiver1TimesCalled);
            Assert.AreEqual(1, context.Receiver2TimesCalled);
        }

        public class Context : ScenarioContext
        {
            public int Receiver1TimesCalled { get; set; }
            public int Receiver2TimesCalled { get; set; }
        }

        public class Sender : EndpointConfigurationBuilder
        {
            public Sender()
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;

                File.WriteAllLines(Path.Combine(basePath, "DistributingACommand.Receiver.txt"), new[]
                {
                    "1:",
                    "2:"
                });

                EndpointSetup<DefaultServer>(c =>
                {
                    c.Routing().UseFileBasedEndpointInstanceLists().LookForFilesIn(basePath);
                    c.Routing().UnicastRoutingTable.AddStatic(typeof(Request), new EndpointName("DistributingACommand.Receiver"));
                    c.Routing().SetMessageDistributionStrategy(new AllInstancesDistributionStrategy(), t => t == typeof(Request));
                });
            }

            public class ResponseHandler : IHandleMessages<Response>
            {
                public Context Context { get; set; }

                public Task Handle(Response message, IMessageHandlerContext context)
                {
                    if (context.MessageHeaders[Headers.ReplyToAddress].Contains("Receiver-1"))
                    {
                        Context.Receiver1TimesCalled++;
                    }
                    else if (context.MessageHeaders[Headers.ReplyToAddress].Contains("Receiver-2"))
                    {
                        Context.Receiver2TimesCalled++;
                    }
                    return Task.FromResult(0);
                }
            }
        }

        public class Receiver1 : EndpointConfigurationBuilder
        {
            public Receiver1()
            {
                EndpointSetup<DefaultServer>(c =>
                {
                    c.EndpointName("DistributingACommand.Receiver");
                    c.ScaleOut().UniqueQueuePerEndpointInstance("1");
                });
            }

            public class MyMessageHandler : IHandleMessages<Request>
            {
                public Task Handle(Request message, IMessageHandlerContext context)
                {
                    return context.Reply(new Response
                    {
                        EndpointName = "Receiver1"
                    });
                }
            }
        }

        public class Receiver2 : EndpointConfigurationBuilder
        {
            public Receiver2()
            {
                EndpointSetup<DefaultServer>(c =>
                {
                    c.EndpointName("DistributingACommand.Receiver");
                    c.ScaleOut().UniqueQueuePerEndpointInstance("2");
                });
            }

            public class MyMessageHandler : IHandleMessages<Request>
            {
                public Task Handle(Request message, IMessageHandlerContext context)
                {
                    return context.Reply(new Response
                    {
                        EndpointName = "Receiver2"
                    });
                }
            }
        }

        [Serializable]
        public class Request : ICommand
        {
        }

        [Serializable]
        public class Response : IMessage
        {
            public string EndpointName { get; set; }
        }
    }
}