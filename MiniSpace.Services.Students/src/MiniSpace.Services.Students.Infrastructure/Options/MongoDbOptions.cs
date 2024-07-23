namespace MiniSpace.Services.Students.Infrastructure.Options
{
    public class MongoDbOptions
    {
        public string ConnectionString { get; set; }
        public string WriteDatabase { get; set; }
        public string ReadDatabase { get; set; }
        public bool Seed { get; set; }
    }
}
