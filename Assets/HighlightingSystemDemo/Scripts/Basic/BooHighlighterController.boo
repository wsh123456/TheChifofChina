import UnityEngine

class BooHighlighterController (MonoBehaviour): 
	
	ifdef not UNITY_WSA:
		protected h as HighlightingSystem.Highlighter

	// 
	def Awake():
		ifdef not UNITY_WSA:
			h = (gameObject.GetComponent[of HighlightingSystem.Highlighter]() as HighlightingSystem.Highlighter);
			if h == null:
				h = (gameObject.AddComponent[of HighlightingSystem.Highlighter]() as HighlightingSystem.Highlighter);
	
	// 
	def Start():
		ifdef not UNITY_WSA:
			h.FlashingOn(Color(0F, 1F, 0F, 0F), Color(0F, 1F, 0F, 1F), 1F);
