﻿using System;
using DurableTask.Core;
using LLL.DurableTask.Core;
using LLL.DurableTask.Server.Client;
using Microsoft.Extensions.Options;
using static DurableTaskHub.OrchestrationService;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DurableTaskGrpcServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddDurableTaskServerStorageGrpc(
            this IServiceCollection services,
            Action<GrpcOrchestrationServiceOptions> configure)
        {
            services.AddOptions<GrpcOrchestrationServiceOptions>()
                .Configure(configure);

            services.AddSingleton<GrpcOrchestrationService>();
            services.AddSingleton<IOrchestrationService>(p => p.GetRequiredService<GrpcOrchestrationService>());
            services.AddSingleton<IExtendedOrchestrationService>(p => p.GetRequiredService<GrpcOrchestrationService>());

            services.AddSingleton<GrpcOrchestrationServiceClient>();
            services.AddSingleton<IOrchestrationServiceClient>(p => p.GetRequiredService<GrpcOrchestrationServiceClient>());
            services.AddSingleton<IExtendedOrchestrationServiceClient>(p => p.GetRequiredService<GrpcOrchestrationServiceClient>());

            return services.AddGrpcClient<OrchestrationServiceClient>((s, o) =>
            {
                var options = s.GetRequiredService<IOptions<GrpcOrchestrationServiceOptions>>().Value;
                o.Address = options.BaseAddress;
            });
        }
    }
}
