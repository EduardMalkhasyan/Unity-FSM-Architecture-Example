using Sirenix.OdinInspector;
using UnityEngine;

namespace BugiGames.Tools
{
    public class BenchmarkExample : MonoBehaviour
    {
        [SerializeField] private int iterationCount = 10000000;

        [Button]
        private void RunBenchmarkTest()
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            float number = 5;

            stopwatch.Start();

            for (int i = 0; i < iterationCount; i++)
            {
                number++;
            }

            stopwatch.Stop();
            Debug.Log(stopwatch.Elapsed);
        }
    }
}
