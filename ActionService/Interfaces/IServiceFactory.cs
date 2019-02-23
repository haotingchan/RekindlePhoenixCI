using ActionService.Interfaces.Prefix5;
using ActionService.Interfaces.Prefix6;
using ActionService.Interfaces.PrefixC;
using ActionService.Interfaces.PrefixZ;

namespace ActionService.Interfaces
{
    public interface IServiceFactory
    {
        IServiceCommon ServiceCommon { get; }

        #region Prefix1

        IServicePrefix1 ServicePrefix1 { get; }

        #endregion

        #region Prefix5
        IService50072 Service50072 { get; }
        IService50073 Service50073 { get; }
        #endregion

        #region Prefix6

        IService60110 Service60110 { get; }

        IService60210 Service60210 { get; }

        IService60310 Service60310 { get; }

        IService60320 Service60320 { get; }

        IService60330 Service60330 { get; }

        IService60410 Service60410 { get; }

        #endregion Prefix6

        #region PrefixZ

        IServiceZ0010 ServiceZ0010 { get; }

        IServiceZ0011 ServiceZ0011 { get; }

        IServiceZ0019 ServiceZ0019 { get; }

        IServiceZ0020 ServiceZ0020 { get; }

        IServiceZ0110 ServiceZ0110 { get; }

        IServiceZ0111 ServiceZ0111 { get; }

        IServiceZ0112 ServiceZ0112 { get; }

        IServiceZ0990 ServiceZ0990 { get; }

        IServiceZ9999 ServiceZ9999 { get; }

        #endregion PrefixZ
    }
}