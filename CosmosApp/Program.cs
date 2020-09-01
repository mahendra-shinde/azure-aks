﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Azure.Cosmos;
using Microsoft.Azure.Cosmos;

namespace MongoApp
{
    class Program
    {
        private const string EndpointUrl = "https://maxlimited.documents.azure.com:443/";
        private const string AuthorizationKey = "";
        private const string DatabaseId = "FamilyDatabase";
        private const string ContainerId = "FamilyContainer";

        static void Main(string[] args)
        {
            CosmosClient cosmosClient = new CosmosClient(EndpointUrl, AuthorizationKey);
            Program.CreateDatabaseAsync(cosmosClient).Wait();
             Program.CreateContainerAsync(cosmosClient).Wait();
             Program.AddItemsToContainerAsync(cosmosClient).Wait();
             Program.QueryItemsAsync(cosmosClient).Wait();
             Program.ReplaceFamilyItemAsync(cosmosClient).Wait();
             Program.DeleteFamilyItemAsync(cosmosClient).Wait();
             Program.DeleteDatabaseAndCleanupAsync(cosmosClient).Wait();
        }
        /// <summary>
        /// Create the database if it does not exist
        /// </summary>
        private static async Task CreateDatabaseAsync(CosmosClient cosmosClient)
        {
            // Create a new database
            DatabaseResponse database = await cosmosClient.CreateDatabaseIfNotExistsAsync(Program.DatabaseId);
            Console.WriteLine("Created Database: {0}\n", database.Database.Id);
        }

        /// <summary>
        /// Create the container if it does not exist. 
        /// Specify "/LastName" as the partition key since we're storing family information, to ensure good distribution of requests and storage.
        /// </summary>
        /// <returns></returns>
        private static async Task CreateContainerAsync(CosmosClient cosmosClient)
        {
            // Create a new container
            ContainerResponse container = await cosmosClient.GetDatabase(Program.DatabaseId).CreateContainerIfNotExistsAsync(Program.ContainerId, "/LastName");
            Console.WriteLine("Created Container: {0}\n", container.Container.Id);
        }
        /// <summary>
        /// Add Family items to the container
        /// </summary>
        private static async Task AddItemsToContainerAsync(CosmosClient cosmosClient)
        {
            // Create a family object for the Andersen family
            Family andersenFamily = new Family
            {
                Id = "Andersen.1",
                LastName = "Andersen",
                Parents = new Parent[]
                {
            new Parent { FirstName = "Thomas" },
            new Parent { FirstName = "Mary Kay" }
                },
                Children = new Child[]
                {
            new Child
            {
                FirstName = "Henriette Thaulow",
                Gender = "female",
                Grade = 5,
                Pets = new Pet[]
                {
                    new Pet { GivenName = "Fluffy" }
                }
            }
                },
                Address = new Address { State = "WA", County = "King", City = "Seattle" },
                IsRegistered = false
            };

            Container container = cosmosClient.GetContainer(Program.DatabaseId, Program.ContainerId);
            try
            {
                // Read the item to see if it exists.  
                ItemResponse<Family> andersenFamilyResponse = await container.ReadItemAsync<Family>(andersenFamily.Id, new PartitionKey(andersenFamily.LastName));
                Console.WriteLine("Item in database with id: {0} already exists\n", andersenFamilyResponse.Value.Id);
            }
            catch (CosmosException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
                ItemResponse<Family> andersenFamilyResponse = await container.CreateItemAsync<Family>(andersenFamily, new PartitionKey(andersenFamily.LastName));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse.
                Console.WriteLine("Created item in database with id: {0}\n", andersenFamilyResponse.Value.Id);
            }

            // Create a family object for the Wakefield family
            Family wakefieldFamily = new Family
            {
                Id = "Wakefield.7",
                LastName = "Wakefield",
                Parents = new Parent[]
                {
            new Parent { FamilyName = "Wakefield", FirstName = "Robin" },
            new Parent { FamilyName = "Miller", FirstName = "Ben" }
                },
                Children = new Child[]
                {
            new Child
            {
                FamilyName = "Merriam",
                FirstName = "Jesse",
                Gender = "female",
                Grade = 8,
                Pets = new Pet[]
                {
                    new Pet { GivenName = "Goofy" },
                    new Pet { GivenName = "Shadow" }
                }
            },
            new Child
            {
                FamilyName = "Miller",
                FirstName = "Lisa",
                Gender = "female",
                Grade = 1
            }
                },
                Address = new Address { State = "NY", County = "Manhattan", City = "NY" },
                IsRegistered = true
            };

            // Create an item in the container representing the Wakefield family. Note we provide the value of the partition key for this item, which is "Wakefield"
            ItemResponse<Family> wakefieldFamilyResponse = await container.UpsertItemAsync<Family>(wakefieldFamily, new PartitionKey(wakefieldFamily.LastName));

            // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
            Console.WriteLine("Created item in database with id: {0}\n", wakefieldFamilyResponse.Value.Id);
        }
        /// <summary>
        /// Run a query (using Azure Cosmos DB SQL syntax) against the container
        /// </summary>
        private static async Task QueryItemsAsync(CosmosClient cosmosClient)
        {
            var sqlQueryText = "SELECT * FROM c WHERE c.LastName = 'Andersen'";

            Console.WriteLine("Running query: {0}\n", sqlQueryText);

            Container container = cosmosClient.GetContainer(Program.DatabaseId, Program.ContainerId);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);

            List<Family> families = new List<Family>();

            await foreach (Family family in container.GetItemQueryIterator<Family>(queryDefinition))
            {
                families.Add(family);
                Console.WriteLine("\tRead {0}\n", family);
            }
        }
        /// <summary>
        /// Replace an item in the container
        /// </summary>
        private static async Task ReplaceFamilyItemAsync(CosmosClient cosmosClient)
        {
            Container container = cosmosClient.GetContainer(Program.DatabaseId, Program.ContainerId);

            ItemResponse<Family> wakefieldFamilyResponse = await container.ReadItemAsync<Family>("Wakefield.7", new PartitionKey("Wakefield"));
            Family itemBody = wakefieldFamilyResponse;

            // update registration status from false to true
            itemBody.IsRegistered = true;
            // update grade of child
            itemBody.Children[0].Grade = 6;

            // replace the item with the updated content
            wakefieldFamilyResponse = await container.ReplaceItemAsync<Family>(itemBody, itemBody.Id, new PartitionKey(itemBody.LastName));
            Console.WriteLine("Updated Family [{0},{1}].\n \tBody is now: {2}\n", itemBody.LastName, itemBody.Id, wakefieldFamilyResponse.Value);
        }

        /// <summary>
        /// Delete an item in the container
        /// </summary>
        private static async Task DeleteFamilyItemAsync(CosmosClient cosmosClient)
        {
            Container container = cosmosClient.GetContainer(Program.DatabaseId, Program.ContainerId);

            string partitionKeyValue = "Wakefield";
            string familyId = "Wakefield.7";

            // Delete an item. Note we must provide the partition key value and id of the item to delete
            ItemResponse<Family> wakefieldFamilyResponse = await container.DeleteItemAsync<Family>(familyId, new PartitionKey(partitionKeyValue));
            Console.WriteLine("Deleted Family [{0},{1}]\n", partitionKeyValue, familyId);
        }

        /// <summary>
        /// Delete the database and dispose of the Cosmos Client instance
        /// </summary>
        private static async Task DeleteDatabaseAndCleanupAsync(CosmosClient cosmosClient)
        {
            Database database = cosmosClient.GetDatabase(Program.DatabaseId);
            DatabaseResponse databaseResourceResponse = await database.DeleteAsync();

            Console.WriteLine("Deleted Database: {0}\n", Program.DatabaseId);
        }
    }
}
