using Castle.DynamicProxy;

namespace AutofacTesting
{
    public class AppInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            System.Console.WriteLine("Starting invocation");

            invocation.Proceed();

            System.Console.WriteLine("Invocation finished");
        }
    }
}