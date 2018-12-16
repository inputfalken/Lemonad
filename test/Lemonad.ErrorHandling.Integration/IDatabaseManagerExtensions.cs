using System;

namespace Lemonad.ErrorHandling.Integration {
    public static class DatabaseManagerExtensions {
        public static void BuildIntegrationTestDatabase(this IDatabaseManager manager,
            Func<bool> skipCreationWhenDatabaseExistsAnd) {
            var databaseExists = manager.DatabaseExists();
            if (databaseExists && skipCreationWhenDatabaseExistsAnd()) return;
            if (databaseExists) manager.DeleteDatabase();
            manager.CreateAndSeedDatabase(20, 20000);
        }
    }
}