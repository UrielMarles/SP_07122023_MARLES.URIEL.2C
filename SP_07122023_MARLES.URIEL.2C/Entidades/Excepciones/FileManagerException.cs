using Entidades.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Exceptions
{
    public class FileManagerException : Exception
    {
        public FileManagerException(string message,Exception innerException): base(message, innerException) { FileManager.Guardar(message, "logs.txt", true); }

    }
}
