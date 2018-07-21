using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturableStruct : MonoBehaviour {

    public Sprite player1Sprite;
    public Sprite player2Sprite;
    public int playerOwner;
    public int hp;

    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(playerOwner < 0) {
            hp = 10;
        } else {
            hp = 20;
        }

        if(playerOwner == 2) {
            spriteRenderer.sprite = player2Sprite;
        }
	}
	
	public void CaptureStruct (int loss) {
        hp -= loss;

        if(hp <= 0) {
            if(playerOwner == 1) {
                playerOwner = 2;
                spriteRenderer.sprite = player2Sprite;
            } else {
                playerOwner = 1;
                spriteRenderer.sprite = player1Sprite;
            }
        }
    }
}
