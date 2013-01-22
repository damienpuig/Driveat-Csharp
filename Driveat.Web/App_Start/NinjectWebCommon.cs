

// Instanciating services at boot load.
// NInject is a IoC (Inversion of Control) using dependency injection upon presentation project by constructor.
// Dependency injection can be performed in several ways, I choosed Contructor instead of Properties because 
// If the IoC embeds controllers instances, it takes less ressources at preload.
// IoC are based on singleton implementation.
[assembly: WebActivator.PreApplicationStartMethod(typeof(Driveat.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Driveat.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Driveat.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Driveat.Services.UserService;
    using Driveat.Services.EnumServices;
    using Driveat.Services.ReservationService;
    using Driveat.Services.DishService;
    using GeoCoding;
    using GeoCoding.Google;
    using Driveat.Services.SearchService;
    using Driveat.Services.ImageService;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage the application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Services used by the presentation layer.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {

            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IDishService>().To<DishService>();
            kernel.Bind<IReservationService>().To<ReservationService>();
            //kernel.Bind<IReservationStateService>().To<ReservationStateService>();
            kernel.Bind<IDishTypeService>().To<DishTypeService>();
            kernel.Bind<ISearchService>().To<SearchService>();
            kernel.Bind<IGeoCoder>().To<GoogleGeoCoder>();
            kernel.Bind<IImageService>().To<ImageService>();
        }        
    }
}
