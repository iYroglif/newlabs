using System;
using System.Linq;
using System.Reflection;

namespace laba6_part2
{
    class Program
    {
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class NewAttribute : Attribute
        {
            public NewAttribute() { }
            public NewAttribute(string DescriptionParam)
            {
                Description = DescriptionParam;
            }
            public string Description { get; set; }
        }

        public class ForInspection : IComparable
        {
            public ForInspection() { }
            public ForInspection(int i) { }
            public ForInspection(string str) { }
            public int Plus(int x, int y) { return x + y; }
            public int Minus(int x, int y) { return x - y; }

            [New("Описание для Property1")]
            public string Property1 { get; set; }

            public int Property2 { get; set; }

            [New(Description = "Описание для Property3")]
            public double Property3 { get; private set; }

            public int field1;
            public float field2;
            public int CompareTo(object obj)
            {
                return 0;
            }
        }

        public static bool GetPropertyAttribute(PropertyInfo checkType, Type attributeType, out object attribute)
        {
            bool Result = false;
            attribute = null;
            var isAttribute = checkType.GetCustomAttributes(attributeType, false);
            if (isAttribute.Length > 0)
            {
                Result = true;
                attribute = isAttribute[0];
            }
            return Result;
        }

        static void AssemblyInfo()
        {
            Console.WriteLine("Вывод информации о сборке:");
            Assembly i = Assembly.GetExecutingAssembly();
            Console.WriteLine("Полное имя:" + i.FullName);
            Console.WriteLine("Исполняемый файл:" + i.Location);
        }

        static void TypeInfo()
        {
            Type t = typeof(ForInspection);
            Console.WriteLine("\nИнформация о типе:");
            Console.WriteLine("Тип " + t.FullName + " унаследован от " + t.BaseType.FullName);
            Console.WriteLine("Пространство имен " + t.Namespace);
            Console.WriteLine("Находится в сборке " + t.AssemblyQualifiedName);
            Console.WriteLine("\nКонструкторы:");
            foreach (var x in t.GetConstructors())
            {
                Console.WriteLine(x);
            }
            Console.WriteLine("\nМетоды:");
            foreach (var x in t.GetMethods())
            {
                Console.WriteLine(x);
            }
            Console.WriteLine("\nСвойства:");
            foreach (var x in t.GetProperties())
            {
                Console.WriteLine(x);
            }
            Console.WriteLine("\nПоля данных (public):");
            foreach (var x in t.GetFields())
            {
                Console.WriteLine(x);
            }
            Console.WriteLine("\nForInspection реализует IComparable -> " +
            t.GetInterfaces().Contains(typeof(IComparable))
            );
        }

        static void InvokeMemberInfo()
        {
            Type t = typeof(ForInspection);
            Console.WriteLine("\nВызов метода:");
            ForInspection fi = (ForInspection)t.InvokeMember(null, BindingFlags.CreateInstance, null, null, new object[] { });
            object[] parameters = new object[] { 3, 2 };
            object Result = t.InvokeMember("Plus", BindingFlags.InvokeMethod, null, fi, parameters);
            Console.WriteLine("Plus(3,2)={0}", Result);
        }

        static void AttributeInfo()
        {
            Type t = typeof(ForInspection);
            Console.WriteLine("\nСвойства, помеченные атрибутом:");
            foreach (var x in t.GetProperties())
            {
                object attrObj;
                if (GetPropertyAttribute(x, typeof(NewAttribute), out attrObj))
                {
                    NewAttribute attr = attrObj as NewAttribute;
                    Console.WriteLine(x.Name + " - " + attr.Description);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "Терентьев Владислав ИУ5-33";
            AssemblyInfo();
            TypeInfo();
            InvokeMemberInfo();
            AttributeInfo();
            Console.ReadLine();
        }
    }
}