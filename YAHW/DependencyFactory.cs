using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Configuration;

namespace YAHW
{
    /// <summary>
    /// <para>
    /// A simple wrapper for the DI-Container
    /// </para>
    /// 
    /// <para>
    /// Class history:
    /// <list type="bullet">
    ///     <item>
    ///         <description>1.0: First release, working (Steffen Steinbrecher).</description>
    ///     </item>
    /// </list>
    /// </para>
    /// 
    /// <para>Author: Steffen Steinbrecher</para>
    /// <para>Date: 12.07.2015</para>
    /// </summary>
    public class DependencyFactory
    {
        private static IUnityContainer _container;

        /// <summary>
        /// Public reference to the unity container which will 
        /// allow the ability to register instrances or take 
        /// other actions on the container.
        /// </summary>
        public static IUnityContainer Container
        {
            get
            {
                return _container;
            }
            private set
            {
                _container = value;
            }
        }

        /// <summary>
        /// Static constructor for DependencyFactory which will 
        /// initialize the unity container.
        /// </summary>
        static DependencyFactory()
        {
            var container = new UnityContainer();

            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");

            if (section != null)
            {
                section.Configure(container);
            }

            _container = container;
        }

        /// <summary>
        /// Register an instance of type TInterface with a given name
        /// </summary>
        /// <typeparam name="TInterface">Type of object</typeparam>
        /// <param name="name">The name</param>
        /// <param name="instance">The instance</param>
        public static void RegisterInstance<TInterface>(string name, TInterface instance)
        {
            if (!String.IsNullOrEmpty(name) && instance != null)
            {
                Container.RegisterInstance<TInterface>(name, instance, new ContainerControlledLifetimeManager());
            }
        }

        /// <summary>
        /// Resolves the type parameter T to an instance of the appropriate type.
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        public static T Resolve<T>()
        {
            T ret = default(T);

            if (Container.IsRegistered(typeof(T)))
            {
                ret = Container.Resolve<T>();
            }

            return ret;
        }

        /// <summary>
        /// Resolves the type parameter T to an instance of the appropriate type and name.
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        public static T Resolve<T>(string name)
        {
            return Container.Resolve<T>(name);
        }
    }
}