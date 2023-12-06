using System.Data.SqlClient;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Interfaces;

namespace Entidades.DataBase
{
    public static class DataBaseManager
    {
        private static SqlConnection connection;
        private static string stringConnection;

        static DataBaseManager()
        {
            stringConnection = "Data Source=DESKTOP-FT8QR65\\NEWSERVER; Initial Catalog=20230622SP;Integrated Security=True";
            connection = new SqlConnection(stringConnection);   
        }

        public static bool GuardarTicket<T>(string nombreEmpleado,T comida) where T : IComestible,new() 
        {
            try
            {
                string comandoString = "INSERT INTO comidas (empleado, ticket) VALUES (@empleado,@ticket);";
                SqlCommand comandoObjeto = new SqlCommand();
                comandoObjeto.CommandType = System.Data.CommandType.Text;
                comandoObjeto.CommandText = comandoString;
                comandoObjeto.Connection = connection;
                comandoObjeto.Parameters.AddWithValue("@empleado", nombreEmpleado);
                comandoObjeto.Parameters.AddWithValue("@empleado", comida.Ticket);
            }
            catch(DataBaseManagerException)
            {
                throw;
            }catch (Exception ex) 
            {
                throw new DataBaseManagerException("No se pudo escribir el ticket en la base",ex);
            }
            finally
            {
                connection.Close();
            }


            return true;
        }
        public static string GetImagenComida(string tipo)
        {
            try
            {
                string comandoString = "SELECT * FROM comidas WHERE tipo_comida = @tipo ";
                SqlCommand comandoObjeto = new SqlCommand();
                comandoObjeto.CommandType = System.Data.CommandType.Text;
                comandoObjeto.CommandText = comandoString;
                comandoObjeto.Connection = connection;
                comandoObjeto.Parameters.AddWithValue("@tipo", tipo);
                connection.Open();
                using (SqlDataReader lector = comandoObjeto.ExecuteReader())
                {
                    lector.Read();
                    if (!lector.HasRows)
                    {
                        throw new DataBaseManagerException("No se encontro una comida de ese tipo");
                    }
                    return lector["imagen"].ToString();
                }

            }
            catch (DataBaseManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataBaseManagerException("Error leyendo la base de datos", ex);
            }
            finally{
                connection.Close();
            }
            
        }
    }

}
