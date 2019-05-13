using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace Common.Helper {
   public class ConnectSharedFolder : IDisposable {
      // Fields
      private readonly string _networkName;

      // Methods
      public ConnectSharedFolder(string networkName , NetworkCredential credentials) {
         this._networkName = networkName;
         NetResource resource1 = new NetResource();
         resource1.Scope = ResourceScope.GlobalNetwork;
         resource1.ResourceType = ResourceType.Disk;
         resource1.DisplayType = ResourceDisplaytype.Share;
         resource1.RemoteName = networkName;
         NetResource netResource = resource1;
         string username = string.IsNullOrEmpty(credentials.Domain) ? credentials.UserName : $"{credentials.Domain}\\{credentials.UserName}";
         if (!Directory.Exists(networkName)) {
            int error = WNetAddConnection2(netResource , credentials.Password , username , 0);
            if (error != 0) {
               throw new Win32Exception(error , "Error connecting to remote share, error code is " + error);
            }
         }
      }

      public void Dispose() {
         this.Dispose(true);
         GC.SuppressFinalize(this);
      }

      protected virtual void Dispose(bool disposing) {
      }

      ~ConnectSharedFolder() {
         this.Dispose(false);
      }

      [DllImport("mpr.dll")]
      private static extern int WNetAddConnection2(NetResource netResource , string password , string username , int flags);
      [DllImport("mpr.dll")]
      private static extern int WNetCancelConnection2(string name , int flags , bool force);

      // Nested Types
      [StructLayout(LayoutKind.Sequential)]
      public class NetResource {
         public ConnectSharedFolder.ResourceScope Scope;
         public ConnectSharedFolder.ResourceType ResourceType;
         public ConnectSharedFolder.ResourceDisplaytype DisplayType;
         public int Usage;
         public string LocalName;
         public string RemoteName;
         public string Comment;
         public string Provider;
      }

      public enum ResourceDisplaytype {
         Generic,
         Domain,
         Server,
         Share,
         File,
         Group,
         Network,
         Root,
         Shareadmin,
         Directory,
         Tree,
         Ndscontainer
      }

      public enum ResourceScope {
         Connected = 1,
         GlobalNetwork = 2,
         Remembered = 3,
         Recent = 4,
         Context = 5
      }

      public enum ResourceType {
         Any = 0,
         Disk = 1,
         Print = 2,
         Reserved = 8
      }
   }
}
