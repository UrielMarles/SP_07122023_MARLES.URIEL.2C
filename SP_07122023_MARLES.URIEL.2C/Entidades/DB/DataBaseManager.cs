using System.Data.SqlClient;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Files;
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
            connection.Open();
            try
            {
                string comandoString = "INSERT INTO tickets (empleado, ticket) VALUES (@empleado,@ticket);";
                SqlCommand comandoObjeto = new SqlCommand();
                comandoObjeto.CommandType = System.Data.CommandType.Text;
                comandoObjeto.CommandText = comandoString;
                comandoObjeto.Connection = connection;
                comandoObjeto.Parameters.AddWithValue("@empleado", nombreEmpleado);
                comandoObjeto.Parameters.AddWithValue("@ticket", comida.Ticket);
                comandoObjeto.ExecuteNonQuery();
            }catch (Exception ex) 
            {
                FileManager.Guardar(ex.Message, "logs.txt", true);
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
            connection.Open();
            try
            {
                string comandoString = "SELECT * FROM comidas WHERE tipo_comida = @tipo ";
                SqlCommand comandoObjeto = new SqlCommand();
                comandoObjeto.CommandType = System.Data.CommandType.Text;
                comandoObjeto.CommandText = comandoString;
                comandoObjeto.Connection = connection;
                comandoObjeto.Parameters.AddWithValue("@tipo", tipo);
                using (SqlDataReader lector = comandoObjeto.ExecuteReader())
                {
                    lector.Read();
                    if (!lector.HasRows)
                    {
                        throw new ComidaInvalidaExeption("No se encontro una comida de ese tipo");
                    }
                    return lector["imagen"].ToString();
                }

            }
            catch (ComidaInvalidaExeption dbex)
            {
                FileManager.Guardar(dbex.Message, "logs.txt", true);
                throw;
            }
            catch (Exception ex)
            {
                FileManager.Guardar(ex.Message, "logs.txt", true);
                throw new DataBaseManagerException("No se pudo escribir el ticket en la base", ex);
            }
            finally
            {
                connection.Close();
            }

        }
    }

}
