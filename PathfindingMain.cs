using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    public struct NodePosition
    {
        public int x;
        public int y;

        public NodePosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }


    public class Path : List<NodePosition>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Count; ++i)
            {
                sb.Append(string.Format("{1},{2}", i, this[i].x, this[i].y));

                if (i < Count - 1)
                {
                    sb.Append("-");
                }
            }

            return sb.ToString();
        }
    }


    public class Map
    {
        const int MAP_WIDTH = 60; //1200
        const int MAP_HEIGHT = 34; //680
        /*
        static int[] map = new int[MAP_WIDTH * MAP_HEIGHT]
        {
        // 0001020304050607080910111213141516171819
            1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,   // 00
            1,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,1,   // 01
            1,9,9,1,1,9,9,9,1,9,1,9,1,9,1,9,9,9,1,1,   // 02
            1,9,9,1,1,9,9,9,1,9,1,9,1,9,1,9,9,9,1,1,   // 03
            1,9,1,1,1,1,9,9,1,9,1,9,1,1,1,1,9,9,1,1,   // 04
            1,9,1,1,9,1,1,1,1,9,1,1,1,1,9,1,1,1,1,1,   // 05
            1,9,9,9,9,1,1,1,1,1,1,9,9,9,9,1,1,1,1,1,   // 06
            1,9,9,9,9,9,9,9,9,1,1,1,9,9,9,9,9,9,9,1,   // 07
            1,9,1,1,1,1,1,1,1,1,1,9,1,1,1,1,1,1,1,1,   // 08
            1,9,1,9,9,9,9,9,9,9,1,1,9,9,9,9,9,9,9,1,   // 09
            1,9,1,1,1,1,9,1,1,9,1,1,1,1,1,1,1,1,1,1,   // 10
            1,9,9,9,9,9,1,9,1,9,1,9,9,9,9,9,1,1,1,1,   // 11
            1,9,1,9,1,9,9,9,1,9,1,9,1,9,1,9,9,9,1,1,   // 12
            1,9,1,9,1,9,9,9,1,9,1,9,1,9,1,9,9,9,1,1,   // 13
            1,9,1,1,1,1,9,9,1,9,1,9,1,1,1,1,9,9,1,1,   // 14
            1,9,1,1,9,1,1,1,1,9,1,1,1,1,9,1,1,1,1,1,   // 15
            1,9,9,9,9,1,1,1,1,1,1,9,9,9,9,1,1,1,1,1,   // 16
            1,1,9,9,9,9,9,9,9,1,1,1,9,9,9,1,9,9,9,9,   // 17
            1,9,1,1,1,1,1,1,1,1,1,9,1,1,1,1,1,1,1,1,   // 18
            1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,   // 19
        };
        */

        // 60 x 34 mapa (level 6)
        static int[] map = new int[MAP_WIDTH * MAP_HEIGHT]
	{
	
		9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,  
		9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,1,1,1,1,1,9,  
        9,1,1,1,1,1,9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
		9,1,1,1,1,1,9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,1,1,1,1,1,9,   
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,1,1,1,1,1,9,  
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,1,1,1,1,1,9,  
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,1,1,1,1,1,9,  
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,1,1,1,1,1,9,  
		9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,1,1,1,1,1,9,  
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,1,1,1,1,1,9,  
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,1,1,1,1,1,9,  
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,1,1,1,1,1,9,  
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,1,1,1,1,1,9,  
        9,1,1,1,1,1,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,1,1,1,1,1,9,  
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
	    9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9,  
        9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,   
    };

        public static int GetMap(int x, int y)
        {
            if (x < 0 || x >= MAP_WIDTH || y < 0 || y >= MAP_HEIGHT)
            {
                return 9;
            }

            return map[(y * MAP_WIDTH) + x];
        }
    }

    public class MapSearchNode
    {
        public NodePosition position;
        AStarPathfinder pathfinder = null;


        public MapSearchNode(AStarPathfinder _pathfinder)
        {
            position = new NodePosition(0, 0);
            pathfinder = _pathfinder;
        }

        public MapSearchNode(NodePosition pos, AStarPathfinder _pathfinder)
        {
            position = new NodePosition(pos.x, pos.y);
            pathfinder = _pathfinder;
        }

        // Here's the heuristic function that estimates the distance from a Node
        // to the Goal. 
        public float GoalDistanceEstimate(MapSearchNode nodeGoal)
        {
            double X = (double)position.x - (double)nodeGoal.position.x;
            double Y = (double)position.y - (double)nodeGoal.position.y;
            // euklidska norma
            //return ((float)System.Math.Sqrt((X * X) + (Y * Y)));
            // manhatten norma
            return ((float)(System.Math.Abs(X) + System.Math.Abs(Y)));
        }

        public bool IsGoal(MapSearchNode nodeGoal)
        {
            return (position.x == nodeGoal.position.x && position.y == nodeGoal.position.y);
        }

        public bool ValidNeigbour(int xOffset, int yOffset)
        {
            // Return true if the node is navigable and within grid bounds
            return (Map.GetMap(position.x + xOffset, position.y + yOffset) < 9);
        }

        void AddNeighbourNode(int xOffset, int yOffset, NodePosition parentPos, AStarPathfinder aStarSearch)
        {
            if (ValidNeigbour(xOffset, yOffset) &&
                !(parentPos.x == position.x + xOffset && parentPos.y == position.y + yOffset))
            {
                NodePosition neighbourPos = new NodePosition(position.x + xOffset, position.y + yOffset);
                MapSearchNode newNode = pathfinder.AllocateMapSearchNode(neighbourPos);
                aStarSearch.AddSuccessor(newNode);
            }
        }

        // This generates the successors to the given Node. It uses a helper function called
        // AddSuccessor to give the successors to the AStar class. The A* specific initialisation
        // is done for each node internally, so here you just set the state information that
        // is specific to the application
        public bool GetSuccessors(AStarPathfinder aStarSearch, MapSearchNode parentNode)
        {
            NodePosition parentPos = new NodePosition(0, 0);

            if (parentNode != null)
            {
                parentPos = parentNode.position;
            }

            // push each possible move except allowing the search to go backwards
            AddNeighbourNode(-1, 0, parentPos, aStarSearch);
            AddNeighbourNode(0, -1, parentPos, aStarSearch);
            AddNeighbourNode(1, 0, parentPos, aStarSearch);
            AddNeighbourNode(0, 1, parentPos, aStarSearch);



            return true;
        }

        // given this node, what does it cost to move to successor. In the case
        // of our map the answer is the map terrain value at this node since that is 
        // conceptually where we're moving
        public float GetCost(MapSearchNode successor)
        {
            // Implementation specific
            return Map.GetMap(successor.position.x, successor.position.y);
        }

        public bool IsSameState(MapSearchNode rhs)
        {
            return (position.x == rhs.position.x &&
                    position.y == rhs.position.y);
        }
    }


    public class AStarExample
    {
        // nizovi koji cuvaju x i y koordinate za dolazak AI zmije do cilja, tj. hrane
        public static int[] koordXi = new int[100000];
        public static int[] koordYi = new int[100000];

        public static bool pathfindSucceeded;

        public static bool Pathfind(NodePosition startPos, NodePosition goalPos, AStarPathfinder pathfinder)
        {
            // Reset the allocated MapSearchNode pointer
            pathfinder.InitiatePathfind();

            // Create a start state
            MapSearchNode nodeStart = pathfinder.AllocateMapSearchNode(startPos);

            // Define the goal state
            MapSearchNode nodeEnd = pathfinder.AllocateMapSearchNode(goalPos);

            // Set Start and goal states
            pathfinder.SetStartAndGoalStates(nodeStart, nodeEnd);

            // Set state to Searching and perform the search
            AStarPathfinder.SearchState searchState = AStarPathfinder.SearchState.Searching;
            uint searchSteps = 0;

            do
            {
                searchState = pathfinder.SearchStep();
                searchSteps++;
            }
            while (searchState == AStarPathfinder.SearchState.Searching);

            // Search complete
            pathfindSucceeded = (searchState == AStarPathfinder.SearchState.Succeeded);

            if (pathfindSucceeded)
            {
                // Success
                Path newPath = new Path();
                int numSolutionNodes = 0;	// Don't count the starting cell in the path length

                // Get the start node
                MapSearchNode node = pathfinder.GetSolutionStart();
                newPath.Add(node.position);
                ++numSolutionNodes;

                // Get all remaining solution nodes
                for (; ; )
                {
                    node = pathfinder.GetSolutionNext();

                    if (node == null)
                    {
                        break;
                    }

                    ++numSolutionNodes;
                    newPath.Add(node.position);
                };

                // Once you're done with the solution we can free the nodes up
                pathfinder.FreeSolutionNodes();




                // parsiranje string buildera u kojem su pohranjene koordinate do hrane koja
                // je cilj za AI zmiju
                string[] koordinate = newPath.ToString().Split('-');

                StringBuilder izlazX = new StringBuilder();
                StringBuilder izlazY = new StringBuilder();

                for (int i = 0; i <= koordinate.Length - 1; i++)
                    izlazX.AppendFormat("{0},", koordinate[i].Split(',')[0]);

                for (int i = 0; i <= koordinate.Length - 1; i++)
                    izlazY.AppendFormat("{0},", koordinate[i].Split(',')[1]);

                string[] koordX = izlazX.ToString().Split(',');
                string[] koordY = izlazY.ToString().Split(',');

                for (int i = 0; i <= koordinate.Length - 1; i++)
                    koordXi[i] = Int32.Parse(koordX[i]);

                for (int i = 0; i <= koordinate.Length - 1; i++)
                    koordYi[i] = Int32.Parse(koordY[i]);
                //


            }
            else if (searchState == AStarPathfinder.SearchState.Failed)
            {
                // FAILED, no path to goal
                //pathfindSucceeded = false;
                return pathfindSucceeded;
            }

            return pathfindSucceeded;
        }
    }
}
