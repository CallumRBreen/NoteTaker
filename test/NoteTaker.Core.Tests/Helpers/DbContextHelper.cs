using Microsoft.EntityFrameworkCore;
using NoteTaker.DAL;

namespace NoteTaker.Core.Tests.Helpers
{
    public static class DbContextHelper
    {
        public static DbContextOptions<NoteTakerContext> GetTestInMemoryDatabase(string testDatabaseName)
        {
            return new DbContextOptionsBuilder<NoteTakerContext>().UseInMemoryDatabase(testDatabaseName).Options;
        }
    }
}
