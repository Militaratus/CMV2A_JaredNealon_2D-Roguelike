﻿using UnityEngine;
using System;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random; 		//Tells Random to use the Unity Engine random number generator.

namespace Completed
	
{
	
	public class BoardManager : MonoBehaviour
	{
		// Using Serializable allows us to embed a class with sub properties in the inspector.
		[Serializable]
		public class Count
		{
			public int minimum; 			//Minimum value for our Count class.
			public int maximum; 			//Maximum value for our Count class.
			
			
			//Assignment constructor.
			public Count (int min, int max)
			{
				minimum = min;
				maximum = max;
			}
		}
		
		
		public int columns = 8; 										//Number of columns in our game board.
		public int rows = 8;											//Number of rows in our game board.
		public Count wallCount = new Count (5, 9);						//Lower and upper limit for our random number of walls per level.
		public Count foodCount = new Count (1, 5);						//Lower and upper limit for our random number of food items per level.
        public Count goldCount = new Count(1, 5);                       //[JARED] Lower and upper limit for our random number of gold items per level.
        public GameObject exit;											//Prefab to spawn for exit.
		public GameObject[] floorTiles;									//Array of floor prefabs.
		public GameObject[] wallTiles;									//Array of wall prefabs.
		public GameObject[] foodTiles;									//Array of food prefabs.
        public GameObject[] goldTiles;                                  //[JARED] Array of gold prefabs.
        public GameObject[] enemyTiles;									//Array of enemy prefabs.
        public GameObject[] npcTiles;                                   //[JARED] Array of npc prefabs.  
        public GameObject[] outerWallTiles;								//Array of outer tile prefabs.
		
		private Transform boardHolder;									//A variable to store a reference to the transform of our Board object.
		private List <Vector3> gridPositions = new List <Vector3> ();   //A list of possible locations to place tiles.

        //[JARED] Keep track of counts for analytics
        internal int levelEnemyCount;
        internal int levelFoodCount;
        internal int levelGoldCount;
		
		//Clears our list gridPositions and prepares it to generate a new board.
		void InitialiseList ()
		{
			//Clear our list gridPositions.
			gridPositions.Clear ();
			
			//Loop through x axis (columns).
			for(int x = 1; x < columns-1; x++)
			{
				//Within each column, loop through y axis (rows).
				for(int y = 1; y < rows-1; y++)
				{
					//At each index add a new Vector3 to our list with the x and y coordinates of that position.
					gridPositions.Add (new Vector3(x, y, 0f));
				}
			}
		}
		
		
		//Sets up the outer walls and floor (background) of the game board.
		void BoardSetup ()
		{
			//Instantiate Board and set boardHolder to its transform.
			boardHolder = new GameObject ("Board").transform;
			
			//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
			for(int x = -1; x < columns + 1; x++)
			{
				//Loop along y axis, starting from -1 to place floor or outerwall tiles.
				for(int y = -1; y < rows + 1; y++)
				{
					//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
					GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];
					
					//Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
					if(x == -1 || x == columns || y == -1 || y == rows)
						toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					
					//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
					GameObject instance =
						Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					
					//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
					instance.transform.SetParent (boardHolder);
				}
			}
		}
		
		
		//RandomPosition returns a random position from our list gridPositions.
		Vector3 RandomPosition ()
		{
			//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
			int randomIndex = Random.Range (0, gridPositions.Count);
			
			//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
			Vector3 randomPosition = gridPositions[randomIndex];
			
			//Remove the entry at randomIndex from the list so that it can't be re-used.
			gridPositions.RemoveAt (randomIndex);
			
			//Return the randomly selected Vector3 position.
			return randomPosition;
		}
		
		
		//LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
		void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
		{
			//Choose a random number of objects to instantiate within the minimum and maximum limits
			int objectCount = Random.Range (minimum, maximum+1);

            //[JARED] Store objectCounts
            if (tileArray == enemyTiles)
            {
                levelEnemyCount = objectCount;
            }
            if (tileArray == foodTiles)
            {
                levelFoodCount = objectCount;
            }
            if (tileArray == goldTiles)
            {
                levelGoldCount = objectCount;
            }

            //Instantiate objects until the randomly chosen limit objectCount is reached
            for (int i = 0; i < objectCount; i++)
			{
				//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
				Vector3 randomPosition = RandomPosition();
				
				//Choose a random tile from tileArray and assign it to tileChoice
				GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
				
				//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
				Instantiate(tileChoice, randomPosition, Quaternion.identity);
			}
		}
		
		
		//SetupScene initializes our level and calls the previous functions to lay out the game board
		public void SetupScene (int level)
		{
			//Creates the outer walls and floor.
			BoardSetup ();
			
			//Reset our list of gridpositions.
			InitialiseList ();
			
			//Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
			LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);

            //[JARED] Check if we are at a bonus level yet
            if (level % 5 == 0 || level == 1)
            {
                //Instantiate a static number of npc's, at randomized position.
                int npcCount = 1;
                LayoutObjectAtRandom(npcTiles, npcCount, npcCount);
            }
            else
            {
                //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
                LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

                //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
                LayoutObjectAtRandom(goldTiles, goldCount.minimum, goldCount.maximum);

                //Determine number of enemies based on current level number, based on a logarithmic progression
                int enemyCount = (int)Mathf.Log(level, 2f);

                //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
                LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
            }

            /* Old Code
			//Instantiate the exit tile in the upper right hand corner of our game board
			Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
            */

            // Refactored Code from Jared
            //Instantiate the exit tile in one of the corners of our game board
            SpawnExit();
        }

        // Define which one of now three exit spawn points to use and actually spawn the exit
        void SpawnExit()
        {
            int spawnLocation = Random.Range(0, 3);
            int spawnColumn = 0;
            int spawnRow = 0;

            switch (spawnLocation)
            {
                case 0:
                    spawnColumn = 0; spawnRow = rows - 1; break;
                case 1:
                    spawnColumn = columns - 1; spawnRow = rows - 1; break;
                case 2:
                    spawnColumn = columns - 1; spawnRow = 0; break;
                default:
                    spawnColumn = columns - 1; spawnRow = rows - 1; break;
            }

            Instantiate(exit, new Vector3(spawnColumn, spawnRow, 0f), Quaternion.identity);
        }
	}
}
