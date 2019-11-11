using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler// required interface when using the OnPointerEnter method.
{
	Text ButtonText;
	Vector3 TextStartingPosition = new Vector3();
	Vector3 TextHoverPosition = new Vector3();
	Vector3 Destination = new Vector3();

	void Start()
	{
		ButtonText = GetComponentInChildren<Text>();
		TextStartingPosition = ButtonText.transform.localPosition;
		TextHoverPosition = new Vector3(TextStartingPosition.x + 20f, TextStartingPosition.y, TextStartingPosition.z);
		Destination = TextStartingPosition;
	}

	void Update()
	{
		ButtonText.transform.localPosition = Vector3.Lerp(ButtonText.transform.localPosition, Destination, 0.25f);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Destination = TextHoverPosition;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Destination = TextStartingPosition;
	}
}