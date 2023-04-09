// Сапёр
using System.Reflection.Emit;

Random rnd = new Random();
int x = rnd.Next(5, 16), y = rnd.Next(5, 16);
Sapper game = new Sapper(y, x);

game.Start();

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
    //void InputHandler(string key);
    void CheckEmptyCells(int y, int x, bool IsNum);

    void PrintField();
    int CheckBox(int x, int y);
}

class Dot
{
    public int Type { get; set; } // -2(#) - флаг / -1(*) - мина / 0(.) - пустая клетка / 123456789 цифры
    public bool Show { get; set; }
    public bool Checked { get; set; }
    public Dot() { Type = 0; Checked = false; }
}

class Sapper : ISapper
{
    Random rnd = new Random();

    int y, x, mines = 0, flags = 0;
    int[] mousePTR;
    Dot[,] field; // i = y, j = x 

    public Sapper(int y, int x)
    {
        if (y < 1 || x < 1) throw new Exception("Вам нужна хотя-бы одна клетка для игры!");
        this.y = y;
        this.x = x;

        mousePTR = new int[2]{ y / 2, x / 2 };

        field = new Dot[y, x];

        for (int i = 0; i < y; i++)
            for (int j = 0; j < x; j++)
                field[i, j] = new Dot();

        if(y*x > 50) GenerateMines(rnd.Next(7, 13));
        else if(y*x > 75) GenerateMines(rnd.Next(10, 15));
        else if (y * x > 150) GenerateMines(rnd.Next(15, 25));
        else GenerateMines(rnd.Next(4, 9));

        GenerateNums();
    }

    public void Start()
    {
        bool lose = false;
        bool win = false;

        while(true)
        {
            if(lose)
            {
                Console.WriteLine("Вы взорвались!!!");
                break;
            }

            PrintField();
            Console.WriteLine($"Осталось мин: {mines - flags}\n");
            Console.WriteLine("Управление:\nСтрелочки на клавиатуре - двигать курсор\nENTER - Поставить флаг\nSPACE - Вскрыть клетку\nBACKSPACE - Убрать флаг"); ;

            ConsoleKeyInfo ch = Console.ReadKey(true);
            int code = ch.GetHashCode();

            if (Key(code) == "right") { if (mousePTR[1] < x-1) mousePTR[1]++; Console.Beep(200, 100); }
            else if (Key(code) == "left") { if (mousePTR[1] > 0) mousePTR[1]--; Console.Beep(250, 100); }
            else if (Key(code) == "up") { if (mousePTR[0] > 0) mousePTR[0]--; Console.Beep(300, 100); }
            else if (Key(code) == "down") { if (mousePTR[0] < y-1) mousePTR[0]++; Console.Beep(350, 100); }
            else if(Key(code) == "space")
            {
                if (field[mousePTR[0], mousePTR[1]].Type == -1)
                {
                    lose = true;
                    Console.Beep(450, 250);
                    Console.Beep(250, 250);
                    Console.Beep(200, 250);
                }
                else if (field[mousePTR[0], mousePTR[1]].Type > 0 && !field[mousePTR[0], mousePTR[1]].Show)
                {
                    Console.Beep(450, 250);
                    Console.Beep(550, 50);
                    Console.Beep(650, 50);
                    field[mousePTR[0], mousePTR[1]].Show = true;
                }
                else
                {
                    Console.Beep(650, 250);
                    Console.Beep(750, 50);
                    Console.Beep(850, 50);
                    CheckEmptyCells(mousePTR[0], mousePTR[1], false);
                }
            }
            
            Console.Clear();
        }        
    }

    public string Key(int code)
    {
        if (code == 2490368) return "up";
        else if (code == 2424832) return "left";
        else if (code == 2621440) return "down";
        else if (code == 2555904) return "right";

        else if (code == 3211313) return "1";
        else if (code == 3276850) return "2";
        else if (code == 3342387) return "3";
        else if (code == 3407924) return "4";
        else if (code == 3473461) return "5";
        else if (code == 3538998) return "6";
        else if (code == 3604535) return "7";
        else if (code == 3670072) return "8";
        else if (code == 3735609) return "9";
        else if (code == 3145776) return "0";

        else if (code == 851981) return "enter";
        else if (code == 2097184) return "space";
        else if (code == 524296) return "backspace";

        return "";
    }   

    public void CheckEmptyCells(int y, int x, bool IsNum)
    {
        if (field[y, x].Checked) return;

        if (CheckBox(x, y) == 0 || field[y, x].Type > 0 && field[y, x].Show == false)
        {
            field[y, x].Show = true;
            field[y, x].Checked = true;

            if (x + 1 < this.x)
            {
                if (field[y, x].Type > 0) return;
                if (field[y, x + 1].Type >= 0 && !IsNum)
                {
                    if(field[y, x].Type > 0) CheckEmptyCells(y, x + 1, true);
                    else if (field[y, x].Type == 0) CheckEmptyCells(y, x + 1, false);
                }                
            }
            if (x - 1 >= 0)
            {
                if (field[y, x].Type > 0) return;
                if (field[y, x - 1].Type >= 0 && !IsNum)
                {
                    if(field[y, x].Type > 0) CheckEmptyCells(y, x - 1, true);
                    else if (field[y, x].Type == 0) CheckEmptyCells(y, x - 1, false);
                }                
            }
            
            if (x - 1 >= 0 && y - 1 >= 0)
            {
                if (field[y, x].Type > 0) return;
                if (field[y - 1, x - 1].Type >= 0 && !IsNum)
                {
                    if(field[y, x].Type > 0) CheckEmptyCells(y - 1, x - 1, true);
                    else if (field[y, x].Type == 0) CheckEmptyCells(y - 1, x - 1, false);
                }                
            }
            if (x - 1 >= 0 && y + 1 < this.y)
            {
                if (field[y, x].Type > 0) return;
                if (field[y + 1, x - 1].Type >= 0 && !IsNum)
                {
                    if(field[y, x].Type > 0) CheckEmptyCells(y + 1, x - 1, true);
                    else if (field[y, x].Type == 0) CheckEmptyCells(y + 1, x - 1, false);
                }                
            }

            if (y + 1 < this.y)
            {
                if (field[y, x].Type > 0) return;
                if (field[y + 1, x].Type >= 0 && !IsNum)
                {
                    if(field[y, x].Type > 0) CheckEmptyCells(y + 1, x, true);
                    else if (field[y, x].Type == 0) CheckEmptyCells(y + 1, x, false);
                }                
            }
            if (y - 1 >= 0)
            {
                if (field[y, x].Type > 0) return;
                if (field[y - 1, x].Type >= 0 && !IsNum)
                {
                    if(field[y, x].Type > 0) CheckEmptyCells(y - 1, x, true);
                    else if (field[y, x].Type == 0) CheckEmptyCells(y - 1, x, false);
                }                
            }
            
            if (y - 1 >= 0 && x - 1 >= 0)
            {
                if (field[y, x].Type > 0) return;
                if (field[y - 1, x - 1].Type >= 0 && !IsNum)
                {
                    if(field[y, x].Type > 0) CheckEmptyCells(y - 1, x - 1, true);
                    else if (field[y, x].Type == 0) CheckEmptyCells(y - 1, x - 1, false);
                }                
            }
            if (y - 1 >= 0 && x + 1 < this.x)
            {
                if (field[y, x].Type > 0) return;
                if (field[y - 1, x + 1].Type >= 0 && !IsNum)
                {
                    if(field[y, x].Type > 0) CheckEmptyCells(y - 1, x + 1, true);
                    else if (field[y, x].Type == 0) CheckEmptyCells(y - 1, x + 1, false);
                }                
            }
            //if (x - 1 > 0)
            //{
            //    if (field[y, x].Type > 0) return;
            //    if (field[y, x - 1].Type >= 0 && !IsNum)
            //    {
            //        if(field[y, x].Type > 0) CheckEmptyCells(y, x - 1, true);
            //        else if (field[y, x].Type == 0) CheckEmptyCells(y, x - 1, false);
            //    }                
            //}
        }
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
                if(i == mousePTR[0] && j == mousePTR[1]) { Console.BackgroundColor = ConsoleColor.DarkGreen; }
                if (field[i, j].Show == true)
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
                }
                else Console.Write("O");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}