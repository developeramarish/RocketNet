using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RocketNet
{
    public partial class Rocket
    {
        /// <summary>
        /// İşlem yapılacak input parametreleri belirtebilirsiniz.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        public void SetParameter(string parameterName, object parameterValue)
        {
                var parameteritem = parameters.Find(parameterName);
                if (parameteritem.IsNull())
                {
                    SqlParameter parameter = new SqlParameter();
                    parameter.ParameterName = parameterName.Trim();
                    parameter.Value = parameterValue;
                    parameter.Direction = ParameterDirection.Input;
                    parameters.Add(parameter);
                }
                else
                    throw new Exception("Aynı isimde bir parametre zaten var.");
        }

        /// <summary>
        /// İşlem yapılacak input parametreleri ve veri tipini belirtebilirsiniz.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        /// <param name="dbType"></param>
        public void SetParameter(string parameterName, object parameterValue, DbType dbType)
        {
            var parameteritem = parameters.Find(parameterName);
            if (parameteritem.IsNull())
            {
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = parameterName.Trim();
                parameter.Value = parameterValue;
                parameter.Direction = ParameterDirection.Input;
                parameter.DbType = dbType;
                parameters.Add(parameter);
            }
            else
                throw new Exception("Aynı isimde bir parametre zaten var.");
        }

        /// <summary>
        /// İşlem yapılacak output parametre adını belirtin.
        /// </summary>
        /// <param name="parameterName"></param>
        public void SetOutParameter(string parameterName)
        {
                var parameteritem = parameters.Find(parameterName);
                if (parameteritem.IsNull())
                {
                    SqlParameter parameter = new SqlParameter();
                    parameter.ParameterName = parameterName.Trim();
                    parameter.Direction = ParameterDirection.Output;
                    parameters.Add(parameter);
                }
                else
                    throw new Exception("Aynı isimde bir parametre zaten var.");
        }

        /// <summary>
        /// İşlem yapılacak output parametre adını ve veri tipini belirtin.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        public void SetOutParameter(string parameterName, DbType dbType)
        {
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = parameterName.Trim();
                parameter.Direction = ParameterDirection.Output;
                parameter.DbType = dbType;
                parameters.Add(parameter);
        }

        /// <summary>
        /// SetOutParameter ile belirttiğiniz parametrenen değerini adını belirterek elde edersiniz.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public object GetOutputValue(string parameterName)
        {
            SqlParameter parameter = parameters.Find(parameterName.Trim());
            if (!parameter.IsNull())
            {
                return parameter.Value;
            }
            throw new Exception("Output tipinde parametre bulunamadı.");
        }

        /// <summary>
        /// Bellekteki tüm parametreleri temizler.
        /// </summary>
        public void RemoveParameters()
        {
            parameters.Clear();
        }

        /// <summary>
        /// İndisi belirtilmiş parametreyi bellekten siler.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveParameters(int index)
        {
            parameters.RemoveAt(index);
        }

        /// <summary>
        /// SqlParameter tipinde belirtilen parametreyi bellekten siler.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveParameters(SqlParameter item)
        {
            parameters.Remove(item);
        }

        /// <summary>
        /// string tipte belirtilen parametreyi bellekten siler. Örneğin : @id
        /// </summary>
        /// <param name="parameterName"></param>
        public void RemoveParameters(string parameterName)
        {
            var item = parameters.Find(parameterName.Trim());

            if (!item.IsNull())
                RemoveParameters(item);
        }
    }
}