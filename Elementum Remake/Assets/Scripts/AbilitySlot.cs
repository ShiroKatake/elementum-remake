using UnityEngine;

public class AbilitySlot : MonoBehaviour {
	public bool occupied;
	public GameObject element;
	public Color color;
	public bool active;

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

	public bool Occupied
	{
		get
		{
			return occupied;
		}
	}

	public void Awake()
	{
		active = true;
	}

	void Update() {

		//Change the color of the slot depending on what element is occupying it
		if (!occupied) {
			color = Color.white;
		} else {
			color = Element.Color;
		}
		transform.GetComponent<SpriteRenderer>().color = color;
	}


	//Activates the element in the slot, then clears slot
	public void Activate(PlayerMovement player)
	{
		if (occupied)
		{
			Element.Activate(player);
			element = null;
			occupied = false;
		}
	}

	//Places element into slot
	public void SetElement(GameObject e)
	{
		if (e != null)
		{
			element = e;
			occupied = true;
		}
	}
}
