using UnityEngine;

public class AbilitySlot : MonoBehaviour {
	public bool occupied;
	public GameObject element;
	public Color color;

	public Sprite Sprite
	{
		get
		{
			return Element.sprite;
		}
	}

	public Element Element
	{
		get
		{
			return element.GetComponent<Element>();
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
		if (occupied)
		{
			Element.Activate(player);
			element = null;
			occupied = false;
		}
	}

	public void SetElement(GameObject e)
	{
		element = e;
		occupied = true;
	}
}
