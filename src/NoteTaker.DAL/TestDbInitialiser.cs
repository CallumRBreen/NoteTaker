using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NoteTaker.DAL.Entities;
using NoteTaker.DAL.Utilities;

namespace NoteTaker.DAL
{
    public static class TestDbInitialiser
    {
        public static void Initialise(IServiceProvider serviceProvider)
        {
            using (var context = new NoteTakerContext(serviceProvider.GetRequiredService<DbContextOptions<NoteTakerContext>>()))
            {
                if (!context.Notes.Any())
                {
                    var notes = FakeDataHelper.GetNotes(200).ToList();

                    context.Notes.AddRange(notes);

                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    var user = new User("CallumBreen", "Callum", "Breen", "");
                }
            }
        }
    }
}
