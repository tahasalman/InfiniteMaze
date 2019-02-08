using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateMaze : MonoBehaviour {

    public GameObject player;
    public Transform tile;
    public Transform sideWall;
    public Transform bottomWall;

    private TileSet tileSet;
    private System.Random rand = new System.Random();

    private class TileSet
    {
        private int[] tileSet = new int[8];
        private int maxVal;
        private float lastZVal; 

        public TileSet(int[] tileSet,float last_z_val)
        {
            this.tileSet = tileSet;
            this.maxVal = findMaxVal(tileSet);
            this.lastZVal = last_z_val;
        }

        public float getLastZ()
        {
            return this.lastZVal;
        }
        public int[] getTileSet()
        {
            int[] copySet = new int[8];
            for (int i = 0; i < this.tileSet.Length; i++)
                copySet[i] = this.tileSet[i];
            return copySet;
        }

        public int getMax()
        {
            return this.maxVal;
        }
        public void updateTileSet(int[] newTiles)
        {
            this.tileSet = newTiles;
            this.maxVal = findMaxVal(this.tileSet);
            this.lastZVal += 5;
        }

        private static int findMaxVal(int[] arr)
        {
            int max = -1;
            if (arr.Length < 1)
                return max;

            max = arr[0];
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] > max)
                    max = arr[i];
            }

            return max;
        }

    }

	// Use this for initialization
	void Start () {
        //Initialize tiles as 0 which means they do not belong to any set
        int [] initTiles = new int[8];
        for (int i = 0; i < 8; i++)
            initTiles[i] = 0;
        tileSet = new TileSet(initTiles,500);
	}
	
	// Update is called once per frame
	void Update () {
        float last_z_position = tileSet.getLastZ();
        if (last_z_position - player.transform.position.z <5)
        {
            generateRow(new Vector3(250, 0, player.transform.position.z));
            addSideWall(new Vector3(210, 0, player.transform.position.z + 5));
            addSideWall(new Vector3(290, 0, player.transform.position.z + 5));
            runEllerAlg(last_z_position);

        }
	}

    private void generateRow(Vector3 position)
    {
        Instantiate(tile, position, Quaternion.identity);
    }

    /*
     * This method adds a bottom wall to the specified position
     */ 
    private void addBottomWall(Vector3 position)
    {
        Instantiate(bottomWall, position, Quaternion.identity);
    }

    /*
     * This method adds a side wall to the specified position
     */
    private void addSideWall(Vector3 position)
    {
        Instantiate(sideWall, position, Quaternion.identity);
    }

    /*
     * This method returns a boolean with equal probability of it being true or false
     */ 
    private bool getTrueOrFalse()
    {
        int randomNum = rand.Next(10);
        if (randomNum < 5)
        {
            return false;
        }
        return true;
    }

    private void runEllerAlg(float z_position)
    {
        int [] tiles = tileSet.getTileSet();
        int max = tileSet.getMax();
        
        // Step 1: Add any tile which is not a member of its own set to a unique set
        for(int i = 0; i < 8; i++)
        {
            if(tiles[i] == 0)       // tile is not a member of any other set
            {
                tiles[i] = max + 1;
                max++;
            }
        }

        // Step 2: Randomly add a wall between tiles or make a union out of them
        for(int i = 1; i < 8; i++)
        {
            bool doWeAddWall = getTrueOrFalse();
            if (doWeAddWall)
                addSideWall(new Vector3(210 + i * 10.0F, 0, z_position));
            else
                tiles[i] = tiles[i - 1];
        }

        tileSet.updateTileSet(tiles);   //update tileSEt with new tile data


        //Step3: Now we randomly add bottom walls ensuring that each set has atleast one cell with no bottom wall
        bool[] bottomSet = new bool[8]; 
        for(int i = 0; i < 7; i++)
        {
            int index = i;
            int sameSetCount = 1;
            for(; index < 7; index++)
            {
                if (tiles[index] != tiles[index + 1])
                    break;
                sameSetCount++;
            }

            if (sameSetCount != 1)
            {
                int wallsAdded = 0;
                for (int j = i; j < i + sameSetCount; j++)
                {
                    if (wallsAdded < sameSetCount - 1)
                    {
                        bool doWeAddWall = getTrueOrFalse();

                        if (doWeAddWall)
                        {
                            addBottomWall(new Vector3(220 + j *10F, 0, z_position + 5));
                            bottomSet[j] = true;
                        }
                        else
                            bottomSet[j] = false;


                    }
                    wallsAdded++;
                }
            }
            else
                bottomSet[i] = false;

            i += sameSetCount - 1;
        }

        //Step 5: Set up new row

        //First we create a new row which is copy of old row
        /*
        int[] newTiles = new int[8];
        for (int i = 0; i < 8; i++)
        {
            newTiles[i] = tiles[i];
        }

        //If a tile in the new row has a bottom set remove it from the set
        for(int i = 0; i < 8; i++)
        {
            if 
        }
        */

        for(int i = 0; i < 8; i++)
        {
            if (bottomSet[i])
            {
                tiles[i] = 0;
            } 
        }

        tileSet.updateTileSet(tiles);
    }

}
