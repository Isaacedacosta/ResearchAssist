using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RA.Infra.Data.Repositories
{
    public class Repository<TypeEntity> where TypeEntity : class
    {


        string propertyName = "";
        string propertyValue = "";

        #region READ
        public TypeEntity GetById(TypeEntity getEntity)
        {
            string entityTable = getEntity.GetType().Name.ToString();
            string stringCommand = new StringBuilder($"SELECT * FROM {entityTable}s WHERE ").ToString();
            TypeEntity returnEntity = getEntity;

            foreach (var prop in getEntity.GetType().GetProperties())
            {
                if (prop.Name == "Id")
                {
                    propertyName = new StringBuilder(propertyName).Append(prop.Name).ToString();
                    propertyValue = new StringBuilder(propertyValue).Append($"{prop.GetValue(getEntity)}").ToString();
                }
            }
            stringCommand = new StringBuilder(stringCommand).Append($"{propertyName} = {propertyValue};").ToString();

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(stringCommand, RADBContext.RADBConnection);
                using (adapter)
                {
                    DataTable dataReceived = new DataTable();
                    adapter.Fill(dataReceived);
                    foreach (DataColumn column in dataReceived.Columns)
                    {
                        foreach (var prop in returnEntity.GetType().GetProperties())
                        {
                            if (prop.Name == column.ColumnName && dataReceived.Rows[0][column].ToString() != String.Empty)
                            {
                                var entProp = prop.Name;
                                var entPropValue = dataReceived.Rows[0][column];
                                prop.SetValue(returnEntity, entPropValue);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string erro = e.ToString();
                Console.WriteLine(erro);
                Console.WriteLine("erro");
                throw;
            }

            return returnEntity;
        }
        #endregion

        #region READALL
        public void GetAll(TypeEntity entity)
        {
            string entityTable = entity.GetType().Name.ToString();
            string stringCommand = new StringBuilder($"SELECT * FROM {entityTable}s;").ToString();

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(stringCommand, RADBContext.RADBConnection);
                using (adapter)
                {
                    DataTable dataReceived = new DataTable();
                    adapter.Fill(dataReceived);
                    Console.WriteLine(dataReceived);
                    foreach (DataRow row in dataReceived.Rows)
                    {
                        foreach (DataColumn column in dataReceived.Columns)
                        {
                            foreach (var prop in entity.GetType().GetProperties())
                            {
                                if (prop.Name == column.ColumnName)
                                {
                                    var entProp = prop.Name;
                                    var entPropValue = dataReceived.Rows[dataReceived.Rows.IndexOf(row)][column];
                                    prop.SetValue(entity, entPropValue);
                                    Console.WriteLine(entProp + ": " + entPropValue);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string erro = e.ToString();
                Console.WriteLine(erro);
                Console.WriteLine("erro");
                throw;
            }
        }
        #endregion

        #region CREATE
        public void Insert(TypeEntity entity)
        {
            string entityTable = entity.GetType().Name.ToString();
            string stringCommand = new StringBuilder($"INSERT INTO {entityTable}s ").ToString();


            foreach (var prop in entity.GetType().GetProperties())
            {
                if (prop.GetValue(entity) != null)
                {
                    if (prop.GetValue(entity).GetType() == typeof(DateTime))
                    {
                        DateTime dateToAdjust = (DateTime)prop.GetValue(entity);
                        string dateAdjustment = dateToAdjust.ToString("yyyy-MM-dd");
                        propertyName = new StringBuilder(propertyName).Append(prop.Name + ", ").ToString();
                        propertyValue = new StringBuilder(propertyValue).Append($"'{dateAdjustment}'" + ", ").ToString();
                        continue;
                    }
                    else
                    {
                        propertyName = new StringBuilder(propertyName).Append(prop.Name + ", ").ToString();
                        propertyValue = new StringBuilder(propertyValue).Append($"'{prop.GetValue(entity)}'" + ", ").ToString();
                    }

                }
            }
            propertyName = propertyName.Trim();
            propertyValue = propertyValue.Trim();

            if (propertyName.Trim().Last<char>() == ',')
            {
                propertyName = propertyName.Remove(propertyName.Length - 1);
            }
            if (propertyValue.Last<char>() == ',')
            {
                propertyValue = propertyValue.Remove(propertyValue.Length - 1);
            }

            stringCommand = new StringBuilder(stringCommand).Append($"({propertyName}) VALUES ({propertyValue});").ToString();
            Console.WriteLine(stringCommand);

            try
            {
                SqlCommand cmd = new SqlCommand(stringCommand, RADBContext.RADBConnection);
                RADBContext.RADBConnection.Open();
                cmd.ExecuteNonQuery();
                RADBContext.RADBConnection.Close();
                Console.WriteLine("Success!");

            }
            catch (Exception e)
            {
                string erro = e.ToString();
                Console.WriteLine(erro);
                Console.WriteLine("erro");
                throw;
            }

        }
        #endregion

        #region UPDATE
        public TypeEntity Update(TypeEntity entity)
        {
            string entityTable = entity.GetType().Name.ToString();
            PropertyInfo entityIdProperty = entity.GetType().GetProperty("Id");
            var targetId = entityIdProperty.GetValue(entity);
            string propetiesAndValues = "";

            if (entity == null)
            {
                throw new Exception("User not found");
            }


            foreach (var property in entity.GetType().GetProperties())
            {
                if (property.Name == "Id") { continue; }
                else if (property.GetValue(entity).GetType() == typeof(DateTime))
                {
                    DateTime dateToAdjust = (DateTime)property.GetValue(entity);
                    string dateAdjustment = dateToAdjust.ToString("yyyy-MM-dd");
                    propetiesAndValues = new StringBuilder(propetiesAndValues).Append($"{property.Name} = '{dateAdjustment}', ").ToString();
                    continue;
                }
                else
                {
                    propetiesAndValues = new StringBuilder(propetiesAndValues).Append($"{property.Name} = '{property.GetValue(entity)}', ").ToString();
                }

            }

            propetiesAndValues = propetiesAndValues.Trim();
            if (propetiesAndValues.Last<char>() == ',')
            {
                propetiesAndValues = propetiesAndValues.Remove(propetiesAndValues.Length - 1);
            }

            string stringCommand = new StringBuilder($"UPDATE {entityTable}s SET {propetiesAndValues} WHERE Id={targetId};").ToString();
            Console.WriteLine(stringCommand);
            try
            {
                SqlCommand cmd = new SqlCommand(stringCommand, RADBContext.RADBConnection);
                RADBContext.RADBConnection.Open();
                cmd.ExecuteNonQuery();
                RADBContext.RADBConnection.Close();
                Console.WriteLine("Success!");

            }
            catch (Exception e)
            {
                string erro = e.ToString();
                Console.WriteLine(erro);
                Console.WriteLine("erro");
                throw;
            }
            return null;
        }
        #endregion

        #region DELETE
        public void Delete(TypeEntity entity)
        {
            string entityTable = entity.GetType().Name.ToString();
            PropertyInfo entityIdProperty = entity.GetType().GetProperty("Id");
            var targetId = entityIdProperty.GetValue(entity);
            string stringCommand = new StringBuilder($"DELETE FROM {entityTable}s WHERE ID='{targetId}';").ToString();

            try
            {
                GetById(entity);
            }
            catch (Exception e)
            {
                string error = e.ToString();
                Console.WriteLine(error);
                throw;
            }


            try
            {
                SqlCommand cmd = new SqlCommand(stringCommand, RADBContext.RADBConnection);
                RADBContext.RADBConnection.Open();
                cmd.ExecuteNonQuery();
                RADBContext.RADBConnection.Close();
                Console.WriteLine("Success!");

            }
            catch (Exception e)
            {
                string erro = e.ToString();
                Console.WriteLine(erro);
                Console.WriteLine("erro");
                throw;
            }
        }
        #endregion



    }
}