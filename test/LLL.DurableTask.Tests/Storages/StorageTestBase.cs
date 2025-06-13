﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using DurableTask.Core;
using DurableTask.Core.History;
using DurableTask.Core.Query;
using LLL.DurableTask.Core;
using LLL.DurableTask.Core.Serializing;
using LLL.DurableTask.EFCore.Polling;
using LLL.DurableTask.Tests.Storage.Activities;
using LLL.DurableTask.Tests.Storage.Orchestrations;
using LLL.DurableTask.Worker.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace LLL.DurableTask.Tests.Storages;

public abstract class StorageTestBase : IAsyncLifetime
{
    private readonly ITestOutputHelper _output;
    protected IHost _host;
    protected IConfiguration Configuration { get; }
    protected TimeSpan FastWaitTimeout { get; set; } = TimeSpan.FromSeconds(20);
    protected TimeSpan SlowWaitTimeout { get; set; } = TimeSpan.FromSeconds(30);
    protected bool SupportsMultipleExecutionStorage { get; set; } = true;
    protected bool SupportsTags { get; set; } = true;
    protected bool SupportsEventsAfterCompletion { get; set; } = true;

    public StorageTestBase(ITestOutputHelper output)
    {
        _output = output;

        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile("appsettings.private.json", true)
            .AddEnvironmentVariables()
            .Build();
    }

    public virtual async Task InitializeAsync()
    {
        _host = await Host.CreateDefaultBuilder()
            .ConfigureLogging(logging =>
            {
                logging.AddFilter(l => l >= LogLevel.Warning).AddXUnit(_output);
            })
            .ConfigureServices(services =>
            {
                ConfigureStorage(services);

                services.AddDurableTaskClient();

                services.AddDurableTaskWorker(builder =>
                {
                    ConigureWorker(builder);
                });
            }).StartAsync();
    }

    protected abstract void ConfigureStorage(IServiceCollection services);

    protected virtual void ConigureWorker(IDurableTaskWorkerBuilder builder)
    {
        builder.AddOrchestration<EmptyOrchestration>(EmptyOrchestration.Name, EmptyOrchestration.Version);
        builder.AddOrchestration<TimerOrchestration>(TimerOrchestration.Name, TimerOrchestration.Version);
        builder.AddOrchestration<ContinueAsNewOrchestration>(ContinueAsNewOrchestration.Name, ContinueAsNewOrchestration.Version);
        builder.AddOrchestration<ContinueAsNewEmptyOrchestration>(ContinueAsNewEmptyOrchestration.Name, ContinueAsNewEmptyOrchestration.Version);
        builder.AddOrchestration<ParentOrchestration>(ParentOrchestration.Name, ParentOrchestration.Version);
        builder.AddOrchestration<ParallelTasksOrchestration>(ParallelTasksOrchestration.Name, ParallelTasksOrchestration.Version);
        builder.AddOrchestration<WaitForEventOrchestration>(WaitForEventOrchestration.Name, WaitForEventOrchestration.Version);
        builder.AddOrchestration<FibonacciRecursiveOrchestration>(FibonacciRecursiveOrchestration.Name, FibonacciRecursiveOrchestration.Version);
        builder.AddActivity<SumActivity>(SumActivity.Name, SumActivity.Version);
        builder.AddActivity<SubtractActivity>(SubtractActivity.Name, SubtractActivity.Version);
        builder.AddActivity<MeasuredDelayActivity>(MeasuredDelayActivity.Name, MeasuredDelayActivity.Version);
    }

    public virtual async Task DisposeAsync()
    {
        await _host.StopAsync();
        await _host.WaitForShutdownAsync();
        _host.Dispose();
    }

    [Trait("Category", "Integration")]
    [SkippableFact]
    public async Task EmptyOrchestration_ShouldComplete()
    {
        var taskHubClient = _host.Services.GetRequiredService<TaskHubClient>();

        var input = Guid.NewGuid();

        var instance = await taskHubClient.CreateOrchestrationInstanceAsync(
            EmptyOrchestration.Name,
            EmptyOrchestration.Version,
            input);

        var state = await taskHubClient.WaitForOrchestrationAsync(instance, FastWaitTimeout);

        state.Should().NotBeNull();
        state.Output.Should().Be($"\"{input}\"");
        state.OrchestrationStatus.Should().Be(OrchestrationStatus.Completed);
    }

    [Trait("Category", "Integration")]
    [SkippableFact]
    public async Task EventsAfterCompletion_ShouldBeProcessed()
    {
        Skip.IfNot(SupportsEventsAfterCompletion, "Doesn't support events after completion");

        var taskHubClient = _host.Services.GetRequiredService<TaskHubClient>();

        var input = Guid.NewGuid();

        var instance = await taskHubClient.CreateOrchestrationInstanceAsync(
            EmptyOrchestration.Name,
            EmptyOrchestration.Version,
            input);

        var state = await taskHubClient.WaitForOrchestrationAsync(instance, FastWaitTimeout);

        state.Should().NotBeNull();
        state.OrchestrationStatus.Should().Be(OrchestrationStatus.Completed);

        await taskHubClient.RaiseEventAsync(instance, "SetResult", Guid.NewGuid());

        state = await BackoffPollingHelper.PollAsync(
            async () => await taskHubClient.WaitForOrchestrationAsync(instance, FastWaitTimeout),
            x => x != null && x.OrchestrationStatus == OrchestrationStatus.ContinuedAsNew,
            FastWaitTimeout,
            new PollingIntervalOptions(10, 1.2, 100),
            CancellationToken.None);

        state.Should().NotBeNull();
        state.OrchestrationStatus.Should().Be(OrchestrationStatus.ContinuedAsNew);
    }

    [Trait("Category", "Integration")]
    [SkippableFact]
    public async Task TimerOrchestration_ShouldComplete()
    {
        var taskHubClient = _host.Services.GetRequiredService<TaskHubClient>();

        var input = Guid.NewGuid();

        var instance = await taskHubClient.CreateOrchestrationInstanceAsync(
            TimerOrchestration.Name,
            TimerOrchestration.Version,
            input);

        var state = await taskHubClient.WaitForOrchestrationAsync(instance, FastWaitTimeout);

        state.Should().NotBeNull();
        state.Output.Should().Be($"\"{input}\"");
        state.OrchestrationStatus.Should().Be(OrchestrationStatus.Completed);
    }

    [Trait("Category", "Integration")]
    [SkippableFact]
    public async Task TimerOrchestration_ShouldTerminateProperly()
    {
        var taskHubClient = _host.Services.GetRequiredService<TaskHubClient>();

        var input = Guid.NewGuid();

        var instance = await taskHubClient.CreateOrchestrationInstanceAsync(
            TimerOrchestration.Name,
            TimerOrchestration.Version,
            input);

        await Task.Delay(TimeSpan.FromSeconds(1));

        await taskHubClient.TerminateInstanceAsync(instance);

        var state = await taskHubClient.WaitForOrchestrationAsync(instance, FastWaitTimeout);

        state.Should().NotBeNull();
        state.OrchestrationStatus.Should().Be(OrchestrationStatus.Terminated);

        await Task.Delay(TimeSpan.FromSeconds(3));

        var history = await taskHubClient.GetOrchestrationHistoryAsync(instance);

        var events = new TypelessJsonDataConverter().Deserialize<HistoryEvent[]>(history);
        events.Should().NotContain(x => x.EventType == EventType.TimerFired);
    }

    [Trait("Category", "Integration")]
    [SkippableTheory]
    [InlineData(ContinueAsNewEmptyOrchestration.Name, ContinueAsNewEmptyOrchestration.Version)]
    [InlineData(ContinueAsNewOrchestration.Name, ContinueAsNewOrchestration.Version)]
    public async Task ContinueAsNewOrchestration_ShouldComplete(string name, string version)
    {
        var taskHubClient = _host.Services.GetRequiredService<TaskHubClient>();

        var instance = await taskHubClient.CreateOrchestrationInstanceAsync(name, version, 5);

        if (SupportsMultipleExecutionStorage)
        {
            var firstExecutionState = await taskHubClient.WaitForOrchestrationAsync(instance, FastWaitTimeout);
            firstExecutionState.Should().NotBeNull();
            firstExecutionState.Output.Should().Be("4");
            firstExecutionState.OrchestrationStatus.Should().Be(OrchestrationStatus.ContinuedAsNew);
        }

        var lastExecution = new OrchestrationInstance { InstanceId = instance.InstanceId };
        var lastExecutionState = await taskHubClient.WaitForOrchestrationAsync(lastExecution, FastWaitTimeout);
        lastExecutionState.Should().NotBeNull();
        lastExecutionState.Output.Should().Be("0");
        lastExecutionState.OrchestrationStatus.Should().Be(OrchestrationStatus.Completed);
    }

    [Trait("Category", "Integration")]
    [SkippableFact]
    public async Task ParentOrchestration_ShouldComplete()
    {
        var taskHubClient = _host.Services.GetRequiredService<TaskHubClient>();

        var instance = await taskHubClient.CreateOrchestrationInstanceAsync(
            ParentOrchestration.Name,
            ParentOrchestration.Version,
            5);

        var state = await taskHubClient.WaitForOrchestrationAsync(instance, FastWaitTimeout);

        state.Should().NotBeNull();
        state.Output.Should().Be("5");
        state.OrchestrationStatus.Should().Be(OrchestrationStatus.Completed);
    }

    [Trait("Category", "Integration")]
    [SkippableFact]
    public async Task ParallelTasksOrchestration_ShouldExecuteInParallel()
    {
        var taskHubClient = _host.Services.GetRequiredService<TaskHubClient>();
        var orchestrationService = _host.Services.GetRequiredService<IOrchestrationService>();

        var instance = await taskHubClient.CreateOrchestrationInstanceAsync(
            ParallelTasksOrchestration.Name,
            ParallelTasksOrchestration.Version,
            orchestrationService.MaxConcurrentTaskActivityWorkItems);

        var state = await taskHubClient.WaitForOrchestrationAsync(instance, FastWaitTimeout);

        state.Should().NotBeNull();
        state.Output.Should().Be(orchestrationService.MaxConcurrentTaskActivityWorkItems.ToString());
        state.OrchestrationStatus.Should().Be(OrchestrationStatus.Completed);
    }

    [Trait("Category", "Integration")]
    [SkippableFact]
    public async Task WaitEventOrchestration_ShouldComplete()
    {
        var taskHubClient = _host.Services.GetRequiredService<TaskHubClient>();

        var eventData = Guid.NewGuid();

        var instance = await taskHubClient.CreateOrchestrationInstanceAsync(
            WaitForEventOrchestration.Name,
            WaitForEventOrchestration.Version,
            null);

        await taskHubClient.RaiseEventAsync(instance, "SetResult", eventData);

        var state = await taskHubClient.WaitForOrchestrationAsync(instance, FastWaitTimeout);

        state.Should().NotBeNull();
        state.Output.Should().Be($"\"{eventData}\"");
        state.OrchestrationStatus.Should().Be(OrchestrationStatus.Completed);
    }

    [Trait("Category", "Integration")]
    [SkippableFact]
    public async Task FibonacciOrchestration_ShouldComplete()
    {
        var taskHubClient = _host.Services.GetRequiredService<TaskHubClient>();

        var instance = await taskHubClient.CreateOrchestrationInstanceAsync(
            FibonacciRecursiveOrchestration.Name,
            FibonacciRecursiveOrchestration.Version,
            Guid.NewGuid().ToString(),
            2);

        var state = await taskHubClient.WaitForOrchestrationAsync(instance, SlowWaitTimeout);

        state.Should().NotBeNull();
        state.Output.Should().Be("1");
        state.OrchestrationStatus.Should().Be(OrchestrationStatus.Completed);
    }

    [Trait("Category", "Integration")]
    [SkippableFact]
    public async Task Tags_ShouldBeStoredAndRetrieved()
    {
        Skip.IfNot(SupportsTags, "Tags not supported");

        var taskHubClient = _host.Services.GetRequiredService<TaskHubClient>();

        var tags = new Dictionary<string, string> {
            { "Tag1", "Value1" },
            { "Tag2", "Value2" }
        };

        var instance = await taskHubClient.CreateOrchestrationInstanceAsync(
            EmptyOrchestration.Name,
            EmptyOrchestration.Version,
            Guid.NewGuid().ToString(),
            string.Empty,
            tags);

        var state = await taskHubClient.GetOrchestrationStateAsync(instance);
        state.Should().NotBeNull();
        state.Tags.Should().BeEquivalentTo(tags);
    }

    [Trait("Category", "Integration")]
    [SkippableFact]
    public async Task PurgeInstanceStateWithInstanceId_ShouldPurge()
    {
        var taskHubClient = _host.Services.GetRequiredService<TaskHubClient>();
        var purgeClient = _host.Services.GetService<IOrchestrationServicePurgeClient>();
        Skip.If(purgeClient == null, "Purge instance not supported");

        var instance = await taskHubClient.CreateOrchestrationInstanceAsync(
            EmptyOrchestration.Name,
            EmptyOrchestration.Version,
            string.Empty);

        var state = await taskHubClient.GetOrchestrationStateAsync(instance.InstanceId);
        state.Should().NotBeNull();

        await purgeClient.PurgeInstanceStateAsync(instance.InstanceId);

        var stateAfterPurge = await taskHubClient.GetOrchestrationStateAsync(instance.InstanceId);
        stateAfterPurge.Should().BeNull();
    }

    [Trait("Category", "Integration")]
    [SkippableFact]
    public async Task PurgeInstanceStateWithFilter_ShouldPurge()
    {
        var taskHubClient = _host.Services.GetRequiredService<TaskHubClient>();
        var purgeClient = _host.Services.GetService<IOrchestrationServicePurgeClient>();
        Skip.If(purgeClient == null, "Purge instance not supported");

        var instance = await taskHubClient.CreateOrchestrationInstanceAsync(
            EmptyOrchestration.Name,
            EmptyOrchestration.Version,
            string.Empty);

        var state = await taskHubClient.GetOrchestrationStateAsync(instance.InstanceId);
        state.Should().NotBeNull();

        var filter = new PurgeInstanceFilterExtended(DateTime.UnixEpoch, DateTime.UtcNow, new[] {
            OrchestrationStatus.Pending,
            OrchestrationStatus.Running,
            OrchestrationStatus.Completed,
            OrchestrationStatus.ContinuedAsNew,
            OrchestrationStatus.Failed,
            OrchestrationStatus.Canceled,
            OrchestrationStatus.Terminated,
            OrchestrationStatus.Suspended,
        })
        {
            Limit = 1000
        };

        var purgeResult = await purgeClient.PurgeInstanceStateAsync(filter);
        purgeResult.DeletedInstanceCount.Should().BeGreaterThan(0);

        var stateAfterPurge = await taskHubClient.GetOrchestrationStateAsync(instance.InstanceId);
        stateAfterPurge.Should().BeNull();
    }

    [Trait("Category", "Integration")]
    [SkippableFact]
    public async Task GetOrchestrationsWithQuery_ShouldNotError()
    {
        var taskHubClient = _host.Services.GetRequiredService<TaskHubClient>();
        var queryClient = _host.Services.GetService<IOrchestrationServiceQueryClient>();
        Skip.If(queryClient == null, "Query client instance not supported");
        _ = await taskHubClient.CreateOrchestrationInstanceAsync(
            EmptyOrchestration.Name,
            EmptyOrchestration.Version,
            string.Empty,
            new { },
            new Dictionary<string, string> {
                { "key-a", "value-a" },
                { "key-b", "value-b" }
            });

        var query = new OrchestrationQueryExtended
        {
            Tags = {
                { "key-a", "value-a" },
                { "key-b", "value-b" }
            }
        };
        var queryResult = await queryClient.GetOrchestrationWithQueryAsync(query, default);

        queryResult.OrchestrationState.Should().HaveCountGreaterThan(0);
    }
}
