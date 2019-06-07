using System.Collections.Generic;
using System.Linq;

namespace AutofacTesting
{
    public class App : IApp
    {
        private IDependency _dep;
        private readonly IEnumerable<InputMetadata> _inputs;

        public App(IDependency dependency, IEnumerable<InputMetadata> inputs)
        {
            _dep = dependency;
            _inputs = inputs;
        }

        public void Run()
        {
            foreach (var input in _inputs ?? Enumerable.Empty<InputMetadata>())
            {
                System.Console.WriteLine($"configurated input: {{hostname: {input.Hostname}, retries: {input.Retries}}}");
            }
            _dep.DoSomething();
        }
    }
}