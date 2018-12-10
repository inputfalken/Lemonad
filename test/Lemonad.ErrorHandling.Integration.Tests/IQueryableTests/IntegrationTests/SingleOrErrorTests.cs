using System.Linq;
using DatabaseManager;
using EntityFramework;

namespace IntegrationTests {
    public class SingleOrErrorTests {
        private static MovieContext MovieContext { get; }

        static SingleOrErrorTests() {
            MovieContext = new MovieContext();
            new MovieDatabaseManager(MovieContext).BuildIntegrationTestDatabase(
                () => MovieContext.Users.Count() == 6667
            );
        }
    }
}