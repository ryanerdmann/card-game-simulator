using CardGameSimulator.V2;
using System.Diagnostics;

const int N = 100_000;
const bool DEBUG = false;

var stopwatch = Stopwatch.StartNew();

int won = 0;
for (var i = 0; i < N; i++)
{
    var game = new Game(new BasicStrategy(), DEBUG);

    var remaining = game.Play();
    if (remaining == 0) won++;
}

Console.WriteLine($"Simulated {N} times. Won {Math.Round(1.0 * won / N * 100, 2)}%");
Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");
