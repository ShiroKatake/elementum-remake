using UnityEngine;

public class AbilitySlot : MonoBehaviour {
	public bool occupied;
	public GameObject element;
	public Color color;

	public Element Element
	{
		get
		{
			return element.GetComponent<Air>();
		}
	}

	void Update() {
		if (!occupied) {
			color = Color.white;
		} else {
			color = Element.Color;
		}
		transform.GetComponent<SpriteRenderer>().color = color;
	}

	public void Activate(PlayerMovement player)
	{
		Element.Activate(player);
		element = null;
	}
}
