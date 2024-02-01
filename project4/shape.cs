using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;



namespace shape
{
    using System.Collections;
    using System;

    [Serializable] 

    public abstract class shape  //base class shape
    {
        float x;  //private by default
        float y;  //private
        //Properties
        public float X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public abstract void Draw(Graphics g);
        public abstract bool isInside(int xP, int yP);

    }
    [Serializable]
    public abstract class Polygons : shape
    {

        public float width;
        public float height;

        public float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

    }

    [Serializable]
    public abstract class Elipticals : shape
    {
        public float radius;

        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }
    }

    [Serializable]
    public class myRectangle : Polygons
    {
        public myRectangle()
         : this(10, 10, 10, 20)
        { }

        public myRectangle(float xVal, float yVal, float wVal, float hVal)
        {
            X = xVal;
            Y = yVal;
            width = wVal;
            height = hVal;
        }

        public override void Draw(Graphics g)
        {
            SolidBrush br = new SolidBrush(Color.Pink);
            Pen pen = new Pen(Color.Pink, 2);
            g.FillRectangle(br, X - width / 2, Y - height / 2, width, height);
            g.DrawRectangle(pen, X - width / 2, Y - height / 2, width, height);
        }
        public override bool isInside(int xP, int yP)
        {
            return Math.Abs(xP - X) <= width / 2 && Math.Abs(yP - Y) <= height / 2;
        }

        ~myRectangle() { }
    }

    [Serializable]

    public class myCircle : Elipticals
    {
        public myCircle()
                    : this(10, 10, 5)
        { }

        public myCircle(float xVal, float yVal, float rVal)
        {
            X = xVal;
            Y = yVal;
            radius = rVal;
        }

        public override void Draw(Graphics g)
        {
            SolidBrush br = new SolidBrush(Color.Blue);
            Pen pen = new Pen(Color.Blue, 2);
            g.FillEllipse(br, X - radius, Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(pen, X - radius, Y - radius, 2 * radius, 2 * radius);
        }
        public override bool isInside(int xP, int yP)
        {
            return Math.Sqrt((xP - X) * (xP - X) + (yP - Y) * (yP - Y)) < radius;
        }

        ~myCircle() { }
    }
    [Serializable]
    public class myTriangle : Polygons
    {
        public myTriangle(float xVal, float yVal, float wVal, float hVal)
        {
            X = xVal;
            Y = yVal;
            width = wVal;  // init the base class 
            height = hVal; // init the base class 

        }

        // Return area of triangle. 
        public double area()
        {
            return width * height / 2;
        }

        static double area(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            return Math.Abs((x1 * (y2 - y3) +
                             x2 * (y3 - y1) +
                             x3 * (y1 - y2)) / 2.0);
        }
        public override void Draw(Graphics g)
        {
            Point[] vertices = new Point[]
        {
                new Point((int)X,(int)Y),  // Top vertex
                new Point((int)X+10, (int)Y+30), // Bottom-left vertex
                new Point((int)X-10, (int)Y+30)  // Bottom-right vertex
        };
            SolidBrush br = new SolidBrush(Color.Red);
            Pen pen = new Pen(Color.Red, 2);
            g.DrawPolygon(pen, vertices);
            g.FillPolygon(br, vertices);

        }
        public override bool isInside(int xP, int yP)
        {
            /* Calculate area of triangle ABC */
            double A = area((int)X, (int)Y, (int)X + 10, (int)Y + 30, (int)X - 10, (int)Y + 30);

            /* Calculate area of triangle PBC */
            double A1 = area(xP, yP, (int)X + 10, (int)Y + 30, (int)X - 10, (int)Y + 30);

            /* Calculate area of triangle PAC */
            double A2 = area((int)X, (int)Y, xP, yP, (int)X - 10, (int)Y + 30);

            /* Calculate area of triangle PAB */
            double A3 = area((int)X, (int)Y, (int)X + 10, (int)Y + 30, xP, yP);

            /* Check if sum of A1, A2 and A3 is same as A */
            return (A == A1 + A2 + A3);
        }
    }

    [Serializable]  //!7
    //!2b
    public class shapeList
    {
        protected SortedList shapes;

        public shapeList()
        {
            shapes = new SortedList();
        }
        public int NextIndex
        {
            get
            {
                return shapes.Count;
            }
            // there is no set
        }
        public shape this[int index]
        {
            get
            {
                if (index >= shapes.Count)
                    return (shape)null;
                //SortedList internal method
                return (shape)shapes.GetByIndex(index);
            }
            set
            {
                if (index <= shapes.Count)
                    shapes[index] = value; 		
            }
        }

        public void Remove(int element)
        {
            if (element >= 0 && element < shapes.Count)
            {
                for (int i = element; i < shapes.Count - 1; i++)
                    shapes[i] = shapes[i + 1];
                shapes.RemoveAt(shapes.Count - 1);
            }
        }

        // new method
        public void DrawAll(Graphics g)
        {
            shape prev, cur;
            for (int i = 1; i < shapes.Count; i++)
            {
                prev = (shape)shapes[i - 1];
                cur = (shape)shapes[i];
                g.DrawLine(Pens.Black, prev.X, prev.Y, cur.X, cur.Y);

                ((shape)shapes[i]).Draw(g);
            }
            for (int i = 0; i < shapes.Count; i++)
                ((shape)shapes[i]).Draw(g);
        }
        public void Clear()
        {
            shapes.Clear(); // Clear the internal SortedList
        }
    }
    [Serializable]
    public class DrawingData 
    {
        public Bitmap Paint { get; set; }
        public shapeList Objects { get; set; }
    }
}
