namespace MiniSpace.APIGateway.Ocelot.Infrastructure
{
    internal sealed class AsyncRoutesOptions
    {
        public bool? Authenticate { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public Dictionary<string, RouteOptions> Routes { get; set; } 
    }

    public class RouteOptions
    {
        public string RoutingKey { get; set; }
        public string Exchange { get; set; }
        public bool? Authenticate { get; set; }
    }
}
