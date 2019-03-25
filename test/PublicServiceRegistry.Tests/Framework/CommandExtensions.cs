namespace PublicServiceRegistry.Tests.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using Be.Vlaanderen.Basisregisters.CommandHandling;

    public static class CommandExtensions
    {
        public static CommandMessage<T> PerformedByAdmin<T>(this T source) =>
            new CommandMessage<T>(
                Guid.NewGuid(),
                source,
                new Dictionary<string, object>
                {
                    {
                        CommandMetaData.Keys.UserClaims,
                        new Dictionary<string, string> {{ ClaimTypes.Role, PublicServiceRegistryClaims.AdminRole }}
                    }
                });

        public static CommandMessage<T> PerformedByBeheerder<T>(this T source) =>
            new CommandMessage<T>(
                Guid.NewGuid(),
                source,
                new Dictionary<string, object>
                {
                    {
                        CommandMetaData.Keys.UserClaims,
                        new Dictionary<string, string> {{ ClaimTypes.Role, PublicServiceRegistryClaims.BeheerderRole }}
                    }
                });
    }
}
