using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MovingObject {

	// Use this for initialization
	protected override void Awake () {
        hp = 10;
        attackScore = 5;
        defenseScore = 0;
        attackRange = 1;
        moveRange = 3;

        base.Awake();
	}

    //Called when the object is disabled (between levels)
    //Not sure what I'll use it for
    private void OnDisable() {
       
    }

    protected override void AttemptMove<T>(int xDir, int yDir) {
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;
    }

    protected override void OnCantMove<T>(T component) {
        throw new System.NotImplementedException();
    }
}
