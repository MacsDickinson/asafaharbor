namespace Asafaharbor.Web.Modules
{
    public class HomeModule : BaseModule
    {
        public HomeModule()
        {
            Get["/"] = parameters =>
            {
                Page.Title = "Home";

                return View["Index", Model];
            };

            Get["/about"] = parameters =>
            {

                Page.Title = "About";

                return View["About", Model];
            };
        }
    }
}