using System.Reflection;
using System.Reflection.Emit;

namespace DynamicallyCreateClassRuntimeTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var classFullPath = @"D:\Tests\XsdHandlingTest\XsdHandlingTest\PurchaseOrderSchema.cs";
            using (StreamReader streamReader = new StreamReader(classFullPath))
            {
                Console.WriteLine(streamReader.ReadToEnd());
            }
            MyClassBuilder MCB = new MyClassBuilder("Student");
            var myclass = MCB.CreateObject(new string[3] { "ID", "Name", "Address" }, new Type[3] { typeof(string), typeof(string), typeof(string) });
            Type TP = myclass.GetType();

            foreach (PropertyInfo PI in TP.GetProperties())
            {
                Console.WriteLine(PI.Name);
            }
            Console.ReadLine();
        }
    }
}
