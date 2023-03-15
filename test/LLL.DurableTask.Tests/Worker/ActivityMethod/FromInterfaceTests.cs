using System;
using System.Threading.Tasks;
using DurableTask.Core;
using FluentAssertions;
using LLL.DurableTask.Worker.Attributes;
using LLL.DurableTask.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace LLL.DurableTask.Tests.Worker.ActivityMethod
{
    public class FromInterfaceTests : WorkerTestBase
    {
        public FromInterfaceTests(ITestOutputHelper output)
            : base(output)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddSingleton<SingletonClass>();
            services.AddScoped<ScopedClass>();
            services.AddTransient<TransientClass>();
        }

        protected override void ConfigureWorker(IDurableTaskWorkerBuilder builder)
        {
            base.ConfigureWorker(builder);

            builder.AddAnnotatedFromType(typeof(Orchestrations));
            builder.AddActivitiesFromInterface<IActivities, Activities>(true);
        }

        [Fact]
        public async Task ActivityMethodFromInterface_ShouldInjectConstructorDependencies()
        {
            var taskHubClient = _host.Services.GetRequiredService<TaskHubClient>();

            var instance = await taskHubClient.CreateOrchestrationInstanceAsync(nameof(Orchestrations.InvokeActivityFromInterface), "", null);

            var result = await taskHubClient.WaitForOrchestrationAsync(instance, TimeSpan.FromSeconds(5));

            result.Output.Should().Be("true");
        }

        public class SingletonClass { }
        public class ScopedClass { }
        public class TransientClass { }

        public interface IActivities
        {
            Task<bool> TestActivity();
        }

        public class Orchestrations
        {
            [Orchestration]
            public async Task<bool> InvokeActivityFromInterface(OrchestrationContext context)
            {
                var client = context.CreateClient<IActivities>(true);
                return await client.TestActivity();
            }
        }

        public class Activities : IActivities
        {
            private readonly SingletonClass _singleton;
            private readonly ScopedClass _scoped;
            private readonly TransientClass _transient;

            public Activities(
                SingletonClass singleton,
                ScopedClass scoped,
                TransientClass transient)
            {
                _singleton = singleton;
                _scoped = scoped;
                _transient = transient;
            }

            public Task<bool> TestActivity()
            {
                return Task.FromResult(_singleton != null && _scoped != null && _transient != null);
            }
        }
    }
}