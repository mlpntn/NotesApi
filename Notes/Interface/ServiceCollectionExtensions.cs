using Microsoft.Extensions.DependencyInjection;
using Notes.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Interface
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterRepoServices(this IServiceCollection services)
        {
            services.AddTransient<INotesRepo, NotesRepo>();
            return services;
        }
    }
}
