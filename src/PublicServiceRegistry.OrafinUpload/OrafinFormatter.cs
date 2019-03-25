namespace PublicServiceRegistry.OrafinUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Projections.Backoffice.PublicServiceList;

    public interface IOrafinFormatter
    {
        string FormatList(IEnumerable<PublicServiceListItem> publicServices);
    }

    public class OrafinFormatter : IOrafinFormatter
    {
        // TODO: Moet die laatste pipe?
        // TODO: een reden waarom PubliekeOrganisatie pascal cased is en de rest lower?
        private const string Header = "identificator|naam|PubliekeOrganisatie|startdatum|einddatum|";

        public string FormatList(IEnumerable<PublicServiceListItem> publicServices)
        {
            return (publicServices ?? new List<PublicServiceListItem>())
                .Where(item => item != null)
                .Select(FormatLine)
                .Prepend(Header)
                .Aggregate((result, line) => $"{result}{Environment.NewLine}{line}");
        }

        private static string FormatLine(PublicServiceListItem item)
            => $"{item.PublicServiceId}|{item.Name}|{item.CompetentAuthorityCode}|||";
    }
}
