using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public BoardManager boardScript;
    public ClickManager clickScript;
    [HideInInspector] public int playerNumTurn = 1;
    public List<Player> players;
    public List<MovingObject> player1Units;
    public List<MovingObject> player2Units;
    public MovingObject playerUnitSelected = null;
    public MovingObject defendingUnit = null;

    private Text playerTurnText;
    private Text endGameText;

    private int level = 1;

	// Use this for initialization
	void Awake () {
        if(instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        clickScript = GetComponent<ClickManager>();
        for(int i = 0; i < players.Count; i++) {
            players[i].playerNumber = i + 1;
            Instantiate(players[i]);
        }
        InitGame();
	}

    void InitGame() {
        playerTurnText = GameObject.Find("PlayerTurnText").GetComponent<Text>();
        playerTurnText.text = "Player " + playerNumTurn + " Turn";
        endGameText = GameObject.Find("EndGameText").GetComponent<Text>();
        boardScript.SetupScene(level);
        MovingObject[] allUnits = FindObjectsOfType<MovingObject>();
        foreach(MovingObject unit in allUnits) {
            if(unit.playerOwner == 1) {
                player1Units.Add(unit);
            } else if (unit.playerOwner == 2) {
                player2Units.Add(unit);
            }
        }
    }

    void Update() {
        int player1Count = 0;
        int player2Count = 0;
        if (playerUnitSelected != null && defendingUnit != null) {
            ResolveCombat();
        }
        MovingObject[] allUnits = FindObjectsOfType<MovingObject>();
        foreach (MovingObject unit in allUnits) {
            if (unit.playerOwner == 1) {
                player1Count++;
            } else if (unit.playerOwner == 2) {
                player2Count++;
            }
        }

        if(player1Count == 0) {
            endGameText.text = "Player 2 Wins!";
        } else if (player2Count == 0) {
            endGameText.text = "Player 1 Wins!";
        }
    }

    public void EndPlayerTurn() {
        Debug.Log("Made it. It's player turn: " + playerNumTurn);
        MovingObject[] allUnits = FindObjectsOfType<MovingObject>();
        if (playerNumTurn == 1) {
            Debug.Log("Changing to player turn 2");
            playerNumTurn = 2;
            playerTurnText.text = "Player 2 Turn";
        } else if (playerNumTurn == 2) {
            playerNumTurn = 1;
            playerTurnText.text = "Player 1 Turn";
        } else {
            playerNumTurn = 1;
            playerTurnText.text = "Player 1 Turn";
        }

        foreach (MovingObject unit in allUnits) {
            unit.resetOnNewTurn();
        }
    }

    void ResolveCombat() {
        float deltaX = Mathf.Abs(defendingUnit.transform.position.x - playerUnitSelected.transform.position.x);
        float deltaY = Mathf.Abs(defendingUnit.transform.position.y - playerUnitSelected.transform.position.y);
        bool withinAttackerRange = (deltaX + deltaY) <= playerUnitSelected.attackRange;
        if (withinAttackerRange && (playerUnitSelected.playerOwner != defendingUnit.playerOwner) && !playerUnitSelected.hasAttacked) {
            defendingUnit.hp -= playerUnitSelected.attackScore;
            playerUnitSelected.hasAttacked = true;
        }
        playerUnitSelected.isSelected = false;
        playerUnitSelected = null;
        defendingUnit = null;
    }

    public void EndGame() {
        enabled = false;
    }

}
