using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMoveSelector : MonoBehaviour {

    public Sprite moveSelectSprite;
    public SpriteRenderer moveSelectRenderer;

	// Use this for initialization
	void Start () {
        moveSelectRenderer = GetComponent<SpriteRenderer>();
    }
}
