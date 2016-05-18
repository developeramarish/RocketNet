using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RocketNet
{
    internal class ParamCollection : IEnumerable
    {
        private List<SqlParameter> parameters;
        internal ParamCollection()
        {
            this.parameters = new List<SqlParameter>();
        }

        /// <summary>
        /// Parametre koleksiyonuna SqlParameter tipinde nesne ekler
        /// </summary>
        /// <param name="item"></param>
        public void Add(SqlParameter item)
        {
            this.parameters.Add(item);
        }

        /// <summary>
        /// Parametre koleksiyonundan SqlParameter olarak belirtilen nesneyi siler
        /// </summary>
        /// <param name="item"></param>
        public void Remove(SqlParameter item)
        {
            this.parameters.Remove(item);
        }

        /// <summary>
        /// Parametre koleksiyonundan string olarak belirtilen nesneyi siler 
        /// </summary>
        /// <param name="parameterName"></param>
        public void Remove(string parameterName)
        {
            SqlParameter parameter = parameters.Find(x => x.ParameterName == parameterName.Trim());
            this.parameters.Remove(parameter);
        }

        /// <summary>
        /// Parametre koleksiyonundan indisi belirtilen nesneyi siler
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            this.parameters.RemoveAt(index);
        }

        /// <summary>
        /// Parametre koleksiyonundan tüm nesneleri siler
        /// </summary>
        public void Clear()
        {
            this.parameters.Clear();
        }

        /// <summary>
        /// Parametre koleksiyonunda string olarak belirtilen nesneyi SqlParameter olarak döndürür
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns>SqlParameter</returns>
        public SqlParameter Find(string parameterName)
        {
            return this.parameters.Find(x => x.ParameterName == parameterName.Trim());
        }

        /// <summary>
        /// Parametre koleksiyonunda string olarak belirtilen nesneleri SqlParameter koleksiyonu olarak döndürür
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns>List<SqlParameter></returns>
        public List<SqlParameter> FindAll(string parameterName)
        {
            return this.parameters.FindAll(x => x.ParameterName == parameterName.Trim());
        }

        /// <summary>
        /// Parametre koleksiyonunda indisi belirtilen nesneleri SqlParameter olarak döndürür
        /// </summary>
        /// <param name="i"></param>
        /// <returns>SqlParameter</returns>
        public SqlParameter this[int i]
        {
            get { return this.parameters[i]; }
            set { this.parameters[i] = value; }
        }

        /// <summary>
        /// Parametre koleksyinunu SqlParameter dizesi olarak döndürür
        /// </summary>
        /// <returns>SqlParameter[]</returns>
        public SqlParameter[] ToArray()
        {
            return this.parameters.ToArray();
        }

        public IEnumerator GetEnumerator()
        {
            return this.parameters.GetEnumerator();
        }

        /// <summary>
        /// Parametre koleksiyonundaki parametre sayısı döndürür.
        /// </summary>
        public int Count
        {
            get { return parameters.Count; }
        }

        /// <summary>
        /// Parametre koleksiyonundaki nesneleri birleştirip string olarak döndürür.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return string.Join(",", parameters.Select(x => x.ParameterName).ToArray());
        }
    }
}