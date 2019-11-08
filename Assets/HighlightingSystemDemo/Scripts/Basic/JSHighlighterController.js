#pragma strict
// Windows Store Apps: You can’t access C# classes from JS scripts unless you check .NET Core Partially in Compilation Overrides in PlayerSettings
#if !UNITY_WSA
import HighlightingSystem;
#endif

class JSHighlighterController extends MonoBehaviour
{
	#if !UNITY_WSA
	protected var h : HighlightingSystem.Highlighter;

	// 
	function Awake()
	{
		h = GetComponent(HighlightingSystem.Highlighter);
		if (h == null) { h = gameObject.AddComponent(HighlightingSystem.Highlighter); }
	}

	// 
	function Start()
	{
		h.FlashingOn(new Color(1f, 1f, 0f, 0f), new Color(1f, 1f, 0f, 1f), 2f);
	}
	#endif
}