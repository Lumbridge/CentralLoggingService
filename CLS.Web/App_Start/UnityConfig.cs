using CLS.Infrastructure.Classes;
using CLS.Infrastructure.Data;
using CLS.Infrastructure.Interfaces;
using System;
using Unity;

namespace CLS.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        private static Lazy<IUnityContainer> container =
            new Lazy<IUnityContainer>(() =>
            {
                var container = new UnityContainer();
                RegisterTypes(container);
                return container;
            });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterInstance(new DBEntities());

            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IEntities, DBEntities>();
            container.Resolve<IUnitOfWork>();
        }
    }
}