namespace PublicServiceRegistry.Api.Backoffice.Ipdc
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Be.Vlaanderen.Basisregisters.Api;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Security;

    [ApiVersion("1.0")]
    [AdvertiseApiVersions("1.0")]
    [ApiRoute("ipdc")]
    [ApiExplorerSettings(GroupName = "Ipdc")]
    [PublicServiceRegistryAuthorize]
    public class IpdcChangesController : ApiBusController
    {
        public IpdcChangesController(ICommandHandlerResolver bus) : base(bus) { }

        /// <summary>
        /// Vraag een dienstverlening op.
        /// </summary>
        /// <param name="changedSince"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Als de dienstverlening gevonden is.</response>
        /// <response code="404">Als de dienstverlening niet gevonden kan worden.</response>
        /// <response code="412">Als de gevraagde minimum positie van de event store nog niet bereikt is.</response>
        /// <response code="500">Als er een interne fout is opgetreden.</response>
        [HttpGet("{changedSince}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(
            [FromRoute] string changedSince,
            CancellationToken cancellationToken = default)
        {
            var changedSinceDate = DateTime.ParseExact(
                changedSince,
                new[] {"dd.MM.yyyy"},
                CultureInfo.InvariantCulture,
                DateTimeStyles.None);

            // TODO: Use HttpFactory and trace the call
            var client = new HttpClient();
            var result =
                await client.GetAsync(
                    $"https://productencatalogus.vlaanderen.be/ZoekProducten/1babd0b7-3de6-40b4-3584-93dec1aeed6f?_format=xml_extended&LastModified={changedSinceDate:yyyy-MM-dd}T00:00:00+01:00",
                    cancellationToken);

            var element = XElement.Parse(await result.Content.ReadAsStringAsync());

            var changedProducts =
                element
                    .Descendants("product")
                    .Where(xElement => xElement.Attribute("action").Value == "modified")
                    .Select(xElement => xElement.Attribute("id").Value);

            return Ok(changedProducts);
        }

        [HttpGet("product/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProduct(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            // TODO: Use HttpFactory and trace the call
            var client = new HttpClient();
            var result =
                await client.GetAsync(
                    $"https://productencatalogus.vlaanderen.be/GeefProduct/{id}/1babd0b7-3de6-40b4-3584-93dec1aeed6f?_format=xml_extended",
                    cancellationToken);

            var element = XElement.Parse(await result.Content.ReadAsStringAsync());
            var productId = element.Descendants("productId").First().Value;
            var product = new
            {
                Id = productId,
                Name = element.Descendants("naam").First().Value,
                Theme = element.Descendants("defaultThema").FirstOrDefault()?.Element("waarde")?.Value,
                TargetAudiences =
                    string.Join(
                        ", ",
                        element.Descendants("doelgroep").Descendants("waarde").Select(xElement => xElement.Value)),
                IpdcDataLink = $"https://productencatalogus.vlaanderen.be/GeefProduct/{productId}/1babd0b7-3de6-40b4-3584-93dec1aeed6f?_format=xml_extended",
                IpdcLink = $"https://productencatalogus.vlaanderen.be/fiche/{productId}"
            };

            return Ok(product);
        }
    }
}
