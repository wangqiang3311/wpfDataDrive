using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using ServiceStack;
using ServiceStack.DataAnnotations;
using System.Data.SqlClient;
using System.Web;

namespace PDFFileIETest
{
    public class DBHelper
    {
        private const string TAG = "DbHelper";

        #region Write API

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Insert<T>(T model, bool selectIdentity = false)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return (int)db.Insert<T>(model, selectIdentity);
            }
        }
        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        public static void Insert<T>(IEnumerable<T> objs)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                db.InsertAll<T>(objs);
            }
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Save<T>(T model)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.Save<T>(model);
            }
        }
        /// <summary>
        /// 批量保存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static int SaveAll<T>(IEnumerable<T> objs)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.SaveAll<T>(objs);
            }
        }
        /// <summary>
        /// 根据Id删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int DeleteById<T>(int id)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.DeleteById<T>(id);
            }
        }
        /// <summary>
        /// 按表达式删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static int Delete<T>(Expression<Func<T, bool>> expression = null)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.Delete<T>(expression);
            }
        }
        /// <summary>
        /// 根据Id批量删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static int DeleteByIds<T>(IEnumerable ids)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.DeleteByIds<T>(ids);
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Update<T>(T model)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.Update<T>(model);
            }
        }
        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static int UpdateAll<T>(IEnumerable<T> objs)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.UpdateAll<T>(objs);
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="onlyFields"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static bool UpdateOnly<T>(T obj, Expression<Func<T, object>> onlyFields = null, Expression<Func<T, bool>> where = null)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.UpdateOnly(obj, onlyFields, where) > 0;
            }
        }

        #endregion


        #region Read API
        /// <summary>
        /// 取出一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T SingleById<T>(object id)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.SingleById<T>(id);
            }
        }

        /// <summary>
        /// 根据条件取出单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static T Single<T>(Expression<Func<T, bool>> expression)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.Single<T>(expression);
            }
        }

        /// <summary>
        /// 根据SQL取出单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static T Single<T>(string sql)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.Single<T>(sql);
            }
        }
        /// <summary>
        /// 根据SQL取出字典数据
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static Dictionary<K, V> Dictionary<K, V>(string sql)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.Dictionary<K, V>(sql);
            }
        }
        /// <summary>
        /// 根据SQL语句取出首行首列值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static T Scalar<T>(string sql)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.Scalar<T>(sql);
            }
        }
        /// <summary>
        /// 根据SQL语句取出一列值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T> Column<T>(string sql)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.Column<T>(sql);
            }
        }
        /// <summary>
        /// 根据SQL语句取出一列值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static HashSet<T> ColumnDistinct<T>(string sql)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.ColumnDistinct<T>(sql);
            }
        }

        /// <summary>
        /// 根据条件取出集合总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static int Count<T>(Expression<Func<T, bool>> expression)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return (int)db.Count<T>(expression);
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool Exist<T>(Expression<Func<T, bool>> expression)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.Exists<T>(expression);
            }
        }

        /// <summary>
        /// 根据条件取出集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static List<T> Select<T>(Expression<Func<T, bool>> expression = null)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.Select<T>(expression);
            }
        }

        /// <summary>
        /// 根据SQL获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T> Select<T>(string sql)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.Select<T>(sql);
            }
        }

        /// <summary>
        /// 根据Id集取出数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static List<T> SelectByIds<T>(IEnumerable ids)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.SelectByIds<T>(ids);
            }
        }

        #endregion

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteSql(string sql)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                return db.ExecuteSql(sql);
            }
        }
        /// <summary>
        /// 生成用于IN的SQL语句
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string BuildSqlText(IEnumerable ids)
        {
            var sql = new StringBuilder();
            foreach (var idValue in ids)
            {
                int result = 0;
                if (int.TryParse(idValue.ToString(), out result))
                    sql.AppendFormat("{0},", idValue);
                else
                    sql.AppendFormat("'{0}',", idValue.ToString().Replace("'", "''"));

            }
            if (sql.Length > 0)
                sql.Remove(sql.Length - 1, 1);
            return sql.Length == 0 ? null : sql.ToString();
        }
        /// <summary>
        /// 多表使用存储过程分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName">存储过程名称</param>
        /// <param name="tablename">表名(tA a inner join tB b On a.AID = b.AID)</param>
        /// <param name="tIndex">主键</param>
        /// <param name="tstrGetFields">要查询的字段，全部字段就为*</param>
        /// <param name="tsqlWhere">Where条件</param>
        /// <param name="pageIndex">开始页码</param>
        /// <param name="pageSize">每页查询数据的行数</param>
        /// <param name="torderFields">排序的字段</param>
        /// <param name="total">返回记录总数</param>
        /// <returns></returns>
        public static List<T> SelectByPage<T>(string procName, string tablename, string tIndex, string tstrGetFields, string tsqlWhere, int pageIndex, int pageSize, string torderFields, out int total)
        {
            IDbDataParameter pTotal = null;
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                var results = db.SqlList<T>(procName, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.AddParam("tableName", tablename);
                    cmd.AddParam("tIndex", tIndex);
                    cmd.AddParam("strGetFields", tstrGetFields);
                    cmd.AddParam("sqlWhere", tsqlWhere);
                    cmd.AddParam("pageIndex", pageIndex);
                    cmd.AddParam("pageSize", pageSize);
                    cmd.AddParam("orderFields", torderFields);
                    pTotal = cmd.AddParam("doCount", direction: ParameterDirection.Output);
                });
                total = int.Parse(pTotal.Value.ToString());
                return results;
            }
        }

        public static List<T> SelectIdentityUserByPage<T>(string tsqlWhere, int pageIndex, int pageSize, string torderFields, out int total)
        {
            IDbDataParameter pTotal = null;

            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                var results = db.SqlList<T>("P_IRPMemberList", cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.AddParam("strWhere", tsqlWhere);
                    cmd.AddParam("pageIndex", pageIndex);
                    cmd.AddParam("pageSize", pageSize);
                    cmd.AddParam("torderFields", torderFields);
                    pTotal = cmd.AddParam("rowCount", direction: ParameterDirection.Output);
                });
                total = int.Parse(pTotal.Value.ToString());
                return results;
            }
        }

        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        public static bool BulkInsert<T>(IEnumerable<T> objs)
        {
            bool flag = false;
            DataTable dt = BuildDataTable<T>(objs);

            SqlConnection sqlConn = new SqlConnection(Configuration.DbFactory.ConnectionString);
            sqlConn.Open();
            SqlTransaction sqlbulkTransaction = sqlConn.BeginTransaction();
            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.CheckConstraints, sqlbulkTransaction);
            bulkCopy.DestinationTableName = typeof(T).Name;
            bulkCopy.BatchSize = dt.Rows.Count;

            try
            {

                if (dt != null && dt.Rows.Count != 0)
                    bulkCopy.WriteToServer(dt);
                flag = true;
                sqlbulkTransaction.Commit();
            }
            catch (Exception exp)
            {
                sqlbulkTransaction.Rollback();
            }
            finally
            {
                sqlConn.Close();
                if (bulkCopy != null)
                    bulkCopy.Close();
            }
            return flag;
        }
        public static DataTable BuildDataTable<T>(IEnumerable<T> objs)
        {
            DataTable dt = new DataTable();
            var objProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            foreach (var propertyInfo in objProperties)
            {
                var ignoreAttribute = propertyInfo.FirstAttribute<IgnoreAttribute>();
                if (ignoreAttribute != null)
                    continue;
                if (propertyInfo.PropertyType.IsNullableType())
                {
                    dt.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType.GenericTypeArguments[0]));
                }
                else
                {
                    Type propertyType = propertyInfo.PropertyType;
                    if (propertyInfo.PropertyType.BaseType.Name == "Enum" && !propertyInfo.HasAttributeNamed(typeof(EnumAsIntAttribute).Name))
                    {
                        propertyType = typeof(string);
                    }
                    dt.Columns.Add(new DataColumn(propertyInfo.Name, propertyType));
                }
            }

            foreach (var item in objs)
            {
                DataRow row = dt.NewRow();
                foreach (var propertyInfo in objProperties)
                {
                    var ignoreAttribute = propertyInfo.FirstAttribute<IgnoreAttribute>();
                    if (ignoreAttribute != null)
                        continue;

                    var value = propertyInfo.GetValue(item);
                    if (value == null)
                    {
                        value = DBNull.Value;
                    }
                    else
                    {
                        if (propertyInfo.PropertyType.BaseType.Name == "Enum" && !propertyInfo.HasAttributeNamed(typeof(EnumAsIntAttribute).Name))
                        {
                            value = value.ToString();
                        }
                    }

                    row[propertyInfo.Name] = value;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        public static void SetIdentityField(bool isIdentity, string tableName, string fieldName)
        {
            using (var db = Configuration.DbFactory.OpenDbConnection())
            {
                string constraintKey = db.Scalar<string>(string.Format("select name FROM sys.indexes WHERE object_id = OBJECT_ID(N'{0}') and type_desc='CLUSTERED'", tableName));
                if (!string.IsNullOrEmpty(constraintKey))
                {
                    db.ExecuteSql(string.Format("ALTER TABLE {0} DROP CONSTRAINT {1}", tableName, constraintKey));
                }
                db.ExecuteSql(string.Format("ALTER TABLE {0} DROP COLUMN {1}; ALTER TABLE {0} ADD {1} INT {2} NOT NULL; ALTER TABLE {0} ADD PRIMARY KEY CLUSTERED ({1} ASC) ", tableName, fieldName, isIdentity ? "IDENTITY(1,1)" : ""));
            }
        }
    }
}
