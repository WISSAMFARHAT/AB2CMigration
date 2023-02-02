using AngryMonkey.CloudLogin.DataContract;
using Microsoft.Azure.Cosmos;

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
            CloudUser selectedUser = new()
            { 
                ID=new Guid(user.ID),
                FirstName =user.FirstName,
                LastName = user.LastName,
                DisplayName = user.DisplayName,
                Inputs = new()
                {
                    new()
                    {
                        Input = user.Email,
                        IsPrimary = true,
                        IsValidated= true,
                        Providers = new()
                        {
                            new()
                            {
                                Code = "",
                                Identifier = ""
                            }
                        }
                    }
                }
            };

            await container.CreateItemAsync<CloudUser>(selectedUser);
        }
    }
}
