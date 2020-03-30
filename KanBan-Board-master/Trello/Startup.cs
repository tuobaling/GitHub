﻿using Microsoft.Owin;
using Owin;
using Trello.BLL.Repositories;
[assembly: OwinStartupAttribute(typeof(Trello.Startup))]
namespace Trello
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
