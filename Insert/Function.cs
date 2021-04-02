using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Insert
{
    public class Function
    {
        class Item
        {
            public string itemId;
            public string description;
            public int rating;
            public string type;
            public string company;

        }

        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private string tableName = "Items";

        public async Task<string> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            Table items = Table.LoadTable(client, tableName);

            Item myItem = JsonConvert.DeserializeObject<Item>(input.Body);

            PutItemOperationConfig config = new PutItemOperationConfig();

            config.ReturnValues = ReturnValues.AllOldAttributes;

            Document result = await items.PutItemAsync(Document.FromJson(JsonConvert.SerializeObject(myItem)), config);

            return input.Body;
            
        }
    }
}



/*
 string value = "";
Dictionary<string, AttributeValue> myDictionary = new Dictionary<string, AttributeValue>();

myDictionary.Add("itemId", new AttributeValue() { S = input.ToUpper() });
myDictionary.Add("description", new AttributeValue() { S = input.ToUpper() });
myDictionary.Add("rating", new AttributeValue() { S = input.ToUpper() });
myDictionary.Add("type", new AttributeValue() { S = input.ToUpper() });
myDictionary.Add("company", new AttributeValue() { S = input.ToUpper() });

PutItemRequest myRequest = new PutItemRequest(tableName, myDictionary);

PutItemResponse res = await client.PutItemAsync(myRequest);

return input.ToUpper();
*/