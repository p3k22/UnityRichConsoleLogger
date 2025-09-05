// File: Assets/P3k/UnityLogger/Logger/LogEvent.cs
// Purpose: Immutable data structure representing a single log occurrence.
// Behavior: Normalizes ColorHex, never returns null strings, ToString yields the prefixed line.

namespace P3k.UnityLogger
{
   using System;
   using System.Linq;

   public sealed class LogEvent
   {
      public LogEvent(
         string text,
         string colorHex,
         string logType,
         string className,
         DateTime timeLocal,
         int frame,
         string stackTrace)
      {
         Text = text ?? string.Empty;
         ColorHex = string.IsNullOrWhiteSpace(colorHex) ? "FFFFFF" : colorHex.Trim().TrimStart('#');
         LogType = logType;
         ClassName = string.IsNullOrEmpty(className) ? "UnknownClass" : className;
         TimeLocal = timeLocal;
         Frame = frame;
         StackTrace = stackTrace ?? string.Empty;
      }

      public string ColorHex { get; }

      public string StackTrace { get; }

      private string ClassName { get; }

      private int Frame { get; }

      private string LogType { get; }

      private string Text { get; }

      private DateTime TimeLocal { get; }

      public override string ToString()
      {
         return $"<color=#{ColorHex}>[{ClassName}] [{LogType}] (T: {TimeLocal:HH:mm:ss} | F: {Frame}) - {Text}</color>";
      }
   }
}