namespace PublicServiceRegistry.Api.Backoffice.Infrastructure.ETag
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public class OkResultWithLastObservedPosition : OkObjectResult
    {
        private readonly string _lop;

        public OkResultWithLastObservedPosition(object value, string lop) : base(value) => _lop = lop;

        public override void ExecuteResult(ActionContext context)
        {
            base.ExecuteResult(context);

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.HttpContext.Response.Headers.Add(PublicServiceHeaderNames.LastObservedPosition, _lop);
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.HttpContext.Response.Headers.Add(PublicServiceHeaderNames.LastObservedPosition, _lop);

            return base.ExecuteResultAsync(context);
        }
    }
}
