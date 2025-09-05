# P3k Unity Logger

A structured, allocation‑light logging utility for Unity. Produces consistent, color‑coded messages and accurate double‑click navigation to the **caller line** in your code.

---

## Installation

1. Copy the `Assets/P3k/UnityLogger/Logger/` folder into your project.
2. If your project uses Assembly Definitions (asmdef), ensure any gameplay asmdef references the logger asmdef (if you created one), or keep them in the same asmdef.
3. Add a global alias so your code calls this logger instead of `UnityEngine.Debug`:

   ```csharp
   // Assets/P3k/UnityLogger/GlobalUsings.cs
   global using Debug = P3k.UnityLogger.Debug;
   ```

   Remove this file to instantly restore Unity’s default `Debug`.

---

## Quick start

```csharp
using UnityEngine; // no special using needed thanks to global alias

public class PlayerController : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Game started");
        Debug.LogSuccess("Player initialized successfully");
        Debug.LogWarning("Low health detected");
        Debug.LogError("Player died");
        Debug.LogAssertion("Invariant violated");
        try { throw new System.InvalidOperationException("Bad state"); }
        catch (System.Exception ex) { Debug.LogException(ex); }
    }
}
```

---

## Double‑click navigation

* Double‑clicking a console entry jumps directly to the **caller** in your code (e.g., `PlayerController.cs:42`).

---

## LogEvent string representation

Each `LogEvent` implements `ToString()` to produce the structured prefix:

```csharp
public override string ToString()
{
    return $"[{ClassName}][{LogType}](T:{TimeLocal:HH:mm:ss} | F: {Frame}) {Text}";
}
```

This ensures every message has a consistent, parseable format showing the class name, log type, timestamp, frame, and the actual text.
