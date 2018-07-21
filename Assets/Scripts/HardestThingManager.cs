using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardestThingManager : MonoBehaviour {

	public void DoSomething() {
        GameManager.instance.EndPlayerTurn();
    }
}
