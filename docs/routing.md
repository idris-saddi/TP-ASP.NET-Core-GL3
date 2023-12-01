# Routing

1.  
2.  
    La route n'etait pas reconnu
    alors on doit utiliser le routage d'attribut
    on ajoute `[Route("Movie/released/{year:int}/{month:int}")]` avant la methode ByRelease du controlleur MovieController
3.  
    - Conventional Routing

        In the conventional routing style, during application startup, you define route templates that will be queried each time an incoming request is received in order to make a URL matching. This process will eventually map to a controller and a method inside it.

        ```cs
        // Program.cs
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        );
        ```

    - Attribute Routing

        In this routing, attributes are used to define routes. Attribute routing provides you more control over the URIs by defining routes directly on actions and controllers in your ASP.NET MVC application and WEB API.

        ```cs
        // MovieController.cs
        [Route("Movie/released/{year:int}/{month:int}")]
        public IActionResult ByRelease(int month, int year)
        {
            ViewBag.month = month;
            ViewBag.year = year;

            return View();
        }
        ```
