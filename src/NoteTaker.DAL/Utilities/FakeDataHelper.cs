using System.Collections.Generic;
using Bogus;
using NoteTaker.DAL.Entities;

namespace NoteTaker.DAL.Utilities
{
    internal static class FakeDataHelper
    {
        public static IEnumerable<Note> GetNotes(User user, int count = 10)
        {
            var faker = new Faker();

            for (int i = 0; i < count; i++)
            {
                yield return new Note(faker.Lorem.Lines(1), faker.Lorem.Paragraphs(2)) {User = user};
            }
        }
    }
}
