// 25. «Майнинг Хрюкоинов». Василий занимается майнингом валюты Хрюкоин. Он
//     написал программу Хрюкоинист-1, которая за каждые 100000 итераций на ПК майнит
// 0,01 Хрюкоина. У Васи есть доступ к 1, 2 или 4 ПК (= потокам, threads). Посчитайте
//     время майнинга 1 Хрюкоина в каждом из 3 случаев. Дайте Василию прогноз, за
// сколько дней (или лет) он сможет стать Хрюкоин-миллионером.

using System.Diagnostics;

namespace Lab1;

internal class Program
{
    internal static readonly decimal Award = new (0.01);

    internal static readonly decimal TotalAmount = 1000000;

    static void Main(string[] args)
    {
        var results = new List<Report>()
        {
            new Report() {NumberThreads = 1},
            new Report() {NumberThreads = 2},
            new Report() {NumberThreads = 4},
            new Report() {NumberThreads = 8},
            new Report() {NumberThreads = 20},
        };
        
        results.ForEach(Calculations);
        
        results.ForEach((x) => { Console.WriteLine(x.ToString()); });
    }

    private static void Calculations(Report report)
    {
        var watch = new Stopwatch();
        var lockObject = new object();
        
        watch.Start();
        
        Parallel.For(
            fromInclusive: 0,
            toExclusive: report.NumberAwards, 
            parallelOptions: new ParallelOptions(){ MaxDegreeOfParallelism = report.NumberThreads}, 
            body:(i) =>
            {
                var result = GetReward();
                lock (lockObject) report.Amount += result;
            });
        
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

        public override string ToString()
        {
            return $"Количество потоков - {NumberThreads} Время выполнения - {ResultTime.ToString(@"hh\:mm\:ss\:fff")} Полученная сумма - {Amount}";
        }
    }
}