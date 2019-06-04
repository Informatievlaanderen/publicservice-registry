namespace PublicServiceRegistry.Api.Backoffice.Tests
{
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using FluentAssertions;
    using Infrastructure.ExceptionHandlers;
    using Microsoft.AspNetCore.Http;
    using PublicServiceRegistry.PublicService.Exceptions;
    using Xunit;
    using ProblemDetails = Be.Vlaanderen.Basisregisters.BasicApiProblem.ProblemDetails;

    public class GetOrganisationFailedHandlerTests
    {
        public readonly GetOrganisationFailedHandler _sut;

        public GetOrganisationFailedHandlerTests()
        {
            _sut = new GetOrganisationFailedHandler();
        }

        [Fact]
        public void HandlesGetOrganistionFailedExceptions()
        {
            _sut.Handles(new GetOrganisationFailed()).Should().BeTrue();
        }

        [Fact]
        public void HandledTypeIsGetOrginastionFailedException()
        {
            _sut.HandledExceptionType.Should().Be<GetOrganisationFailed>();
        }
    }

    public class WhenHandlingAGetOganisationFailedException
    {
        private readonly OrganisationNotFoundApiProblem _handledProblem;
        private readonly GetOrganisationFailed _exception;

        public WhenHandlingAGetOganisationFailedException()
        {
            var exceptionContentList = new[]
            {
                "fist thing failed",
                "second this too",
                "third thing was out of office"
            };
            _exception = new GetOrganisationFailed(exceptionContentList);

            var handler = new GetOrganisationFailedHandler();
            _handledProblem = handler.GetApiProblemFor(_exception);
        }

        [Fact]
        public void ThenTheProblemInstanceUriShouldNotBeEmtpy()
        {
            _handledProblem.ProblemInstanceUri.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void ThenThenHttpStatusShouldBeBadRequest()
        {
            _handledProblem.HttpStatus.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public void ThenTheProblemTitleShouldBeTheDefaultApiProblemTitle()
        {
            _handledProblem.Title.Should().Be(ProblemDetails.DefaultTitle);
        }

        [Fact]
        public void ThenTheProblemDetailShouldBeTheErrorMessage()
        {
            _handledProblem.Detail.Should().Be("Kan de gevraagde organisatie niet ophalen.");
        }

        [Fact]
        public void ThenTheProblemAttemptsContainstTheAttemptResults()
        {
            _handledProblem.Attempts.Should().BeEquivalentTo(_exception.AttemptResults);
        }

        [Fact]
        public void ThenTheProblemContainstTheGetOrganisationFailedType()
        {
            _handledProblem.ProblemTypeUri.Should().Be("urn:publicserviceregistry.api.backoffice:getorganisationfailed");
        }
    }
}
