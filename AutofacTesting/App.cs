namespace AutofacTesting
{
    public class App : IApp
    {
        private IDependency _dep;

        public App(IDependency dependency)
        {
            _dep = dependency;
        }

        public void Run()
        {
            _dep.DoSomething();
        }
    }
}