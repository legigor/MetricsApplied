using System;
using System.Threading.Tasks;

namespace MetricsDemo
{
    static class Google
    {
        public static async Task Ping()
        {
            var call = Rnd(0, 100);

            if (call > 95)
            {
                await Task.Delay(1600);
                throw new InvalidOperationException("Timeout");
            }

            if (call > 90)
                await Task.Delay(Rnd(500, 1500));
            else
                await Task.Delay(Rnd(200, 700));
        }

        static int Rnd(int minValue, int maxValue)
        {
            var rnd = new Random();
            return rnd.Next(minValue, maxValue);
        }
    }
}