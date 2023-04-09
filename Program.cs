// Сапёр
Random rnd = new Random();
int x = rnd.Next(5, 16), y = rnd.Next(5, 16);
Sapper game = new Sapper(y, x);

game.PrintField();

//while(true)
//{
//    Console.WriteLine($"Сапёр\n\n1. Ввести размеры поля вручную\n2. Оставить как есть ({}{})");
//    Console.WriteLine("Введите высоту поля -> ");
//    Console.WriteLine("Введите длину поля по X -> ");
//}

interface ISapper
{
    void GenerateMines(int quantity);
    void GenerateNums();

    void PrintField();
    int CheckBox(int x, int y);
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

    int y, x, mines = 0, flags = 0;
    Dot[,] field; // i = y, j = x 

    public Sapper(int y, int x)
    {
        if (y < 1 || x < 1) throw new Exception("Вам нужна хотя-бы одна клетка для игры!");
        this.y = y;
        this.x = x;

        field = new Dot[y, x];

        for (int i = 0; i < y; i++)
            for (int j = 0; j < x; j++)
                field[i, j] = new Dot();

        if(y*x > 50) GenerateMines(rnd.Next(9, 15));
        if(y*x > 75) GenerateMines(rnd.Next(16, 25));
        if (y * x > 150) GenerateMines(rnd.Next(26, 32));
        else GenerateMines(rnd.Next(6, 16));

        GenerateNums();
    }

    public int CheckBox(int x, int y)
    {
        int mines = 0;

        if(y - 1 >= 0)
        {
            if (field[y - 1, x].Type == -1) mines++;
            if (x + 1 < this.x && field[y - 1, x + 1].Type == -1) mines++;
            if(x - 1 >= 0 && field[y - 1, x - 1].Type == -1) mines++;
        }
        if(y + 1 < this.y)
        {
            if (field[y + 1, x].Type == -1) mines++;
            if (x + 1 < this.x && field[y + 1, x + 1].Type == -1) mines++;
            if (x - 1 >= 0 && field[y + 1, x - 1].Type == -1) mines++;
        }
        if (x + 1 < this.x && field[y, x + 1].Type == -1) mines++;
        if (x - 1 >= 0 && field[y, x - 1].Type == -1) mines++;

        return mines;
    }

    public void GenerateNums()
    {
        for (int i = 0; i < y; i++)
            for (int j = 0; j < x; j++)
                if (field[i, j].Type == 0) field[i, j].Type = CheckBox(j, i);
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
                if (field[i, j].Type == -1) Console.Write("*");
                else if (field[i, j].Type == 0) Console.Write(".");
                else
                {                    
                    if (field[i, j].Type == 1) Console.ForegroundColor = ConsoleColor.Blue;
                    else if (field[i, j].Type == 2) Console.ForegroundColor = ConsoleColor.Green;
                    else if (field[i, j].Type == 3) Console.ForegroundColor = ConsoleColor.DarkRed;
                    else if (field[i, j].Type == 4) Console.ForegroundColor = ConsoleColor.DarkBlue;
                    else if (field[i, j].Type == 5) Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    else if (field[i, j].Type == 6) Console.ForegroundColor = ConsoleColor.Cyan;
                    else if (field[i, j].Type == 7) Console.ForegroundColor = ConsoleColor.DarkGray;
                    else if (field[i, j].Type == 8) Console.ForegroundColor = ConsoleColor.White;
                    
                    Console.Write(field[i, j].Type);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}