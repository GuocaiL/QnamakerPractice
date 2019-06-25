using Microsoft.WindowsAzure.Storage.Table;

namespace Model_UpdateTableEntity
{

    public class UpdateTableEntity:TableEntity
    {
        public UpdateTableEntity(string lastName, string firstName)
        {
            this.PartitionKey = lastName;
            this.RowKey = firstName;
        }

        public UpdateTableEntity() { }

        public string FromName { get; set; }

    }
}
