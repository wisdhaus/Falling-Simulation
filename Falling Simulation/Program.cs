Console.ForegroundColor = ConsoleColor.Cyan;

int hight;
Console.Write("Enter hight: ");
hight = Convert.ToInt16(Console.ReadLine());

int initial_y;
Console.Write("Enter initial Y position: ");
initial_y = Convert.ToInt16(Console.ReadLine());

double scale;
Console.Write("Enter scale: ");
scale = Convert.ToDouble(Console.ReadLine());

double losses;
Console.Write("Enter losses: ");
losses = Convert.ToDouble(Console.ReadLine());

int initial_speed;
Console.Write("Enter initial speed: ");
initial_speed = Convert.ToInt16(Console.ReadLine());

int mass;
Console.Write("Enter mass: ");
mass = Convert.ToInt16(Console.ReadLine());

double acceleration_of_gravity;
Console.Write("Enter acceleration of gravity: ");
acceleration_of_gravity = Convert.ToDouble(Console.ReadLine());

double accuracy;
Console.Write("Enter accuracy: ");
accuracy = Convert.ToInt16(Console.ReadLine());

void Update()
{
    Thread.Sleep(5);
    Console.Clear();
}

double timer = -1;

Physical_characteristics Physics(double delta_time, Physical_characteristics block)
{
    block.Additional_losses = 0.1 * block.Mass;
    block.Speed += acceleration_of_gravity * delta_time / 2;
    block.Y += block.Speed * delta_time;
    if (block.Y <= 0 && block.Speed < 0)
    {
        block.E_kinetic = block.E_kinetic * block.Losses - block.Additional_losses;
        if (block.E_kinetic < 0)
        {
            block.E_kinetic = 0;
        }
        block.Speed = Math.Sqrt(2 * block.E_kinetic / block.Mass);
        block.Y = 0;
    }
    if (block.Y >= hight && block.Speed > 0)
    {
        block.E_kinetic = block.E_kinetic * block.Losses - block.Additional_losses;
        if (block.E_kinetic < 0)
        {
            block.E_kinetic = 0;
        }
        block.Speed = -Math.Sqrt(2 * block.E_kinetic / block.Mass);
        block.Y = hight;
    }
    block.E_kinetic = block.Mass * Math.Pow(block.Speed, 2) / 2;
    block.E_potential = -(block.Mass * acceleration_of_gravity * block.Y);
    block.Full_energy = block.E_kinetic + block.E_potential;
    timer = timer + (1 / accuracy);
    return block;
}

void Data(Physical_characteristics block)
{
    Console.WriteLine(
        $"Speed: {Math.Round(block.Speed, 1)}\n"
            + $"Y_position: {Math.Round(block.Y, 1)}\n"
            + $"E_kinetic: {Math.Round(block.E_kinetic, 1)}\n"
            + $"E_potential: {Math.Round(block.E_potential, 1)}\n"
            + $"Timer: {Math.Round(timer, 0)}"
    );
}

string[] Lines = new string[(int)(Math.Round(scale * hight, 0)) + 1];
const string BLOCK = "\u2588";
const string UPPER_HALF_OF_BLOCK = "\u2580";
const string LOWER_HALF_OF_BLOCK = "\u2584";
double scaled_y;

void Graphics(Physical_characteristics block)
{
    scaled_y = scale * block.Y;
    for (int i = 0; i < Lines.Length; i++)
    {
        Lines[i] = $"{i}";
    }
    if (scaled_y < Lines.Length - 1)
    {
        if (scaled_y - Math.Floor(scaled_y) >= 0 && scaled_y - Math.Floor(scaled_y) < 0.5)
        {
            Lines[(int)(Math.Round(scaled_y, 0))] = BLOCK;
        }
        if (scaled_y - Math.Floor(scaled_y) >= 0.5 && scaled_y - Math.Floor(scaled_y) < 1)
        {
            Lines[(int)(Math.Floor(scaled_y))] = UPPER_HALF_OF_BLOCK;
            Lines[(int)(Math.Round(scaled_y, 0))] = LOWER_HALF_OF_BLOCK;
        }
    }
    for (int i = 0; i < Lines.Length; i++)
    {
        Console.WriteLine(Lines[Lines.Length - i - 1]);
    }
}

var block = new Physical_characteristics
{
    Mass = 1,
    Speed = initial_speed,
    Y = initial_y,
    Losses = losses,
    Additional_losses = 0,
    E_kinetic = 0,
    E_potential = 0,
    Full_energy = 0,
};

const double DELTA_T = 0.05;

do
{
    Update();
    for (int i = 0; i < accuracy; i++)
    {
        block = Physics(DELTA_T / accuracy, block);
    }
    Data(block);
    Graphics(block);
} while (block.Full_energy > 0);
while (Console.ReadKey().Key != ConsoleKey.Enter) { }

struct Physical_characteristics
{
    public int Mass;
    public double Speed;
    public double Y;
    public double Losses;
    public double E_kinetic;
    public double E_potential;
    public double Full_energy;
    public double Additional_losses;
}
