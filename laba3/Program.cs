using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace laba3
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Терентьев Владислав ИУ5-33";
            Rectangle rectangle = new Rectangle(5, 4);
            Square square = new Square(5);
            Circle circle = new Circle(5);

            Console.WriteLine("ArrayList");
            ArrayList collection = new ArrayList();
            collection.Add(circle);
            collection.Add(rectangle);
            collection.Add(square);
            foreach (object o in collection)
            {
                Console.WriteLine(o);
            }

            Console.WriteLine("\nList<GeomFigure>");
            List<GeomFigure> coll2 = new List<GeomFigure>();
            coll2.Add(circle);
            coll2.Add(rectangle);
            coll2.Add(square);
            foreach (object o in coll2)
            {
                Console.WriteLine(o);
            }
            Console.WriteLine("\nList<GeomFigure> - сортировка");
            coll2.Sort();
            foreach (object o in coll2)
            {
                Console.WriteLine(o);
            }

            //
            Console.WriteLine("\nМатрица");
            Matrix3D<GeomFigure> cube = new Matrix3D<GeomFigure>(3, 3, 3, null);
            cube[0, 0, 2] = rectangle;
            cube[1, 1, 1] = square;
            cube[2, 2, 0] = circle;
            Console.WriteLine(cube.ToString());
            //

            Console.WriteLine("\nСписок");
            SimpleList<GeomFigure> list = new SimpleList<GeomFigure>();
            list.Add(square);
            list.Add(rectangle);
            list.Add(circle);
            foreach (var o in list)
            {
                Console.WriteLine(o);
            }
            list.Sort();
            Console.WriteLine("\nСортировка списка");
            foreach (var o in list)
            {
                Console.WriteLine(o);
            }

            Console.WriteLine("\nСтек");
            SimpleStack<GeomFigure> stack = new SimpleStack<GeomFigure>();
            stack.Push(rectangle);
            stack.Push(square);
            stack.Push(circle);
            while (stack.Count > 0)
            {
                GeomFigure tmp = stack.Pop();
                Console.WriteLine(tmp);
            }
            Console.ReadKey();
        }
    }

    abstract class GeomFigure : IComparable
    {
        double _area;

        public string Shape { get; protected set; }

        public double Area
        {
            get { return _area; }
            protected set { _area = value; }
        }

        public abstract double CalcArea(); //Можно ли сделать abstract вместо virtual и зачем делать virtual?
                                           //public virtual double CalcArea() { return Area; }

        public int CompareTo(object o)
        {
            GeomFigure a = (GeomFigure)o;
            if (_area < a._area) return -1;
            else if (_area == a._area) return 0;
            else return 1;
        }

        public override string ToString() { return Shape + " плошадью " + _area.ToString("0.00"); }
    }

    interface IPrint { void Print(); }

    class Rectangle : GeomFigure, IPrint
    {
        double _height = 0;
        double _weight = 0;

        Rectangle() { Shape = "Прямоугольник"; }

        public override double CalcArea() { return _height * _weight; }

        public double Height
        {
            get { return _height; }
            private set
            {
                if (value < 0) { throw new Exception("Высота не может быть отрицательной"); }
                else { _height = value; }
            }
        }

        public double Weight
        {
            get { return _weight; }
            private set
            {
                if (value < 0) { throw new Exception("Ширина не может быть отрицательной"); }
                else { _weight = value; }
            }
        }

        public Rectangle(double h, double w) : this()
        {
            Height = h;
            Weight = w;
            Area = CalcArea();
        }

        //public override string ToString() { return base.ToString() + ", высотой " + _height + " и шириной " + _weight; }

        public void Print() { Console.WriteLine(ToString()); }
    }

    class Square : Rectangle
    {
        public Square(double leng) : base(leng, leng) { Shape = "Квадрат"; }

        //public override string ToString() { return base.ToString() + " (стороной " + Height + ")"; } //Можно ли как то обратиться к сАмому родительскому классу?
    }

    class Circle : GeomFigure, IPrint
    {
        double _radius = 0;

        Circle() { Shape = "Окружность"; }

        public override double CalcArea() { return Math.PI * _radius * _radius; }

        public double Radius
        {
            get { return _radius; }
            private set
            {
                if (value < 0) { throw new Exception("Радиус не может быть отрицательным"); }
                else { _radius = value; }
            }
        }

        public Circle(double r) : this()
        {
            Radius = r;
            Area = CalcArea();
        }

        //public override string ToString() { return base.ToString() + " и радиусом " + _radius; }

        public void Print() { Console.WriteLine(ToString()); }
    }

    public class Matrix3D<T>
    {
        Dictionary<string, T> _matrix = new Dictionary<string, T>();
        int maxX;
        int maxY;
        int maxZ;
        T nullElement;

        public Matrix3D(int px, int py, int pz, T nullElementParam)
        {
            maxX = px;
            maxY = py;
            maxZ = pz;
            nullElement = nullElementParam;
        }

        public T this[int x, int y, int z]
        {
            get
            {
                CheckBounds(x, y, z);
                string key = DictKey(x, y, z);
                if (_matrix.ContainsKey(key)) { return _matrix[key]; }
                else { return nullElement; }
            }
            set
            {
                CheckBounds(x, y, z);
                string key = DictKey(x, y, z);
                _matrix.Add(key, value);
            }
        }

        void CheckBounds(int x, int y, int z)
        {
            if (x < 0 || x >= maxX) throw new Exception("x=" + x + " выходит за границы");
            if (y < 0 || y >= maxY) throw new Exception("y=" + y + " выходит за границы");
            if (z < 0 || z >= maxZ) throw new Exception("z=" + z + " выходит за границы");
        }

        string DictKey(int x, int y, int z) { return x.ToString() + "_" + y.ToString() + "_" + z.ToString(); }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            for (int k = 0; k < maxZ; k++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    b.Append("[");
                    for (int i = 0; i < maxX; i++)
                    {
                        if (i > 0) b.Append("\t");
                        if (this[i, j, k] == null) b.Append("----------------------------");
                        else b.Append($"{this[i, j, k].ToString(), 27}");
                    }
                    b.Append("]\n");
                }
                b.Append("\n");
            }
            return b.ToString();
        }
    }

    public class SimpleListItem<T>
    {
        public T data { get; set; }

        public SimpleListItem<T> next { get; set; }

        public SimpleListItem(T param)
        {
            data = param;
        }
    }

    public class SimpleList<T> : IEnumerable<T>
    where T : IComparable
    {
        protected SimpleListItem<T> first = null;

        protected SimpleListItem<T> last = null;

        public int Count { get; protected set; }

        public void Add(T element)
        {
            SimpleListItem<T> newItem = new SimpleListItem<T>(element);
            Count++;
            if (last == null)
            {
                first = newItem;
                last = newItem;
            }
            else
            {
                last.next = newItem;
                last = newItem;
            }
        }

        public SimpleListItem<T> GetItem(int number)
        {
            if ((number < 0) || (number >= Count))
            {
                throw new Exception("Выход за границу индекса");
            }
            SimpleListItem<T> current = first;
            int i = 0;
            while (i < number)
            {
                current = current.next;
                i++;
            }
            return current;
        }

        public T Get(int number)
        {
            return GetItem(number).data;
        }

        public IEnumerator<T> GetEnumerator()
        {
            SimpleListItem<T> current = first;

            while (current != null)
            {
                yield return current.data;
                current = current.next;
            }
        }

        IEnumerator
        IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Sort()
        {
            Sort(0, Count - 1);
        }

        private void Sort(int low, int high)
        {
            int i = low;
            int j = high;
            T x = Get((low + high) / 2);
            do
            {
                while (Get(i).CompareTo(x) < 0) ++i;
                while (Get(j).CompareTo(x) > 0) --j;
                if (i <= j)
                {
                    Swap(i, j);
                    i++; j--;
                }
            } while (i <= j);
            if (low < j) Sort(low, j);
            if (i < high) Sort(i, high);
        }

        private void Swap(int i, int j)
        {
            SimpleListItem<T> ci = GetItem(i);
            SimpleListItem<T> cj = GetItem(j);
            T temp = ci.data;
            ci.data = cj.data;
            cj.data = temp;
        }
    }

    class SimpleStack<T> : SimpleList<T> where T : IComparable
    {
        public bool Empty()
        {
            if (last == null) return true;
            else return false;
        }

        public void Push(T element)
        {
            SimpleListItem<T> newItem = new SimpleListItem<T>(element);
            Count++;
            if (last == null)
            {
                first = newItem;
                last = newItem;
            }
            else
            {
                newItem.next = last;
                last = newItem;
            }
        }

        public T Pop()
        {
            if (Empty()) { throw new Exception("Удаление элемента из пустого стека"); }
            else
            {
                Count--;
                T temp = last.data;
                if (last.next == null) { last = null; first = null; }
                else { last = last.next; }
                return temp;
            }
        }
    }
}