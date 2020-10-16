using System;
using System.Collections.Generic;
using System.Text;

namespace A_star_Search_Algorithm
{
    class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Cost { get; set; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;
        public Tile Parent { get; set; }

        public bool Accept {get; set; }

        public FriendChoice(List<bool> shuffledList)
        {
          this.Accept = shuffledList[0];
          shuffledList.RemoveAt(0);
        }

        public void SetDistance(int targetX, int targetY)
        {
            this.Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
        }

    }
}
