using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using NoteTaker.DAL.Entities;

namespace NoteTaker.DAL.Utilities
{
    public static class FakeDataHelper
    {
        public static IEnumerable<Note> GetNotes(int count = 10)
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
