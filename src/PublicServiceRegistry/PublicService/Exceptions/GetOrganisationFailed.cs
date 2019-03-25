namespace PublicServiceRegistry.PublicService.Exceptions
{
    using System.Collections.Generic;

    public class GetOrganisationFailed : PublicServiceRegistryException
    {
        private const string ExceptionMessage = "Kan de gevraagde organisatie niet ophalen.";

        public IEnumerable<string> AttemptResults { get; }

        public GetOrganisationFailed()
            : this(new string[] { }) { }

        public GetOrganisationFailed(IEnumerable<string> attempts)
            : base(ExceptionMessage) =>
            AttemptResults = attempts ?? new string[] { };
    }
}
