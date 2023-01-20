using System.Globalization;
using UnityEngine;
using System.ComponentModel;

public class InputLogger : Logger
{
        [Header("Logger Specific Settings")] // Create a header for any logger specific settings
        [SerializeField] public string fileNamePrefix = "GO";

        [HideInInspector] public string className = "";
        [HideInInspector] public LogType type;
        [HideInInspector] public string message = "";


        public void Start()
        {
            this.reportHeaders = new string[] {"Class","Type","Message"};
            Initialize(); 
            StartRecording();
    
        }

        public void LogData(Object o, LogType logType, string msg)
        {
            className = TypeDescriptor.GetClassName(o);
            type = logType;
            message = msg;
            Write();
        }
        public string[] GetData()
        {
            string[] strings = new string[3] {
                className,
                type.ToString(CultureInfo.InvariantCulture),
                message
            };
            return strings;
        }

         void OnApplicationQuit()
        {
            StopRecording();
        }
        void OnDisable()
        {
            StopRecording();
        }
        void OnDestroy()
        {
            StopRecording();
        }
        

}
