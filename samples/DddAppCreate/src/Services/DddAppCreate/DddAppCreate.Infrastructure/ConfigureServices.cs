// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DddAppCreate.Core;
using DddAppCreate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {

        services.AddScoped<IDddAppCreateDbContext, DddAppCreateDbContext>();
        services.AddDbContextPool<DddAppCreateDbContext>(options =>
        {
            options.UseSqlServer(connectionString, builder => builder.MigrationsAssembly("DddAppCreate.Infrastructure"))
            .EnableThreadSafetyChecks(false);
        });
    }
}