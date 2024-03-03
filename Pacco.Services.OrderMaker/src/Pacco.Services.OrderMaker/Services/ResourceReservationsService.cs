using System;
using System.Linq;
using System.Threading.Tasks;
using Pacco.Services.OrderMaker.DTO;
using Pacco.Services.OrderMaker.Services.Clients;

namespace Pacco.Services.OrderMaker.Services
{
    public class ResourceReservationsService : IResourceReservationsService
    {
        private readonly IAvailabilityServiceClient _availabilityServiceClient;

        public ResourceReservationsService(IAvailabilityServiceClient availabilityServiceClient)
        {
            _availabilityServiceClient = availabilityServiceClient;
        }

        public async Task<ReservationDto> GetBestAsync(Guid resourceId)
        {
            var resource = await _availabilityServiceClient.GetResourceReservationsAsync(resourceId);
            if (resource is null)
            {
                throw new InvalidOperationException($"Resource with id: '{resourceId}' was not found.");
            }

            var latestReservation = resource.Reservations.Any()
                ? resource.Reservations.OrderBy(r => r.DateTime).Last()
                : null;

            if (latestReservation is null)
            {
                return new ReservationDto
                {
                    DateTime = DateTime.UtcNow.AddDays(1),
                    Priority = 0
                };
            }

            return new ReservationDto
            {
                DateTime = latestReservation.DateTime.AddDays(1),
                Priority = latestReservation.Priority + 1
            };
        }
    }
}