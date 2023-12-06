using Entidades.Exceptions;
using Entidades.Interfaces;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Entidades.Files
{
    
    public static  class FileManager
    {
        private static string path;

        static FileManager()
        {
            // ->INTERPRETACION 1: esto asigna la ruta desde el escritorio hasta esta carpeta
            //string st = Directory.GetCurrentDirectory();
            //string patron = @"^(.*\\SP_07122023_MARLES.URIEL.2C\\)";
            //Regex regex = new Regex(patron);
            //Match match = regex.Match(st);
            //path = match.Groups[1].Value;


            // ->INTERPRETACION 2: esto asigna el path a una carpeta creada en el escritorio
            string rutaAlEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path = Path.Combine(rutaAlEscritorio, "SP_07122023_MARLES.URIEL.2C");
            ValidarExistenciaDelDirectorio();
        }

        private static void ValidarExistenciaDelDirectorio()
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }catch (Exception ex)
                {
                    throw new FileManagerException("Error al crear el directorio",ex);
                }
            }
        }
        public static void Guardar(string data,string nombreArchivo,bool append)
        {
            string rutaArchivo = Path.Combine(path, nombreArchivo);
            using (StreamWriter sw = new StreamWriter(rutaArchivo, append))
            {
                // Escribir el texto al final del archivo
                sw.WriteLine(data);
            }
        }
        public static bool Serializar<T>(T elemento,string nombreArchivo) where T : class
        {
            string ObjetoSerializado = JsonSerializer.Serialize(elemento);
            Guardar(ObjetoSerializado, nombreArchivo, false);
            return true;
        }
    }
}
