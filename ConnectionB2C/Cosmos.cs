using Microsoft.Azure.Cosmos;

using AngryMonkey.CloudLogin;

namespace ConnectionB2C
{
    public class Cosmos
    {
        public Microsoft.Azure.Cosmos.Container container;
        public Cosmos(string connectionString, string databaseId, string containerId)
        {
            CosmosClient client = new(connectionString, new CosmosClientOptions() { SerializerOptions = new() { IgnoreNullValues = true } });

            container = client.GetContainer(databaseId, containerId);
        }

        public async Task CreateUser(UserModel user)
        {
            Model.User selectedUser = new()
            {
                ID = new Guid(user.ID),
                FirstName = user.FirstName,
                LastName = user.LastName,
                DisplayName = user.DisplayName,
                CreatedOn = user.CreatedOn ?? DateTime.UtcNow,
                LastSignedIn = user.LastSignedIn ?? user.CreatedOn ?? DateTime.UtcNow,

                Inputs = new()
                {
                    new()
                    {
                        Input = user.Email,
                        Format = InputFormat.EmailAddress,
                        IsPrimary = true,
                        Providers = null
                    }
                }
            };

            await container.CreateItemAsync(selectedUser);
        }
    }
}
