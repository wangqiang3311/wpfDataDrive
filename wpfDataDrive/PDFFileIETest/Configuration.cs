using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using ServiceStack.Redis;

namespace PDFFileIETest
{
    public class Configuration
    {
        private static OrmLiteConnectionFactory dbFactory = null;

        /// <summary>
        /// ORM工厂
        /// </summary>
        /// <exception cref="Exception">未找到数据库配置信息</exception>
        public static OrmLiteConnectionFactory DbFactory
        {
            get
            {
                if (dbFactory == null)
                {
                    string connectionString = string.Empty;
                    if (ConfigurationManager.ConnectionStrings["DbHelperConnectionString"] != null)
                    {
                        connectionString = ConfigurationManager.ConnectionStrings["DbHelperConnectionString"].ConnectionString;
                    }
                    else
                    {
                        throw new Exception("未找到数据库配置信息");
                    }
                    dbFactory = new OrmLiteConnectionFactory(connectionString, SqlServerDialect.Provider);
                    dbFactory.RegisterConnection("Raw", connectionString, SqlServerDialect.Provider);
                    dbFactory.DialectProvider.GetStringConverter().UseUnicode = true;
                }
                return dbFactory;
            }
        }
    }
}
