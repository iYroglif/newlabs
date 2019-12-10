using System;

namespace laba2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Терентьев Владислав ИУ5-33";
            Rectangle a = new Rectangle(4, 8);
            a.Print();
            Square b = new Square(5.5);
            b.Print();
            Circle c = new Circle(3);
            c.Print();
            Console.ReadKey();
        }
    }

    abstract class GeomFigure
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

        public override string ToString() { return Shape + " площадью " + _area.ToString(); }
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

        public override string ToString() { return base.ToString() + " (стороной " + Height + ")"; } //Можно ли как то обратиться к сАмому родительскому классу?
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

}