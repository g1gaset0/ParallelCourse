using System.Diagnostics;
using ThreadState = System.Threading.ThreadState;

namespace Lab1;

internal class Program
{
    internal static readonly decimal Award = new(0.01);

    internal static readonly decimal TotalAmount = 1000;

    static object lockObject = new();

    static void Main(string[] args)
    {
        var results = new List<Report>()
        {
            new Report() {NumberThreads = 1},
            new Report() {NumberThreads = 2},
            new Report() {NumberThreads = 4},
            new Report() {NumberThreads = 8},
            // new Report() {NumberThreads = 20}
        };
        
        results.ForEach(Calculations);
        
        results.ForEach((x) => { Console.WriteLine(x.ToString()); });
    }
    
    private static void Calculations(Report report)
    {
        var watch = new Stopwatch();
        var threads = new Thread[report.NumberThreads];

        watch.Start();
        for (int i = 0; i < report.NumberThreads; i++)
        {
            threads[i] = new Thread(report.Operation);
            threads[i].Start();
        }

        while (threads.All(x => x.ThreadState != ThreadState.Stopped))
        {
        }
        
        watch.Stop();
        report.ResultTime = watch.Elapsed;
    }
    
    private static decimal GetReward()
    {
        for (int i = 0; i < 100000; i++)
        {
            i = i;
        }

        return Award;
    }

    internal class Report
    {
        internal int NumberThreads { get; set; }

        internal TimeSpan ResultTime { get; set; }

        internal decimal Amount { get; set; }

        internal int NumberAwards => (int) (TotalAmount / Award);
        
        internal void Operation()
        {
            while (true)
            {
                var result = GetReward();
                lock (lockObject)
                {
                    if (this.Amount == TotalAmount)
                        break;
                    this.Amount += result;
                }
            }
        }

        public override string ToString()
        {
            return
                $"Количество потоков - {NumberThreads} Время выполнения - {ResultTime.ToString(@"hh\:mm\:ss\:fff")} Полученная сумма - {Amount}";
        }
    }
}