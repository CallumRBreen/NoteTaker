using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NoteTaker.DAL.Entities;
using NoteTaker.DAL.Utilities;

namespace NoteTaker.DAL
{
    public static class DefaultDbInitialiser
    {

        public static void Initialise(IServiceProvider serviceProvider)
        {
            using (var context = new NoteTakerContext(serviceProvider.GetRequiredService<DbContextOptions<NoteTakerContext>>(), serviceProvider.GetRequiredService<IHttpContextAccessor>()))
            {
                if (context.Users.Any())
                {
                    return;
                }

                if (!context.Notes.Any())
                {
                    // password is apples
                    var user = new User("CallumBreen", "Callum", "Breen", "$2b$10$m59R6F0ubBmQKMxksCizJ.zdToQ5wCLdwTM3S1HA5GyqjwhmC5fri");

                    context.Users.Add(user);

                    var notes = FakeDataHelper.GetNotes(user, 200).ToList();

                    context.Notes.AddRange(notes);

                    context.SaveChanges();
                }
            }
        }
    }
}
