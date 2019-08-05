using EVF.K2.Bll.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVF.K2.Bll.Components
{
    /// <summary>
    /// Config Setting Class get value of web config.
    /// </summary>
    public static class ConfigSetting
    {
        /// <summary>
        /// Get appSettings parameter in webconfig.
        /// </summary>
        /// <param name="name">The name of paramter.</param>
        /// <returns></returns>
        private static string GetAppSetting(string name) => System.Configuration.ConfigurationManager.AppSettings[name];

        /// <summary>
        /// Get appSettings parameter in webconfig.
        /// </summary>
        /// <param name="name">The name of paramter.</param>
        /// <returns></returns>
        private static string GetConnectionString(string name) => System.Configuration.ConfigurationManager.ConnectionStrings[name].ToString();

        /// <summary>
        /// Get k2 url.
        /// </summary>
        public static string K2Url => GetAppSetting(ConstantValueService.K2_URL);

        /// <summary>
        /// Get k2 workflow port.
        /// </summary>
        public static string K2WorkflowPort => GetAppSetting(ConstantValueService.K2_PORT);

        /// <summary>
        /// Get k2 management port.
        /// </summary>
        public static string K2ManagementPort => GetAppSetting(ConstantValueService.K2_MANAGEMENT_PORT);

        /// <summary>
        /// Get k2 security label authen.
        /// </summary>
        public static string K2SecurityLabel => GetAppSetting(ConstantValueService.K2_SECURITYLABEL);

        /// <summary>
        /// Get admin k2 username authen.
        /// </summary>
        public static string K2AdminUserName => GetAppSetting(ConstantValueService.K2_ADMINUSERNAME);

        /// <summary>
        /// Get admin k2 password authen.
        /// </summary>
        public static string K2AdminPassword => GetAppSetting(ConstantValueService.K2_ADMINPASSWORD);

        /// <summary>
        /// Get k2 connection string.
        /// </summary>
        public static string K2ConnectionString => GetConnectionString(ConstantValueService.K2_WORKFLOWEMPLOYEE);

        /// <summary>
        /// Get encrypt key value.
        /// </summary>
        public static string EncryptionKey => GetAppSetting(ConstantValueService.ENCRYPTION_KEY);

    }
}
