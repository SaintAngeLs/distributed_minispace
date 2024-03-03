using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Pacco.Services.Parcels.Api;
using Pacco.Services.Parcels.Core.Entities;
using Pacco.Services.Parcels.Infrastructure.Mongo.Documents;
using Pacco.Services.Parcels.PactProviderTests.Fixtures;
using Pactify;
using Xunit;

namespace Pacco.Services.Parcels.PactProviderTests.PACT
{
    public class ParcelsApiPactProviderTests : IDisposable
    {
        [Fact]
        public async Task Pact_Should_Be_Verified()
        {
            await _mongoDbFixture.InsertAsync(Parcel);
            
            await PactVerifier
                .Create(_httpClient)
                .Between("orders", "parcels")
                .RetrievedFromFile(@"../../../../../../pacts")
                .VerifyAsync();
        }

        #region ARRANGE

        private readonly ParcelDocument Parcel = new ParcelDocument
        {
            Id =  new Guid("c68a24ea-384a-4fdc-99ce-8c9a28feac64"),
            Name = "Product",
            Size = Size.Huge,
            Variant = Variant.Weapon,
            CreatedAt = DateTime.Now
        };
        
        private readonly MongoDbFixture<ParcelDocument, Guid> _mongoDbFixture;
        private readonly HttpClient _httpClient;
        private bool _disposed = false;

        public ParcelsApiPactProviderTests()
        {
            _mongoDbFixture = new MongoDbFixture<ParcelDocument, Guid>("test-parcels-service", "parcels");
            var testServer = new TestServer(Program.GetWebHostBuilder(new string[0]));
            testServer.AllowSynchronousIO = true;
            _httpClient = testServer.CreateClient();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _mongoDbFixture.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
