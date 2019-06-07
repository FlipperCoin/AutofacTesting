namespace AutofacTesting
{
    public class DependencyA : IDependency
    {
        public void DoSomething()
        {
            System.Console.WriteLine("A");
        }
    }
}