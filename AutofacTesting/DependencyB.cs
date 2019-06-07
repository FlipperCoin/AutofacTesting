using System;
using System.Collections.Generic;
using System.Text;

namespace AutofacTesting
{
    public class DependencyB : IDependency
    {
        public void DoSomething()
        {
            Console.WriteLine("B");
        }
    }
}
