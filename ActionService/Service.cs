using ActionService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionService
{
    public class Service
    {
        static IServiceFactory instance;

        // Lock synchronization object
        private static object locker = new object();

        //static string provider = "WebAPI";
        static string provider = "DbDirect";

        //private static string WebApiHost = "http://localhost:57382";

        protected Service()
        {

        }

        public static IServiceFactory Do()
        {
            // Support multithreaded applications through
            // 'Double checked locking' pattern which (once
            // the instance exists) avoids locking each
            // time the method is invoked
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = GetFactory(provider);
                    }
                }
            }

            return instance;
        }

        private static IServiceFactory GetFactory(string serviceProvider)
        {
            switch (serviceProvider)
            {
                case "DbDirect": return new DbDirect.ServiceFactory();
                //case "WebAPI": return new WebAPI.ServiceFactory();

                default: return new DbDirect.ServiceFactory();
            }
        }
    }
}
