using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NoteTaker.Core.Models;
using NoteTaker.DAL;
using Note = NoteTaker.DAL.Entities.Note;
using User = NoteTaker.DAL.Entities.User;

namespace NoteTaker.IntegrationTests.TestHelpers
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
                        InitialiseNotesControllerTestData(context);
                        InitialiseUsersControllerTestData(context);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Error occured when starting {nameof(TestWebApplicationFactory<TStartup>)}");
                    }
                }
            });

            builder.ConfigureAppConfiguration((ctx, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["Security:JwtSecret"] = TestJwtTokenHelper.JwtSecret
                });
            });
        }

        private static void InitialiseNotesControllerTestData(NoteTakerContext context)
        {
            var notes = GetNotes(50).ToList();

            notes[0].Title = "Apples";
            notes[1].Id = new Guid("11111111-1234-4133-8c69-40ca0509be6a");
            notes[2].Id = new Guid("22222222-4321-1234-4321-40ca0509be6a");
            notes[3].Id = new Guid("33333333-4321-1234-4321-40ca0509be6a");

            context.Notes.AddRange(notes);

            context.SaveChanges();
        }

        private static void InitialiseUsersControllerTestData(NoteTakerContext context)
        {
            // Password is Apples
            var user = new User("JohnSmith","John","Smith", "$2b$10$fMcZJqEdLYUkkV0RcOJR0eo.00oiYqpfXD1TH18Q8XEVvO4GVSiKe");

            context.Users.Add(user);

            context.SaveChanges();
        }

        private static IEnumerable<Note> GetNotes(int count)
        {
            var faker = new Faker();

            for (int i = 0; i < count; i++)
            {
                yield return new Note(faker.Lorem.Lines(1), faker.Lorem.Paragraphs(2));
            }
        }
    }
}
