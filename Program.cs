// Сапёр
Random rnd = new Random();
Sapper game = new Sapper(rnd.Next(5, 16), rnd.Next(5, 16));
int x, y;

game.PrintField();

//while(true)
//{
//    Console.WriteLine("Сапёр\n\n1. Ввести размеры поля вручную\n2. Оставить как есть ({}{})");
//    Console.WriteLine("Введите высоту поля -> ");
//    Console.WriteLine("Введите длину поля по X -> ");
//}

interface ISapper
{
    void GenerateMines(int quantity);
    void PrintField();
}

class Dot
{
    public int Type { get; set; } // -2(#) - флаг / -1(*) - мина / 0(.) - пустая клетка / 123456789 цифры
    public bool Show { get; set; }
    public Dot() { Type = 0; }
}

class Sapper : ISapper
{
    Random rnd = new Random();

    int y, x, mines;
    Dot[,] field; // i = y, j = x 

    public Sapper(int y, int x)
    {
        this.y = y;
        this.x = x;

        field = new Dot[y, x];

        for (int i = 0; i < y; i++)
            for (int j = 0; j < x; j++)
                field[i, j] = new Dot();

        if(y*x > 50) GenerateMines(rnd.Next(16, 26));
        if(y*x > 75) GenerateMines(rnd.Next(26, 36));
        if (y * x > 150) GenerateMines(rnd.Next(36, 56));
        else GenerateMines(rnd.Next(6, 16));
    }

    public void GenerateMines(int quantity)
    {
        mines = quantity;
        for (int i = 0; i < quantity; i++)
        {
            while(true)
            {
                int _y = rnd.Next(0, y);
                int _x = rnd.Next(0, x);
                if (field[_y, _x].Type == 0)
                {
                    field[_y, _x].Type = -1;
                    break;
                }
            }            
        }
    }

    public void PrintField()
    {
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                if (field[i, j].Type == -1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("*");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (field[i, j].Type == 0) Console.Write(".");
                else Console.Write(field[i, j].Type);
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}