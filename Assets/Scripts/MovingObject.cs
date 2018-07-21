using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MovingObject : MonoBehaviour {

    public float moveTime = 0.1f;

    public LayerMask blockingLayer;

    public bool isSelected = false;
    public bool hasMoved = false;
    public bool hasAttacked = false;
    //public bool hasActioned for everything else?

    public int playerOwner;

    public Sprite player2Sprite;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    public GridMoveSelector singleMoveGrid;
    public GridMoveSelector[] moveGrid;

    //These are usually set by the inheriting class
    public int hp = 10;
    public int attackScore = 5;
    public int defenseScore = 0;
    public int attackRange = 1;
    public int moveRange = 3;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

	// Use this for initialization. Protected Virtual can be overwritten by inheriting classes
	protected virtual void Awake () {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player2Sprite = GetComponent<Sprite>();

        inverseMoveTime = 1f / moveTime;
	}

    private void Start() {
        //if(playerOwner == 2) {
        //    spriteRenderer.sprite = player2Sprite;
        //    spriteRenderer.gameObject.SetActive(true);
        //}
        moveGrid = new GridMoveSelector[100];
        for (int i = 0; i < moveGrid.Length; i++) {
            moveGrid[i] = Instantiate(singleMoveGrid, new Vector3(0f, 0f, 0f), Quaternion.identity) as GridMoveSelector;
        }
    }

    public void resetOnNewTurn() {
        isSelected = false;
        hasMoved = false;
        hasAttacked = false;
    }

    void Update() {
        if (isSelected) {
            ShowMoveGrid();
        } else {
            HideMoveGrid();
        }
        CheckIfDead();
    }

    //Shows a grid of where the unit can move
    void ShowMoveGrid() {
        int count = 0;
        for (int y = -moveRange; y <= moveRange; y++) {
            for (int x = -moveRange; x <= moveRange; x++) {
                int deltaPos = Mathf.Abs(x) + Mathf.Abs(y);
                if (deltaPos <= moveRange && deltaPos != 0) {
                    moveGrid[count].gameObject.SetActive(true);
                    moveGrid[count].transform.position = new Vector3(transform.position.x + (float)x, transform.position.y + (float)y, 0f);
                    count++;
                }
            }
        }
    }

    void HideMoveGrid() {
        foreach(GridMoveSelector moveSelector in moveGrid) {
            moveSelector.gameObject.SetActive(false);
        }
    }


    //out = pass by reference
    protected bool Move (int xDir, int yDir, out RaycastHit2D hit) {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if(hit.transform == null) {
            GridMove(end);
            return true;
        }

        return false;
    }

    protected virtual void AttemptMove<T> (int xDir, int yDir) where T : Component {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if(hit.transform == null) {
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();

        if(!canMove && hitComponent != null) {
            OnCantMove(hitComponent);
        }
    }

    // Moves to grid position under mouseclick.
    public IEnumerator GridMove (Vector3 end) {
        Vector2 endPos = new Vector2(Mathf.Floor(end.x), Mathf.Ceil(end.y));

        float deltaX = Mathf.Abs(endPos.x - transform.position.x);
        float deltaY = Mathf.Abs(endPos.y - transform.position.y);


        //Check if move is within range
        if ((deltaX + deltaY) <= moveRange) {
            rb2D.MovePosition(endPos);
            hasMoved = true;
        }

        return null;

        //float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //while (sqrRemainingDistance > float.Epsilon) {
        //    //Moves in a straight line from old position to new position. The times move the unit that much towards the destination.
        //    Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

        //    rb2D.MovePosition(newPosition);

        //    sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        //    yield return null;
        // }
    }

    public void recieveDamage(int incomingAtk) {
        hp -= incomingAtk;
        CheckIfDead();
    }

    private void CheckIfDead() {
        if (hp == 0) {
            //TODO: Play death animation? Kaboom?

            Destroy(gameObject);
        }
    }

    protected abstract void OnCantMove<T>(T component) where T : Component;
}
