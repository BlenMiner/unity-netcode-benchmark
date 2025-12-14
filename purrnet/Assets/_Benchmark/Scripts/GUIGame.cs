using PurrNet;
using StinkySteak.NetcodeBenchmark;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StinkySteak.MirrorBenchmark
{
    public class GUIGame : BaseGUIGame
    {
        [SerializeField] private NetworkManager _networkManagerPrefab;
        private NetworkManager _networkManager;

        protected override void Initialize()
        {
            base.Initialize();
            _networkManager = Instantiate(_networkManagerPrefab);
        }

        protected override void StartClient()
        {
            _networkManager.StartClient();
        }

        protected override void StartServer()
        {
            _networkManager.StartServer();
        }

        protected override void StressTest(StressTestEssential stressTest)
        {
            for (int i = 0; i < stressTest.SpawnCount; i++)
            {
                Instantiate(stressTest.Prefab,
                    Random.insideUnitSphere * 10,
                    Quaternion.identity);
            }
        }

        protected override void UpdateNetworkStats()
        {
            if (_networkManager == null || _networkManager.tickModule == null) return;

            _textLatency.SetText("Latency: {0}ms", (float)_networkManager.tickModule.rtt * 1_000);
        }
    }
}
