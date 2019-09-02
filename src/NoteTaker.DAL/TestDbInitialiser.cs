using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NoteTaker.DAL.Utilities;

namespace NoteTaker.DAL
{
    public static class TestDbInitialiser
    {
        public static void Initialise(IServiceProvider serviceProvider)
        {
            using (var context = new NoteTakerContext(serviceProvider.GetRequiredService<DbContextOptions<NoteTakerContext>>()))
            {
                if (context.Notes.Any())
                {
                    return;
                }

                var notes = FakeDataHelper.GetNotes(200);

                context.Notes.AddRange(notes);

                context.SaveChanges();
            }
        }
    }
}
