namespace Astravent.Web.Wasm.Areas.Identity.CommandsDto
{
    public class UpdateUserStatus
    {
        public Guid UserId { get; set; }
        public bool IsOnline { get; set; }
        public string DeviceType { get; set; }
    }
}