using System;

namespace FacebookInformationPoster
{
    public class Writer : IWriter
    {
        private readonly object _object = new();

        public void Log(string output)
        {
            lock (_object)
            {
                Console.WriteLine($"[{DateTime.Now}] {output}");
            }
        }
    }
}
