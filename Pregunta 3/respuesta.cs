using System;

class Pregunta3
{
    class Circunference
    {
        private float radius;
        private float posX;
        private float posY;

        public Circunference(float radius, float posX, float posY)
        {
            this.radius = radius;
            this.posX = posX;
            this.posY = posY;
        }

        public float GetRadius() { return radius; }
        public float GetPosX() { return posX; }
        public float GetPosY() { return posY; }
    }

    float ConvertDegreesToRadians(float degrees)
    {
        float radians = (MathF.PI / 180f) * degrees;
        return radians;
    }

    void DrawPoint(float posX, float posY)
    {
        //TODO(Lmrodriguez): Esto debe implementarse dependiendo de la librería o motor gráfico a usar
    }

    void DrawSemicircunference(int points, Circunference circunference)
    {
        //Estos ángulos corresponden al intervalo en coordenadas polares de una circunferencia
        //Para una semicircunferencia superior, usar 0 y 180
        //Para una semicircunferencia inferior, usar 180 y 360
        float startAngle = 0f;
        float endAngle = 180f;
        //Distancia en ángulos entre cada punto, se resta 1 a la cantidad original para siempre tomar en cuenta el ángulo inicial
        float angleStep = (endAngle - startAngle) / (points - 1);

        //Para cada punto, calcular su ángulo respectivo y junto con el radio obtener sus coordenadas cartesianas, asumiendo el origen en (0,0)
        for (int i = 0; i < points; i++)
        {
            //Calcular el ángulo para este punto específico
            float angle = startAngle + (i * angleStep);

            //Convertir coordenadas polares con el radio y el ángulo a cartesianas
            float posX = circunference.GetRadius() * MathF.Cos(ConvertDegreesToRadians(angle));
            float posY = circunference.GetRadius() * MathF.Sin(ConvertDegreesToRadians(angle));

            //Trasladar del origen (0,0) a la posición real
            posX += circunference.GetPosX();
            posY += circunference.GetPosY();

            DrawPoint(posX, posY);
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Pregunta 3 - Respuesta");
    }
}