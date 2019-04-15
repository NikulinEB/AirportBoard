using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBoard
{
    static class FileOperations
    {
        public static void Save(string directory, string fileName, string content)
        {
            using (FileStream fileStream = File.Open(Path.Combine(directory, fileName), FileMode.Create, FileAccess.Write))
            {
                StreamWriter writer = new StreamWriter(fileStream);
                writer.Write(content);
                writer.Close();
            }
        }

        public static string Load(string directory, string fileName)
        {
            using (FileStream fileStream = File.Open(Path.Combine(directory, fileName), FileMode.OpenOrCreate, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(fileStream);
                string messages = reader.ReadToEnd();
                reader.Close();
                return messages;
            }
        }
    }
}