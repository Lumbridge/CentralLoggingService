﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CLS.Web.Startup))]

namespace CLS.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
