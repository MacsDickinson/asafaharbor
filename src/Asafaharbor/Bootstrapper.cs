using ASafaWeb.Library.DomainModels;
using ASafaWeb.Library.Raven;
using Asafaharbor.Web.Responses;
using Asafaharbor.Web.Utils;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Cryptography;
using Nancy.Diagnostics;
using Raven.Client;

namespace Asafaharbor.Web
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = @"?eUYoJ9J" }; }
        }

        protected override void ConfigureApplicationContainer(Nancy.TinyIoc.TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var store = RavenSessionProvider.DocumentStore;

            container.Register<IDocumentStore>(store);
        }

        protected override void ConfigureRequestContainer(Nancy.TinyIoc.TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            container.Register<IUserMapper, UserMapper>();

            var store = container.Resolve<IDocumentStore>();
            var documentSesion = store.OpenSession();

            container.Register(documentSesion);
        }

        protected override void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container,
                                               Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            pipelines.OnError.AddItemToEndOfPipeline((ctx, ex) =>
                {
                    new LogEvent("An unhandled exception occurred", ex).Raise();
                    return ErrorResponse.FromException(ex);
                });

            base.RequestStartup(container, pipelines, context);

            var cryptographyConfiguration = new CryptographyConfiguration(
                new RijndaelEncryptionProvider(new PassphraseKeyGenerator("SuperSecretPass",
                                                                          new byte[] { 100, 111, 110, 116, 32, 109, 97, 107, 101, 32, 109, 101, 32, 108, 111, 103, 32, 105, 110, 32, 101, 97, 99, 104, 32, 116, 105, 109, 101, 32, 105, 32, 98, 117, 105, 108, 100, 33 })),
                new DefaultHmacProvider(new PassphraseKeyGenerator("UberSuperSecure",
                                                                   new byte[] { 90, 71, 57, 117, 100, 67, 66, 116, 89, 87, 116, 108, 73, 71, 49, 108, 73, 71, 120, 118, 90, 121, 66, 112, 98, 105, 66, 108, 89, 87, 78, 111, 73, 72, 82, 112, 98, 87, 85, 103, 97, 83, 66, 105, 100, 87, 108, 115, 90, 67, 69, 61 })));

            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration
                    {
                        CryptographyConfiguration = cryptographyConfiguration,
                        RedirectUrl = "~/account/log-in",
                        UserMapper = container.Resolve<IUserMapper>(),
                    };
            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);

            pipelines.AfterRequest.AddItemToEndOfPipeline(
                ctx =>
                    {
                        var documentSession = container.Resolve<IDocumentSession>();

                        if (ctx.Response.StatusCode != HttpStatusCode.InternalServerError)
                        {
                            documentSession.SaveChanges();
                        }

                        documentSession.Dispose();
                    }
                );
        }

        protected override void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.StaticContentsConventions.Add(
                Nancy.Conventions.StaticContentConventionBuilder.AddDirectory("Scripts", "Scripts"));
            nancyConventions.StaticContentsConventions.Add(
                Nancy.Conventions.StaticContentConventionBuilder.AddDirectory("Styles", "Styles"));
        }
    }
}