using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Pacco.Services.Orders.Application.DTO;
using Pactify;
using Xunit;

namespace Pacco.Services.Orders.PactConsumerTests.PACT
{
    public class ParcelsApiPactConsumerTests
    {
        private const string ParcelId = "c68a24ea-384a-4fdc-99ce-8c9a28feac64"; 
        
        [Fact]
        public async Task Given_Valid_Parcel_Id_Parcel_Should_Be_Returned()
        {
            var options = new PactDefinitionOptions
            {
                IgnoreCasing = true,
                IgnoreContractValues = true
            };

            await PactMaker
                .Create(options)
                .Between("orders", "parcels")
                .WithHttpInteraction(b => b
                    .Given("Existing parcel")
                    .UponReceiving("A GET request to retrieve parcel details")
                    .With(request => request
                        .WithMethod(HttpMethod.Get)
                        .WithPath($"/parcels/{ParcelId}"))
                    .WillRespondWith(response => response
                        .WithHeader("Content-Type", "application/json")
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithBody<ParcelDto>()))
                .PublishedAsFile("../../../../../../pacts")
                .MakeAsync();
        }
    }
}