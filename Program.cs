using CardGameSimulator;
using CardGameSimulator.V2;

//
// Simulation parameters.
//

const int N = 1_000_000;
const bool DEBUG = false;
const bool PLOT = true;
const StrategyType STRATEGY = StrategyType.Basic;


//
// Run the simulation and report the win percentage.
//

var stopwatch = System.Diagnostics.Stopwatch.StartNew();
var remainingCards = PLOT ? new double[N] : null;

Console.WriteLine("Running simulation...");

int won = 0;
Parallel.For(0, N, i =>
{
    var game = new Game(StrategyFactory.Create(STRATEGY), DEBUG);

    var remaining = game.Play();
    if (remaining == 0) Interlocked.Increment(ref won);

    if (remainingCards != null) remainingCards[i] = remaining;
});

var wonPercent = Math.Round(1.0 * won / N * 100, 2);

Console.WriteLine($"Simulated {N} times. Won {wonPercent}%");
Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");


//
// Render a histogram with the remaining card distribution.
//

if (PLOT)
{
    Console.WriteLine("Saving plot...");

    // Create a histogram.
    var (counts, edges) = ScottPlot.Statistics.Common.Histogram(remainingCards, min: 0, max: 52, binSize: 1, density: true);

    // Render the histogram as a boxplot.
    var plot = new ScottPlot.Plot(600, 400);
    var positions = edges.Take(edges.Length - 1).ToArray();

    var bar = plot.AddBar(values: counts, positions: positions);
    bar.BarWidth = 1;

    // Customize the plot.
    plot.Title($"Remaining Cards: {STRATEGY} Strategy (Won={wonPercent}%) (N={N.ToKMB()}) ");
    plot.YAxis.Label("Probability");
    plot.XAxis.Label("Cards Remaining");
    plot.SetAxisLimits(yMin: 0);

    var filename = $"{STRATEGY}_Histogram.png";

    plot.SaveFig(filename);

    Console.WriteLine($"Saved to '{filename}'");
    System.Diagnostics.Process.Start("explorer.exe", filename);
}
