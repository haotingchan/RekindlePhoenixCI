using ActionService.DbDirect.Prefix5;
using ActionService.Interfaces;
using ActionService.Interfaces.Prefix5;
using ActionService.Interfaces.Prefix6;
using ActionService.Interfaces.PrefixC;
using ActionService.Interfaces.PrefixZ;
using ActionServiceW.DbDirect.Prefix1;
using ActionServiceW.DbDirect.Prefix6;
using ActionServiceW.DbDirect.PrefixZ;

namespace ActionService.DbDirect
{
    public class ServiceFactory : IServiceFactory
    {
        #region 變數區

        private static IServiceCommon   _ServiceCommon = null;
        private static IServicePrefix1  _ServicePrefix1 = null;
        private static IService50072    _Service50072 = null;
        private static IService50073    _Service50073 = null;
        private static IService60110    _Service60110 = null;
        private static IService60210    _Service60210 = null;
        private static IService60310    _Service60310 = null;
        private static IService60320    _Service60320 = null;
        private static IService60330    _Service60330 = null;
        private static IService60410    _Service60410 = null;
        private static IServiceZ0010    _ServiceZ0010 = null;
        private static IServiceZ0011    _ServiceZ0011 = null;
        private static IServiceZ0019    _ServiceZ0019 = null;
        private static IServiceZ0020    _ServiceZ0020 = null;
        private static IServiceZ0110    _ServiceZ0110 = null;
        private static IServiceZ0111    _ServiceZ0111 = null;
        private static IServiceZ0112    _ServiceZ0112 = null;
        private static IServiceZ0990    _ServiceZ0990 = null;
        private static IServiceZ9999    _ServiceZ9999 = null;

        #endregion 變數區

        private static object locker = new object();

        private T CreateSingletonInstance<T>(T instance, IService createdInstance)
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = (T)createdInstance;
                    }
                }
            }

            return instance;
        }

        public IServiceCommon ServiceCommon { get { return CreateSingletonInstance(_ServiceCommon, new ServiceCommon()); } }

        public IServicePrefix1 ServicePrefix1 { get { return CreateSingletonInstance(_ServicePrefix1, new ServicePrefix1()); } }

        public IService50072 Service50072 { get { return CreateSingletonInstance(_Service50072, new Service50072()); } }

        public IService50073 Service50073 { get { return CreateSingletonInstance(_Service50073, new Service50073()); } }

        public IService60110 Service60110 { get { return CreateSingletonInstance(_Service60110, new Service60110()); } }

        public IService60210 Service60210 { get { return CreateSingletonInstance(_Service60210, new Service60210()); } }

        public IService60310 Service60310 { get { return CreateSingletonInstance(_Service60310, new Service60310()); } }

        public IService60320 Service60320 { get { return CreateSingletonInstance(_Service60320, new Service60320()); } }

        public IService60330 Service60330 { get { return CreateSingletonInstance(_Service60330, new Service60330()); } }

        public IService60410 Service60410 { get { return CreateSingletonInstance(_Service60410, new Service60410()); } }

        public IServiceZ0010 ServiceZ0010 { get { return CreateSingletonInstance(_ServiceZ0010, new ServiceZ0010()); } }

        public IServiceZ0011 ServiceZ0011 { get { return CreateSingletonInstance(_ServiceZ0011, new ServiceZ0011()); } }

        public IServiceZ0019 ServiceZ0019 { get { return CreateSingletonInstance(_ServiceZ0019, new ServiceZ0019()); } }

        public IServiceZ0020 ServiceZ0020 { get { return CreateSingletonInstance(_ServiceZ0020, new ServiceZ0020()); } }

        public IServiceZ0110 ServiceZ0110 { get { return CreateSingletonInstance(_ServiceZ0110, new ServiceZ0110()); } }

        public IServiceZ0111 ServiceZ0111 { get { return CreateSingletonInstance(_ServiceZ0111, new ServiceZ0111()); } }

        public IServiceZ0112 ServiceZ0112 { get { return CreateSingletonInstance(_ServiceZ0112, new ServiceZ0112()); } }

        public IServiceZ0990 ServiceZ0990 { get { return CreateSingletonInstance(_ServiceZ0990, new ServiceZ0990()); } }

        public IServiceZ9999 ServiceZ9999 { get { return CreateSingletonInstance(_ServiceZ9999, new ServiceZ9999()); } }
    }
}