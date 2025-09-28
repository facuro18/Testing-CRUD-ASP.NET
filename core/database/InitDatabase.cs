namespace Testing_CRUD.core.database
{
    public class InitDatabase
    {
        public static void StartDatabase()
        {
            // Initialize database connection and setup
            // This method will be called when the application starts
            MySqlDatabase.Connect();
        }
    }
}
