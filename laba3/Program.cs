using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba3
{

    class Program
    {
        static void Main(string[] args)
        {
            Rectangle rect = new Rectangle(5, 4);
            Square square = new Square(5);
            Circle circle = new Circle(5);
            Console.WriteLine("\nArrayList");

            ArrayList al = new ArrayList();
            al.Add(circle);
            al.Add(rect);
            al.Add(square);
            foreach (var x in al) Console.WriteLine(x);
            Console.WriteLine("\nArrayList - сортировка");
            al.Sort();
            foreach (var x in al) Console.WriteLine(x);
            Console.WriteLine("\nList<Figure>");

            List<GeomFigure> fl = new List<GeomFigure>();
            fl.Add(circle);
            fl.Add(rect);
            fl.Add(square);
            foreach (var x in fl) Console.WriteLine(x);
            Console.WriteLine("\nList<Figure> - сортировка");
            fl.Sort();
            foreach (var x in fl) Console.WriteLine(x);
            Console.WriteLine("\nМатрица");

            Matrix3D<GeomFigure> cube = new Matrix3D<GeomFigure>(3, 3, 3, null);
            cube[0, 0, 0] = rect;
            cube[1, 1, 1] = square;
            cube[2, 2, 2] = circle;
            Console.WriteLine(cube.ToString());
            Console.WriteLine("\nСписок");

            SimpleList<GeomFigure> list = new SimpleList<GeomFigure>();
            list.Add(square);
            list.Add(rect);
            list.Add(circle);
            foreach (var x in list) Console.WriteLine(x);
            list.Sort();
            Console.WriteLine("\nСортировка списка");
            foreach (var x in list) Console.WriteLine(x);
            Console.WriteLine("\nСтек");
            SimpleStack<GeomFigure> stack = new SimpleStack<GeomFigure>();
            stack.Push(rect);
            stack.Push(square);
            stack.Push(circle);
            while (stack.Count > 0)
            {
                GeomFigure f = stack.Pop();
                Console.WriteLine(f);
            }

            Console.ReadKey();
        }
    }

    abstract class GeomFigure : IComparable
    {
        string _shape;
        double _area;

        public string Shape
        {
            get { return _shape; }
            protected set { _shape = value; }
        }

        public double Area
        {
            get { return _area; }
            protected set { _area = value; }
        }

        public abstract double CalcArea(); //Смысл делать virtual?

        public int CompareTo(object o)
        {
            GeomFigure a = (GeomFigure)o;
            if (_area < a._area) return -1;
            else if (_area == a._area) return 0;
            else return 1;
        }

        public virtual string ToString() { return _shape + " плошадью " + _area.ToString(); }
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

        public override string ToString() { return base.ToString() + ", высотой " + _height + " и шириной " + _weight; }

        public void Print() { Console.WriteLine(ToString()); }
    }

    class Square : Rectangle
    {
        public Square(double leng) : base(leng, leng) { Shape = "Квадрат"; }

        public override string ToString() { return base.ToString() + " и стороной " + Height; } //Как обратиться к сАмому родительскому классу?
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

        public override string ToString() { return base.ToString() + " и радиусом " + _radius; }

        public void Print() { Console.WriteLine(ToString()); }
    }

    public class Matrix3D<T>
    {
        /// <summary>
        /// Словарь для хранения значений
        /// </summary>
        Dictionary<string, T> _matrix = new Dictionary<string, T>();
        /// <summary>
        /// Количество элементов по горизонтали (максимальное количество столбцов)
        /// </summary>
        int maxX;
        /// <summary>
        /// Количество элементов по вертикали (максимальное количество строк)
        /// </summary>
        int maxY;
        /// <summary>
        /// Пустой элемент, который возвращается если элемент с нужными координатами не был задан
        /// </summary>
        ///+++++++++++++++++++++++++++++++++++++ 
        int maxZ;
        ///+++++++++++++++++++++++++++++++++++++
        T nullElement;
        /// <summary>
        /// Конструктор
        /// </summary>
        public Matrix3D(int px, int py, int pz, T nullElementParam)
        {
            this.maxX = px;
            this.maxY = py;
            this.maxZ = pz;
            this.nullElement = nullElementParam;
        }
        /// <summary>
        /// Индексатор для доступа к данных
        /// </summary>
        public T this[int x, int y, int z]
        {
            get
            {
                CheckBounds(x, y, z);
                string key = DictKey(x, y, z);
                if (this._matrix.ContainsKey(key)) { return this._matrix[key]; }
                else { return this.nullElement; }
            }
            set
            {
                CheckBounds(x, y, z);
                string key = DictKey(x, y, z);
                this._matrix.Add(key, value);
            }
        }
        /// <summary>
        /// Проверка границ
        /// </summary>
        void CheckBounds(int x, int y, int z)
        {
            if (x < 0 || x >= this.maxX) throw new Exception("x=" + x + " выходит за границы");
            if (y < 0 || y >= this.maxY) throw new Exception("y=" + y + " выходит за границы");
            if (z < 0 || z >= this.maxZ) throw new Exception("z=" + z + " выходит за границы");
        }
        /// <summary>
        /// Формирование ключа
        /// </summary>
        string DictKey(int x, int y, int z) { return x.ToString() + "_" + y.ToString() + "_" + z.ToString(); }
        /// <summary>
        /// Приведение к строке
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //Класс StringBuilder используется для построения длинных строк
            //Это увеличивает производительность по сравнению с созданием и склеиванием
            //большого количества обычных строк
            StringBuilder b = new StringBuilder();
            for (int k = 0; k < this.maxZ; k++)
            {
                for (int j = 0; j < this.maxY; j++)
                {
                    b.Append("[");
                    for (int i = 0; i < this.maxX; i++)
                    {
                        if (i > 0) b.Append("\t\t");
                        if (this[i, j, k] == null) b.Append("---------------");
                        else b.Append(this[i, j, k].ToString());
                    }
                    b.Append("]\n");
                }
                b.Append("\n");
            }
            return b.ToString();
        }
    }

    /// <summary>
    /// Элемент списка
    /// </summary>
    public class SimpleListItem<T>
    {
        /// <summary>
        /// Данные
        /// </summary>
        public T data { get; set; }
        /// <summary>
        /// Следующий элемент
        /// </summary>
        public SimpleListItem<T> next { get; set; }
        ///конструктор
        public SimpleListItem(T param)
        {
            this.data = param;
        }
    }

    /// <summary>
    /// Список
    /// </summary>
    public class SimpleList<T> : IEnumerable<T>
    where T : IComparable
    {
        /// <summary>
        /// Первый элемент списка
        /// </summary>
        protected SimpleListItem<T> first = null;
        /// <summary>
        /// Последний элемент списка
        /// </summary>
        protected SimpleListItem<T> last = null;
        /// <summary>
        /// Количество элементов
        /// </summary>
        public int Count
        {
            get { return _count; }
            protected set { _count = value; }
        }
        int _count;
        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="element"></param>
        public void Add(T element)
        {
            SimpleListItem<T> newItem = new SimpleListItem<T>(element);
            this.Count++;
            //Добавление первого элемента
            if (last == null)
            {
                this.first = newItem;
                this.last = newItem;
            }
            //Добавление следующих элементов
            else
            {
                //Присоединение элемента к цепочке
                this.last.next = newItem;
                //Просоединенный элемент считается последним
                this.last = newItem;
            }
        }
        /// <summary>
        /// Чтение контейнера с заданным номером
        /// </summary>
        public SimpleListItem<T> GetItem(int number)
        {
            if ((number < 0) || (number >= this.Count))
            {
                //Можно создать собственный класс исключения
                throw new Exception("Выход за границу индекса");
            }
            SimpleListItem<T> current = this.first;
            int i = 0;
            //Пропускаем нужное количество элементов
            while (i < number)
            {
                //Переход к следующему элементу
                current = current.next;
                //Увеличение счетчика
                i++;
            }
            return current;
        }
        /// <summary>
        /// Чтение элемента с заданным номером
        /// </summary>
        public T Get(int number)
        {
            return GetItem(number).data;
        }
        /// <summary>
        /// Для перебора коллекции
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            SimpleListItem<T> current = this.first;
            //Перебор элементов
            while (current != null)
            {
                //Возврат текущего значения
                yield return current.data;
                //Переход к следующему элементу
                current = current.next;
            }
        }
        //Реализация обощенного IEnumerator<T> требует реализации необобщенного интерфейса
        //Данный метод добавляется автоматически при реализации интерфейса
        System.Collections.IEnumerator
        System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// Cортировка
        /// </summary>
        public void Sort()
        {
            Sort(0, this.Count - 1);
        }
        /// <summary>
        /// Реализация алгоритма быстрой сортировки
        /// </summary>
        /// <param name="low"></param>
        /// <param name="high"></param>
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
        /// <summary>
        /// Вспомогательный метод для обмена элементов при сортировке
        /// </summary>
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
            this.Count++;
            if (last == null)
            {
                this.first = newItem;
                this.last = newItem;
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