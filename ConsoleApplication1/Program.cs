using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot_Arm;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var Arm = new Base();
            Arm.Grab("Orange");
        }
    }
}
