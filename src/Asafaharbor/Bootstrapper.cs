﻿using Asafaharbor.Web.Raven;
using Asafaharbor.Web.Utils;
using Nancy;
using Nancy.Authentication.Forms;
using Raven.Client;

namespace Asafaharbor.Web
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
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

        protected override void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration
                    {
                    RedirectUrl = "~/account/log-on",
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
            nancyConventions.StaticContentsConventions.Add(Nancy.Conventions.StaticContentConventionBuilder.AddDirectory("Scripts", "Scripts"));
            nancyConventions.StaticContentsConventions.Add(Nancy.Conventions.StaticContentConventionBuilder.AddDirectory("Styles", "Styles"));
        }
    }
}