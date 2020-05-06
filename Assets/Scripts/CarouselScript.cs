using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CarouselScript : MonoBehaviour {

	public Canvas parentCanvas;

	public float axisSlideDuration = 0.3f;
	public Text indexText;
	public bool infinite = false;

	private JoystickButtons[] joysticks = new JoystickButtons[4] {
		new JoystickButtons(1),
		new JoystickButtons(2),
		new JoystickButtons(3),
		new JoystickButtons(4)
	};

	bool isPressed;
	bool scrolling;
	bool isSnapping;
	Coroutine snapper;

	private EventSystem es;

	private RectTransform contentRect;
	private RectTransform wrapperRect;

	private bool shouldAnimate;

	[HideInInspector] public Transform selectedItem;
	[HideInInspector] public System.Action<int> OnItemClick;
	[HideInInspector] public List<Transform> slides;

	void Start() {

		// Defaults
		scrolling = false;

		// Transforms
		wrapperRect = GetComponent<RectTransform>();
		contentRect = transform.Find("Viewport").Find("Content").GetComponent<RectTransform>();
		foreach (Transform child in contentRect) {
			slides.Add(child);
		}

		// Get current event system and null out
		es = EventSystem.current;

		// Move to pre-selected first item in event system
		shouldAnimate = false;
		MoveToSelected();
	}

	//
	// Listening for updates
	//

	void OnGUI() {

		// Don't listen for events if we're animating or disabled
		if (isSnapping || !es.isFocused) {
			return;
		}

		// Iterate over controllers
		for (int i = 0; i < joysticks.Length; i++) {
			JoystickButtons joystick = joysticks[i];

			if (Input.GetAxis(joystick.vertical) < 0) {
				Next();
			}
			if (Input.GetAxis(joystick.vertical) > 0) {
				Previous();
			}
		}
	}

	//
	// Managing selected state and passing to ES
	//

	void Select(int index) {
		// Save selected in event system
		selectedItem = contentRect.GetChild(index);
		if (es) es.SetSelectedGameObject(selectedItem.gameObject);
	}

	public void Next() {
		if (es.currentSelectedGameObject) {
			int index = es.currentSelectedGameObject.transform.GetSiblingIndex();

			// Return if we're already at the edge or already scrolling
			if (scrolling || (!infinite && index >= contentRect.childCount - 1)) {
				Debug.Log("YEAH");
				return;
			}

			// Iterate or flip to the other end of the list
			if (index >= contentRect.childCount - 1) {
				index = 0;
			} else {
				index++;
			}

			// Set new index in event system
			shouldAnimate = true;
			Select(index);
		}
	}

	public void Previous() {
		if (es.currentSelectedGameObject) {
			int index = es.currentSelectedGameObject.transform.GetSiblingIndex();

			// Return if we're already at the edge or already scrolling
			if (scrolling || (!infinite && index <= 0)) {
				return;
			}

			// Iterate or flip to the other end of the list
			if (index <= 0) {
				index = contentRect.childCount - 1;
			} else {
				index--;
			}

			// Set new index in event system
			shouldAnimate = true;
			Select(index);
		}
	}

	public void JumpTo(int index, bool animate) {
		if (scrolling) {
			return;
		}
		if (index < 0 || index > contentRect.childCount - 1) {
			Debug.LogError("Index Out of Bound!");
			return;
		}

		// Set target index and animation option
		shouldAnimate = animate;
		Select(index);
	}

	//
	// Animation between slides
	//

	public void MoveToSelected() {
		// Set animation options, reset property
		float duration = shouldAnimate ? axisSlideDuration : 0;
		scrolling = duration > 0;

		// Get selected
		// TODO - resolve race condition where this runs before es is loaded
		int index = es.currentSelectedGameObject ? es.currentSelectedGameObject.transform.GetSiblingIndex() : 0;
		selectedItem = contentRect.GetChild(index);

		// Update index text
		indexText.text = (index + 1).ToString() + "/" + contentRect.childCount.ToString();

		// Animate to destination
		snapper = StartCoroutine(
			Tween(
				contentRect,
				new Vector2(
					contentRect.localPosition.x,
					-(selectedItem.localPosition.y + (wrapperRect.rect.height / 2))
				),
				duration
			)
		);
	}

	IEnumerator Tween(RectTransform item, Vector2 destination, float duration) {
		isSnapping = true;
		int approxNoOfFrames = Mathf.RoundToInt(duration / Time.deltaTime);
		float posDiff = destination.y - item.localPosition.y;
		float eachFrameProgress = posDiff / approxNoOfFrames;

		for (int i = 0; i < approxNoOfFrames; i++) {
			yield return new WaitForEndOfFrame();
			item.localPosition = new Vector2(destination.x, item.localPosition.y + eachFrameProgress);
		}

		yield return new WaitForEndOfFrame();
		item.localPosition = destination;
		isSnapping = false;
		scrolling = false;
	}
}