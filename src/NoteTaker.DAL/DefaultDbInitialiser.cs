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
                    var user = new User("CallumBreen", "Callum", "Breen", "$2b$10$HYrAy.9xP9f99i75tsdLXe9PYZ3/vu5nEwQNoKlQQEiBfXZFl.mf6");

                    context.Users.Add(user);

                    var notes = FakeDataHelper.GetNotes(user, 200).ToList();

                    context.Notes.AddRange(notes);

                    context.SaveChanges();
                }
            }
        }
    }
}
