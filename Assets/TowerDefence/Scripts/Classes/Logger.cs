
public class Logger {
        
    public enum LogType {Log, Error, Warning};

    private static string tempText = "";

    public static void Log (object text = null, bool endLine = true, LogType logType = LogType.Log) {
            
        if (LogSpecial (text, endLine, logType)) {

            return;
        }

        tempText += (text ?? "").ToString ();

        if (endLine) {

            switch (logType) {

                case LogType.Log:
                    
                    UnityEngine.Debug.Log (tempText);
                    break;

                case LogType.Warning:
                    
                    UnityEngine.Debug.LogWarning (tempText);
                    break;

                case LogType.Error:
                    
                    UnityEngine.Debug.LogError (tempText);
                    break;
            }

            tempText = "";
        }
    }

    public static void LogAll (params object [] data) {

        LogList (data);
    }

    
    private static bool LogSpecial (object text, bool endLine, LogType logType) {
        
        if (text is byte []) {
            
            LogByte (text as byte [], endLine, logType);
            return true;
        }

        if (text is System.Collections.IList) {
            
            LogList (text as System.Collections.IList, endLine, logType);
            return true;
        }

        if (text is System.Collections.ICollection) {
            
            LogCollection (text as System.Collections.ICollection, endLine, logType);
            return true;
        }

        return false;
    }

    public static void LogByte (byte [] array, bool endLine = true, LogType logType = LogType.Log) {

        Log ("byte [" + array.Length + "] (", false, logType);

        for (int i = 0; i < array.Length; i++) {

            Log ((char.IsLetterOrDigit ((char) array [i]) ? ((char) array [i]).ToString () : array [i].ToString ()), false, logType);
        }

        Log (")", endLine, logType);
    }

    public static void LogList (System.Collections.IList list, bool endLine = true, LogType logType = LogType.Log) {
        
        Log ("List [" + list.Count + "] (", false, logType);

        for (int i = 0; i < list.Count - 1; i++) {
            
            Log (list [i], false, logType);
            Log (", ", false, logType);
        }

        if (list.Count > 0) {
            
            Log (list [list.Count - 1], false, logType);
        }

        Log (")", endLine, logType);
    }

    public static void LogCollection (System.Collections.ICollection list, bool endLine = true, LogType logType = LogType.Log) {
        
        Log ("List [" + list.Count + "] (", false, logType);

        foreach (var a in list) {
            
            Log (a, false, logType);
            Log (", ", false, logType);
        }

        Log (")", endLine, logType);
    }
}
