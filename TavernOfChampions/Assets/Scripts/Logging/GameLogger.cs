using System.Collections.Generic;
using UnityEngine;

namespace TavernOfChampions.Logging
{
    public class GameLogger : MonoBehaviour
    {
        public static GameLogger Instance { get; private set; }

        [SerializeField] private LogTypeData[] _logTypeData;

        private readonly Dictionary<LoggerType, LogTypeData> _logTypeDict = new Dictionary<LoggerType, LogTypeData>();


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Warning("An instance of this Singleton already exists", LoggerType.GENERAL, this);

            DontDestroyOnLoad(this);
            LogTypeDataToDict();
        }

        public void Info(string message, LoggerType logType, Object context)
        {
            if (!_logTypeDict[logType].ShowLogs) return;

            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGBA(_logTypeDict[logType].PrefixColor)}>{_logTypeDict[logType].PrefixText}</color>: {message}", context);
        }

        public void Warning(object message, LoggerType logType, Object context)
        {
            if (!_logTypeDict[logType].ShowWarnings) return;

            Debug.LogWarning($"<color=#{ColorUtility.ToHtmlStringRGBA(_logTypeDict[logType].PrefixColor)}>{_logTypeDict[logType].PrefixText}</color>: {message}", context);
        }

        public void Error(object message, LoggerType logType, Object context)
        {
            Debug.LogError($"<color=#{ColorUtility.ToHtmlStringRGBA(_logTypeDict[logType].PrefixColor)}>{_logTypeDict[logType].PrefixText}</color>: {message}", context);
        }

        private void LogTypeDataToDict()
        {
            foreach (var logTypeData in _logTypeData)
                _logTypeDict[logTypeData.LogType] = logTypeData;
        }

        [RuntimeInitializeOnLoadMethod]
        void test()
        {
            print("jo");
        }
    }

    public enum LoggerType
    {
        GENERAL,
        NETWORK,
        GRID,
        CHAMPION,
    }

    [System.Serializable]
    struct LogTypeData
    {
        public LoggerType LogType;
        public bool ShowLogs;
        public bool ShowWarnings;
        public string PrefixText;
        public Color PrefixColor;
    }
}
