using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NoteTaker.DAL;
using NoteTaker.DAL.Entities;

namespace NoteTaker.API.Tests.TestHelpers
{
    public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<NoteTakerContext>(options =>
                {
                    options.UseInMemoryDatabase("NoteTakerInMemoryForIntegrationTests");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<NoteTakerContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<TestWebApplicationFactory<TStartup>>>();

                    context.Database.EnsureCreated();

                    try
                    {
                        InitialiseTestData(context);
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex, $"Error occured when starting {nameof(TestWebApplicationFactory<TStartup>)}");
                    }
                }
            });
        }

        private static void InitialiseTestData(NoteTakerContext context)
        {
            var notes = GetNotes(50).ToList();

            notes[0].Content = "Apples";

            context.Notes.AddRange(notes);

            context.SaveChanges();
        }

        private static IEnumerable<Note> GetNotes(int count)
        {
            var faker = new Faker();

            for (int i = 0; i < count; i++)
            {
                yield return new Note()
                {
                    Id = Guid.NewGuid().ToString(),
                    InternalId = i,
                    Title = faker.Lorem.Lines(1),
                    Content = faker.Lorem.Paragraphs(2),
                    Created = DateTime.Now.AddDays(-7),
                    Modified = DateTime.Now
                };
            }
        }
    }
}
