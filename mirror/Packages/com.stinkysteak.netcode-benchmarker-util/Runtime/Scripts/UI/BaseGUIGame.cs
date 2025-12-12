using StinkySteak.NetcodeBenchmark.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StinkySteak.NetcodeBenchmark
{
    public class BaseGUIGame : MonoBehaviour
    {
        [SerializeField] private Button _buttonStartServer;
        [SerializeField] private Button _buttonStartClient;

        [Space]
        [SerializeField] protected TMP_Text _textLatency;
        [SerializeField] private float _updateLatencyTextInterval = 1f;
        private SimulationTimer.SimulationTimer _timerUpdateLatencyText;

        [Header("Stress Test 1: Move Y")]
        [SerializeField] protected StressTestEssential _test_1;

        [Header("Stress Test 2: Move All Axis")]
        [SerializeField] protected StressTestEssential _test_2;

        [Header("Stress Test 3: Move Wander")]
        [SerializeField] protected StressTestEssential _test_3;

        [Header("Stress Test 4: Self Networked Move Wander")]
        [SerializeField] protected StressTestEssential _test_4;

        [System.Serializable]
        public struct StressTestEssential
        {
            public Button ButtonExecute;
            public int SpawnCount;
            public GameObject Prefab;
        }

        protected HeadlessServerProperty _headlessServerProperty;

        public struct HeadlessServerProperty
        {
            public SimulationTimer.SimulationTimer TimerActivateTest;
            public StressTestEssential Test;
        }

        private void Start()
        {
            MonoStart();
            Initialize();
        }


        protected virtual void Initialize()
        {
            _test_1.ButtonExecute.onClick.AddListener(StressTest_1);
            _test_2.ButtonExecute.onClick.AddListener(StressTest_2);
            _test_3.ButtonExecute.onClick.AddListener(StressTest_3);
            _test_4.ButtonExecute.onClick.AddListener(StressTest_4);

            _buttonStartServer.onClick.AddListener(StartServer);
            _buttonStartClient.onClick.AddListener(StartClient);
        }

        protected virtual void StartClient() { }
        protected virtual void StartServer() { }
        protected void StressTest_1() => StressTest(_test_1);
        protected void StressTest_2() => StressTest(_test_2);
        protected void StressTest_3() => StressTest(_test_3);
        protected void StressTest_4() => StressTest(_test_4);

        protected virtual void StressTest(StressTestEssential stressTest) { }


        private void Update()
        {
            MonoUpdate();
        }

        private void LateUpdate()
        {
            MonoLateUpdate();

            if (!_timerUpdateLatencyText.IsExpiredOrNotRunning()) return;

            UpdateNetworkStats();
            _timerUpdateLatencyText = SimulationTimer.SimulationTimer.CreateFromSeconds(_updateLatencyTextInterval);
        }

        protected virtual void MonoLateUpdate() { }
        protected virtual void MonoStart() { }
        protected virtual void MonoUpdate() { }
        protected virtual void UpdateNetworkStats() { }

        protected void RefigureHeadlessServerProperty()
        {
            bool isTest1 = HeadlessUtils.HasArg(HeadlessArguments.SERVER_TEST_1);
            bool isTest2 = HeadlessUtils.HasArg(HeadlessArguments.SERVER_TEST_2);
            bool isTest3 = HeadlessUtils.HasArg(HeadlessArguments.SERVER_TEST_3);
            bool isTest4 = HeadlessUtils.HasArg(HeadlessArguments.SERVER_TEST_4);

            float delaySpawnTest = 1f;

            _headlessServerProperty.TimerActivateTest = SimulationTimer.SimulationTimer.CreateFromSeconds(delaySpawnTest);

            if (isTest1)
            {
                _headlessServerProperty.Test = _test_1;
                return;
            }

            if (isTest2)
            {
                _headlessServerProperty.Test = _test_2;
                return;
            }

            if (isTest3)
            {
                _headlessServerProperty.Test = _test_3;
                return;
            }

            if (isTest4)
            {
                _headlessServerProperty.Test = _test_4;
                return;
            }
        }
    }
}