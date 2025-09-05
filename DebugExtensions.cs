// File: Assets/P3k/UnityLogger/Logger/UnityDebugExtensions.cs
// Purpose:
// - Provide a "success" call adjacent to UnityEngine.Debug with friendly call sites.
// - Use: using static UnityEngine.DebugExtensions;  LogSuccess("OK");

namespace UnityEngine
{
   using System.Linq;

   public static class DebugExtensions
   {
      public static void LogSuccess(object message)
      {
         Debug.Log("<color=#228E22>" + (message?.ToString() ?? string.Empty) + "</color>");
      }
   }
}