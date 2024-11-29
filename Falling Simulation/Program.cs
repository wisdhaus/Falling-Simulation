Console.ForegroundColor = ConsoleColor.Cyan;

int acceleration_of_gravity = -10;
int hight = 30;
int initial_y = 30;
double y_position = initial_y;
const double DELTA_T = 0.05;
int initial_speed = 0;

double e_kinetic = 0;
double e_potential = 0;
double losses = 0.8;
double full_energy = 0;
double scale = 1;
double scaled_y;

string[] Array = new string[Convert.ToInt16(Math.Round(scale * hight, 0)) + 1];
string block = "\u2588";
string upper_half_of_block = "\u2580";
string lower_half_of_block = "\u2584";

int timer = -1;

void Enter()
{
    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
}
void Data()
{
    Console.WriteLine(
        $""
            + $"Hight: {Math.Round(y_position, 1)}\n"
            + $"E_kinetic: {Math.Round(e_kinetic, 1)}\n"
            + $"E_potential: {Math.Round(e_potential, 1)}\n"
            + $"Initial Hight: {initial_y}\n"
            + $"Initial Speed: {initial_speed}\n"
            + $"Scale: {scale} lines/meters\n"
            + $"Timer: {timer}"
    );
}
void Update()
{
    Thread.Sleep(5);
    Console.Clear();
}
Object Physics(double delta_time, Object ball)
{
    double additional_losses = 0.1 * ball.Mass;
    ball.Speed += acceleration_of_gravity * delta_time / 2;
    y_position += ball.Speed * delta_time;
    if (y_position <= 0 && ball.Speed < 0)
    {
        e_kinetic = e_kinetic * losses - additional_losses;
        if (e_kinetic < 0)
        {
            e_kinetic = 0;
        }
        ball.Speed = Math.Sqrt(2 * e_kinetic / ball.Mass);
        y_position = 0;
    }
    if (y_position >= hight && ball.Speed > 0)
    {
        e_kinetic = e_kinetic * losses - additional_losses;
        if (e_kinetic < 0)
        {
            e_kinetic = 0;
        }
        ball.Speed = -Math.Sqrt(2 * e_kinetic / ball.Mass);
        y_position = hight;
    }
    e_kinetic = ball.Mass * Math.Pow(ball.Speed, 2) / 2;
    e_potential = -(ball.Mass * acceleration_of_gravity * y_position);
    full_energy = e_kinetic + e_potential;
    timer++;
    return ball;
}
void Graphics()
{
    scaled_y = scale * y_position;
    for (int i = 0; i < Array.Length; i++)
    {
        Array[i] = $"{i}";
    }
    if (scaled_y < Array.Length - 1)
    {
        if (scaled_y - Math.Floor(scaled_y) >= 0 && scaled_y - Math.Floor(scaled_y) < 0.5)
        {
            Array[Convert.ToInt32(Math.Round(scaled_y, 0))] = block;
        }
        if (scaled_y - Math.Floor(scaled_y) >= 0.5 && scaled_y - Math.Floor(scaled_y) < 1)
        {
            Array[Convert.ToInt32(Math.Floor(scaled_y))] = upper_half_of_block;
            Array[Convert.ToInt32(Math.Round(scaled_y, 0))] = lower_half_of_block;
        }
    }
    for (int i = 0; i < Array.Length; i++)
    {
        Console.WriteLine(Array[Array.Length - i - 1]);
    }
}
Enter();
var ball = new Object { Mass = 1, Speed = 0 };
do
{
    Update();
    for (int i = 0; i < 5; i++)
    {
        ball = Physics(DELTA_T / 5, ball);
    }
    Data();
    Graphics();
} while (full_energy > 0);
Enter();

struct Object
{
    public int Mass;
    public double Speed;
}
