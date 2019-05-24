namespace PublicServiceRegistry.Api.Backoffice.IpdcCode.Requests
{
    using System.ComponentModel.DataAnnotations;
    using PublicServiceRegistry.PublicService.Commands;
    using Swashbuckle.AspNetCore.Filters;
    using IpdcCode = PublicServiceRegistry.IpdcCode;

    public class SetIpdcCodeRequest
    {
        /// <summary>Ipdc code voor de dienstverlening.</summary>
        [Required]
        public string IpdcCode { get; set; }
    }

    public class SetIpdcCodeRequestExample : IExamplesProvider
    {
        public object GetExamples() =>
            new SetIpdcCodeRequest
            {
                IpdcCode = "1928"
            };
    }

    public static class SetIpdcCodeRequestMapping
    {
        public static SetIpdcCode Map(
            string publicServiceId,
            SetIpdcCodeRequest message) =>
            new SetIpdcCode(
                new PublicServiceId(publicServiceId),
                new IpdcCode(message.IpdcCode));
    }
}
