using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdoClass.DbUtil
{
    public class ShaneDbUtil
    {
        string DBConnectionString = "";

        public ShaneDbUtil(string DBConnectionString)
        {
            this.DBConnectionString = DBConnectionString;
        }

        /// <summary>
        /// 執行sql 不須回傳值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        public int Exec(string sql, object param)
        {
            var count = 0;
            try
            {
                using (var conn = new SqlConnection(this.DBConnectionString))
                {
                    using (var command = new SqlCommand(sql, conn))
                    {
                        conn.Open();

                        foreach (PropertyInfo propertyInfo in param.GetType().GetProperties())
                        {
                            // do stuff here
                            command.Parameters.AddWithValue($"@{propertyInfo.Name}", propertyInfo.GetValue(param, null));
                        }

                        count = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //Write Log
            }
            return count;
        }

        /// <summary>
        /// 執行sql 回傳第一列第一欄位
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        public string ExecScalar(string sql, object param)
        {
            string OnlyItem = "";
            try
            {
                using (var conn = new SqlConnection(this.DBConnectionString))
                {
                    using (var command = new SqlCommand(sql, conn))
                    {
                        conn.Open();

                        foreach (PropertyInfo propertyInfo in param.GetType().GetProperties())
                        {
                            // do stuff here
                            command.Parameters.AddWithValue($"@{propertyInfo.Name}", propertyInfo.GetValue(param, null));
                        }

                        OnlyItem = command.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                //Write Log
            }
            return OnlyItem;
        }

        /// <summary>
        /// 執行sql需回傳值
        /// </summary>
        /// <typeparam name="VModel">逾接回model的型態</typeparam>
        /// <param name="sql">sql 語法</param>
        /// <param name="param">參數</param>
        /// <returns></returns>
        public List<VModel> Exec<VModel>(string sql, object param)
        {
            List<VModel> result = new List<VModel>();
            try
            {
                using (var conn = new SqlConnection(this.DBConnectionString))
                {
                    using (var command = new SqlCommand(sql, conn))
                    {
                        conn.Open();

                        foreach (PropertyInfo propertyInfo in param.GetType().GetProperties())
                        {
                            // do stuff here
                            command.Parameters.AddWithValue($"@{propertyInfo.Name}", propertyInfo.GetValue(param, null));
                        }

                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result.Add(ConvertToVModel<VModel>(reader));
                            }
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //Write Log
            }
            return result;
        }

        /// <summary>
        /// Mapping Date Reader 與 Model
        /// </summary>
        /// <typeparam name="VModel"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        private VModel ConvertToVModel<VModel>(SqlDataReader dr)
        {
            VModel obj = default(VModel);
            obj = Activator.CreateInstance<VModel>();
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                if (!object.Equals(dr[prop.Name], DBNull.Value))
                {
                    prop.SetValue(obj, dr[prop.Name], null);
                }
            }

            return obj;
        }
    }
}

