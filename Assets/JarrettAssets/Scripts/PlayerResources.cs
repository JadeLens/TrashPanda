using UnityEngine;
using System.Collections;

//use this script on an empty object to hold the players resources
public class PlayerResources : MonoBehaviour
{
        private int Water = 0;
        private int Trash = 0;

        public int getTrash()
        {
            return Trash;
        }

        public int getWater()
        {
            return Water;
        }

        public void IncrementTrash(int x)
        {
            Trash += x;
        }

        public void IncrementWater(int x)
        {
            Water += x;
        }
}
