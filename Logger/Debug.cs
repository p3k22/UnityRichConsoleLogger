namespace P3k.UnityLogger
{
   using System;
   using System.Diagnostics;
   using System.Linq;
   using System.Runtime.CompilerServices;

   using UnityEngine;

   public static class Debug
   {
      private const string HEX_ASSERT = "E67E22";

      private const string HEX_ERROR = "F5B7B1";

      private const string HEX_EXCEPTION = "F5B7B1";

      private const string HEX_LOG = "BDC3C7";

      private const string HEX_SUCCESS = "A9DFBF";

      private const string HEX_WARNING = "FFF9D6";

      private const bool TRACE_ASSERT = false;

      private const bool TRACE_ERROR = false;

      private const bool TRACE_EXCEPTION = true;

      private const bool TRACE_LOG = false;

      private const bool TRACE_SUCCESS = false;

      private const bool TRACE_WARNING = false;

      [HideInCallstack]
      [MethodImpl(MethodImplOptions.NoInlining)]
      public static void Log(object message)
      {
         Write(LogType.Log, ToStringSafe(message), null, HEX_LOG, TRACE_LOG);
      }

      [HideInCallstack]
      [MethodImpl(MethodImplOptions.NoInlining)]
      public static void LogAssertion(object message)
      {
         Write(LogType.Assert, ToStringSafe(message), null, HEX_ASSERT, TRACE_ASSERT);
      }

      [HideInCallstack]
      [MethodImpl(MethodImplOptions.NoInlining)]
      public static void LogError(object message)
      {
         Write(LogType.Error, ToStringSafe(message), null, HEX_ERROR, TRACE_ERROR);
      }

      [HideInCallstack]
      [MethodImpl(MethodImplOptions.NoInlining)]
      public static void LogException(Exception exception)
      {
         var text = exception?.Message ?? "Exception";
         Write(LogType.Exception, text, exception, HEX_EXCEPTION, TRACE_EXCEPTION);
      }

      [HideInCallstack]
      [MethodImpl(MethodImplOptions.NoInlining)]
      public static void LogSuccess(object message)
      {
         Write(LogType.Log, ToStringSafe(message), null, HEX_SUCCESS, TRACE_SUCCESS);
      }

      [HideInCallstack]
      [MethodImpl(MethodImplOptions.NoInlining)]
      public static void LogWarning(object message)
      {
         Write(LogType.Warning, ToStringSafe(message), null, HEX_WARNING, TRACE_WARNING);
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      private static string ResolveCallerClassName()
      {
         // 0 = ResolveCallerClassName, 1 = Write, 2 = public Log*, 3 = caller
         var trace = new StackTrace(3, false);
         var frame = trace.GetFrame(0);
         var method = frame?.GetMethod();
         var type = method?.DeclaringType;
         if (type == null)
         {
            return "UnknownClass";
         }

         if (!type.IsGenericType)
         {
            return type.Name;
         }

         var name = type.Name;
         var tick = name.IndexOf('`');
         if (tick >= 0)
         {
            name = name[..tick];
         }

         var args = type.GetGenericArguments();
         var parts = new string[args.Length];
         for (var i = 0; i < args.Length; i++)
         {
            parts[i] = args[i].Name;
         }

         return $"{name}<{string.Join(",", parts)}>";
      }

      private static string ToStringSafe(object obj)
      {
         return obj == null ? "null" : obj.ToString();
      }

      [HideInCallstack]
      [MethodImpl(MethodImplOptions.NoInlining)]
      private static void Write(LogType type, string text, Exception exception, string colorHex, bool includeTrace)
      {
         var className = ResolveCallerClassName();
         var time = DateTime.Now;
         var frame = Time.frameCount;

         var stack = string.Empty;
         if (exception != null)
         {
            stack = exception.ToString();
         }
         else if (includeTrace)
         {
            stack = StackTraceUtility.ExtractStackTrace();
         }

         var t = colorHex == HEX_SUCCESS ? "Success" : type.ToString();
         var evt = new LogEvent(text, colorHex, t, className, time, frame, stack);

         var line = evt.ToString();

         switch (type)
         {
            case LogType.Warning:
               UnityEngine.Debug.LogWarning(line);
               break;

            case LogType.Error:
            case LogType.Assert:
            case LogType.Exception:
               if (!string.IsNullOrEmpty(evt.StackTrace))
               {
                  UnityEngine.Debug.LogError($"{line}\n{evt.StackTrace}");
               }
               else
               {
                  UnityEngine.Debug.LogError(line);
               }

               break;

            default:
               if (!string.IsNullOrEmpty(evt.StackTrace))
               {
                  UnityEngine.Debug.Log($"{line}\n{evt.StackTrace}");
               }
               else
               {
                  UnityEngine.Debug.Log(line);
               }

               break;
         }
      }
   }
}