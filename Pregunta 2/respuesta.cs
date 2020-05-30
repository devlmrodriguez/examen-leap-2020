using System;

class Pregunta2
{
    class Rectangle
    {
        private float posX; //Centrado
        private float posY; //Centrado
        private float width;
        private float height;

        private float top;
        private float right;
        private float bottom;
        private float left;

        public Rectangle(float posX, float posY, float width, float height)
        {
            this.posX = posX;
            this.posY = posY;
            this.width = width;
            this.height = height;

            top = posY + height / 2f;
            right = posX + width / 2f;
            bottom = posY - height / 2f;
            left = posX - width / 2f;
        }

        public float GetTop() { return top; }
        public float GetRight() { return right; }
        public float GetBottom() { return bottom; }
        public float GetLeft() { return left; }
    }

    bool CheckDoubleRectangleCollision(Rectangle r1, Rectangle r2)
    {
        //Colisión 2D entre 2 rectángulos
        return
            r1.GetTop() >= r2.GetBottom() &&
            r1.GetRight() >= r2.GetLeft() &&
            r1.GetBottom() <= r2.GetTop() &&
            r1.GetLeft() <= r2.GetRight();
    }

    bool CheckTripleRectangleCollision(Rectangle r1, Rectangle r2, Rectangle r3)
    {
        //Colisión 2D entre 3 rectángulos al mismo tiempo
        return
            CheckDoubleRectangleCollision(r1, r2) && 
            CheckDoubleRectangleCollision(r2, r3) && 
            CheckDoubleRectangleCollision(r1, r3);
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Pregunta 2 - Respuesta");
    }
}
