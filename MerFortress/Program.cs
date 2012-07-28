using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerFortress
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (GameEngine.Engine engine = new GameEngine.Engine())
            {
                engine.Run();
            }
        }
    }
}
