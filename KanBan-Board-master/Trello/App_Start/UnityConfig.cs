using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Web.Mvc;
using Trello.BLL.Services;
using Trello.Controllers;
using Trello.DAL.Repositories;
using Trello.DAL.UnitOfWorks;
using Trello.Models;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace Trello
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            #region Service
            //container.RegisterType<IAccountService, AccountService>();
            //container.RegisterType<IActivitiesService, ActivitiesService>();
            //container.RegisterType<IAspNetUsersService, AspNetUsersService>();
            //container.RegisterType<IAttachmentsService, AttachmentsService>();
            //container.RegisterType<IBoardsService, BoardsService>();
            //container.RegisterType<ICardsService, CardsService>();
            //container.RegisterType<ICommentRepliesService, CommentRepliesService>();
            //container.RegisterType<ICommentsService, CommentsService>();
            //container.RegisterType<IFilesService, FilesService>();
            //container.RegisterType<ILabelsService, LabelsService>();
            //container.RegisterType<IManageService, ManageService>();
            //container.RegisterType<ITaskItemsService, TaskItemsService>();
            container.RegisterType<ITicketsService, TicketsService>();
            container.RegisterType<IUserBoardsService, UserBoardsService>();
            #endregion
            #region UnitOfWork
            container.RegisterType<IUnitOfWorks, UnitOfWorks>();
            #endregion
            #region Repository
            container.RegisterType(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            #endregion
            #region Identity
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
            container.RegisterType<UserManager<ApplicationUser>>();
            container.RegisterType<DbContext, ApplicationDbContext>();
            container.RegisterType<ApplicationUserManager>();
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());
            #endregion

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}