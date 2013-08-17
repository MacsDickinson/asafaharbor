using Asafaharbor.Web.Renderers;
using Nancy;
using Nancy.ErrorHandling;
using Nancy.ViewEngines;

namespace Asafaharbor.Web.Handlers
{
    public class ErrorHandler :  IStatusCodeHandler
    {
        private readonly ErrorViewRenderer _renderer;

        public ErrorHandler(IViewFactory factory)
        {
            _renderer = new ErrorViewRenderer(factory);
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.NotFound
                   || statusCode == HttpStatusCode.InternalServerError
                   || statusCode == HttpStatusCode.BadRequest;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            context.Response = _renderer.RenderResponse(statusCode, context);
        }
    }
}