namespace PublicServiceRegistry
{
    using System;
    using System.Text.RegularExpressions;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Newtonsoft.Json;

    public class IpdcCode: StringValueObject<IpdcCode>
    {
        public IpdcCode([JsonProperty("value")] string ipdcCode) : base(ipdcCode)
        {
            if(!Regex.IsMatch(ipdcCode, @"^(?!(?:0000)$)[0-9]{4}$"))
                throw new ArgumentException(nameof(ipdcCode));
        }
    }
}
