using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardManager : MonoBehaviour {
    //TODO: Make a "playerOwnable" script that MovingObject and CapturableStruct can inherit

    public int mapHeight = 10;
    public int mapWidth = 10;

    public string mapConfigFile; // A map config file for generating maps

    public int numberOfPlayers = 1;

    public Sprite player1Sprite;
    public Sprite player2Sprite;

    public TextAsset[] levelFiles;

    public CapturableStruct[] playerHqs;

    public Unit[] units;

    public GameObject grassTile;
    public GameObject bridgeTile;
    public GameObject waterTile;
    public GameObject treeTile;

    private Transform boardHolder; //Not sure if needed

    private List<Vector3> gridPositions = new List<Vector3>(); //Not sure if needed

    //Clears our list gridPositions and prepares it to generate a new board.
    void InitialiseList() {
        //Loop through x axis (columns).
        for (int x = 0; x < mapHeight; x++) {
            //Within each column, loop through y axis (rows).
            for (int y = 0; y < mapWidth; y++) {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup(int level) {
        boardHolder = new GameObject("Board").transform;
        int textCounter = 0;
        StreamReader reader = new StreamReader("Assets/Levels/level1.txt");
        string levelConfig = "";

        for (int y = (mapHeight - 1); y >= 0; y--) {
            levelConfig = reader.ReadLine();
            textCounter = 0;
            for (int x = 0; x < mapWidth; x++) {
                GameObject toInsantiate = null;
                if (levelConfig[textCounter].Equals('g')) {
                     toInsantiate = grassTile;
                } else if (levelConfig[textCounter].Equals('x')) {
                    toInsantiate = waterTile;
                } else {
                    toInsantiate = grassTile;
                }

                GameObject instance = Instantiate(toInsantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);

                textCounter++;
            }
        }

        reader.Close();

    }

    void PlaceStructures() {
        int textCounter = 0;
        StreamReader reader = new StreamReader("Assets/Levels/level1.txt");
        string levelConfig = "";

        for (int y = (mapHeight - 1); y >= 0; y--) {
            levelConfig = reader.ReadLine();
            textCounter = 0;
            for (int x = 0; x < mapWidth; x++) {
                CapturableStruct toInsantiate = null;
                if (levelConfig[textCounter].Equals('1')) {
                    toInsantiate = playerHqs[0];
                    toInsantiate.playerOwner = 1;
                    Instantiate(toInsantiate, new Vector3(x, y, 0f), Quaternion.identity);
                } else if (levelConfig[textCounter].Equals('2')) {
                    toInsantiate = playerHqs[0];
                    toInsantiate.playerOwner = 2;
                    Instantiate(toInsantiate, new Vector3(x, y, 0f), Quaternion.identity);
                }


                textCounter++;
            }
        }

        reader.Close();
    }


    void PlaceUnits() {
        int textCounter = 0;
        StreamReader reader = new StreamReader("Assets/Levels/level1.txt");
        string levelConfig = "";

        for (int y = (mapHeight - 1); y >= 0; y--) {
            levelConfig = reader.ReadLine();
            textCounter = 0;
            for (int x = 0; x < mapWidth; x++) {
                Unit toInsantiate = null;
                if (levelConfig[textCounter].Equals('a')) {
                    toInsantiate = units[0];
                    toInsantiate.playerOwner = 1;
                    Instantiate(toInsantiate, new Vector3(x, y, 0f), Quaternion.identity);
                } else if (levelConfig[textCounter].Equals('A')) {
                    toInsantiate = units[0];
                    toInsantiate.playerOwner = 2;
                    Instantiate(toInsantiate, new Vector3(x, y, 0f), Quaternion.identity);
                }



                textCounter++;
            }
        }

        reader.Close();

    }

    public void SetupScene(int level) {
        BoardSetup(level);
        InitialiseList();
        PlaceStructures();
        PlaceUnits();
    }


}
