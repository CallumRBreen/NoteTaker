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
                yield return new Note(faker.Lorem.Lines(1), faker.Lorem.Paragraphs(2));
            }
        }
    }
}
