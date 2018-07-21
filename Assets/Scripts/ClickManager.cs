using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour {

    private MovingObject selectedUnit;
    private MovingObject defendingUnit;

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);


            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            // If the collider hits something, do something to it
            // If there's a unit selected and blocking object wasn't clicked, move the unit
            if(hit.collider != null) {

                // If it's a movingObject, select it.
                // Might need to look at the logic about checking out the selected unit for the game manager
                if(hit.collider.gameObject.GetComponent<MovingObject>() != null) {
                    selectedUnit = hit.collider.gameObject.GetComponent<MovingObject>();
                    //Check and make sure the Player owns the unit to be selected
                    if(selectedUnit.playerOwner == GameManager.instance.playerNumTurn) {
                        if (GameManager.instance.playerUnitSelected == null) {
                            if (selectedUnit.isSelected == false) {
                                selectedUnit.isSelected = true;
                                GameManager.instance.playerUnitSelected = selectedUnit;
                            }
                        } 
                    } else if (selectedUnit.isSelected == false && GameManager.instance.playerUnitSelected != null) {
                        GameManager.instance.defendingUnit = selectedUnit;
                    }
                }
            } else if (hit.collider == null && GameManager.instance.defendingUnit == null && selectedUnit != null && selectedUnit.isSelected == true) {
                if(!selectedUnit.hasMoved) {
                    selectedUnit.GridMove(mousePos);
                }
                selectedUnit.isSelected = false;
                selectedUnit = null;
                GameManager.instance.playerUnitSelected = null;
            }
        }
    }
}
