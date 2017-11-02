using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ServiceStack.OrmLite;
using pdfResearch;

namespace PDFFileIETest
{
    public class DatabaseInit
    {
        public static void InitDatabase()
        {
            CreateTables();
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="overwrite"></param>
        public static void CreateTables(bool overwrite = false)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                db.CreateTable<BlockData>(overwrite);
            }
        }
    }
}
