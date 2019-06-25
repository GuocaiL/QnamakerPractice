using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Model_UpdateTableEntity;
using System.Threading.Tasks;

namespace Controllertable_Controller
{
    public class CotrollerTable
    {
        private static string tablename="qnabotone83cd";

        private static string tablepsw =
            "p0DgQIyWJvdPJOr/Z9mRpJYmc8lUe90FrS0Jesj/iEVX4UghuEYzwzFzfunY9lXZkNjrOok1OYcLcZhTgMh2vg==";

        // Retrieve the storage account from the connection string.      
        private static CloudStorageAccount storageAccount = new CloudStorageAccount(
            new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(tablename,tablepsw),true);

        // Create the table client.
        private static CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

        // Create the CloudTable object that represents the "people" table.
        private static CloudTable table = tableClient.GetTableReference("UpdateTable");

        public static async void CreatePeopleTableAsync()
        {
            // Create the CloudTable if it does not exist
            await table.CreateIfNotExistsAsync();
        }

        //If query successed return ture and others false
        public static async Task<bool> Query(string question,string answer)
        {
            CreatePeopleTableAsync();

            bool query = false;

            // Create a retrieve operation that takes a customer entity.
            //TableOperation retrieveOperation = TableOperation.Retrieve<UpdateTableEntity>("Smith", "Ben");
            TableOperation retrieveOperation = TableOperation.Retrieve<UpdateTableEntity>(question ,answer);

            // Execute the retrieve operation.
            TableResult retrievedResult =await table.ExecuteAsync(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result== null)
            {
                query = true;
            }
            return query;
        }

        //Insert entity
        public static async void Insert(string question,string answer,string fromname)
        {
            UpdateTableEntity Entity = new UpdateTableEntity(question,answer);
            Entity.FromName = fromname;
            TableOperation insertopretion = TableOperation.Insert(Entity);
            await table.ExecuteAsync(insertopretion );
        }      
    }
}
