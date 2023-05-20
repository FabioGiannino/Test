using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TowerDefence
{
    class Map : I_Drawable
    {
        private enum TILES
        {
            ROAD = 0,
            FOREST = 1,
            BLOCK = 2,
            WATER = 3,
            NON = 99
        }
        Dictionary<Node, Node> cameFrom;        // parents
        Dictionary<Node, int> costSoFar;        // distances
        PriorityQueue frontier;                 // toVisit

        int width;
        int height;
        int[] cells;

        private Random rand = new Random();
        public int Width { get { return width; } }
        public Node[] Nodes { get; }

        Sprite sprite;

        public Map(int horizontalCells, int verticalCells )
        {

            this.width = horizontalCells;
            this.height = verticalCells;
            int cellsNumber = horizontalCells * verticalCells;
            this.cells = new int[cellsNumber];

            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = 99;
            }
            
            sprite = new Sprite(1, 1);  // each objects will be 1 cell (1 x 1) - to achieve this we must set the window orthographic size as well
            Nodes = new Node[cells.Length];

            // build Nodes from cells
            for (int i = 0; i < cells.Length; i++)
            {
                int x = i % width;
                int y = i / width;
                Nodes[i] = new Node(x, y, cells[i]);
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    AddNeighbours(Nodes[index], x, y);
                }
            }

            ToggleForest(0, 0);
            WaveFunctionCollapse();
        }

        // Add a Neighbour Node (to the passed one) for each direction if they are valid
        void AddNeighbours(Node node, int x, int y)
        {
            // Checks each direction neighbour for the current Node            
            CheckNeighbours(node, x, y - 1);    // TOP            
            CheckNeighbours(node, x, y + 1);    // BOTTOM            
            CheckNeighbours(node, x + 1, y);    // RIGHT            
            CheckNeighbours(node, x - 1, y);    // LEFT
        }

        public void CheckNeighbours(Node currentNode, int cellX, int cellY)
        {
            if (cellX < 0 || cellX >= width)
            {
                return;
            }

            if (cellY < 0 || cellY >= height)
            {
                return;
            }

            int index = cellY * width + cellX;

            Node neighbour = Nodes[index];

            if (neighbour != null)
            {
                currentNode.AddNeighbour(neighbour);
            }
        }

        void AddNode(int x, int y, int cost = 1)
        {
            int index = y * width + x;
            Node node = new Node(x, y, cost);
            Nodes[index] = node;
            AddNeighbours(node, x, y);

            foreach (Node adj in node.Neighbours)
            {
                adj.AddNeighbour(node);
            }

            cells[index] = cost;
        }

        void RemoveNode(int x, int y)
        {
            int index = y * width + x;
            Node node = GetNode(x, y);
            /*
            foreach (Node adj in node.Neighbours)
            {
                adj.RemoveNeighbour(node);
            }
            */
            Nodes[index] = new Node(x, y, 99); 
            cells[index] = 99;
        }

        // Return the relative Node using coords
        Node GetNode(int x, int y)
        {
            if ((x >= width || x < 0) || (y >= height || y < 0)) { return null; }

            return Nodes[y * width + x];
        }

        public void ToggleRoad(int x, int y)
        {
            Node node = GetNode(x, y);

            if (node == null || node.Cost != 2)
            {
                AddNode(x, y, 0);
            }
            else
            {
                Console.WriteLine("removeRoad");
            }
        }
        public void ToggleForest(int x, int y)
        {
            Node node = GetNode(x, y);

            if (node == null || node.Cost != 1)
            {
                AddNode(x, y, 1);
            } 
            else
            {
                RemoveNode(x, y);
            }
        }
        public void ToggleBlock(int x, int y)
        {
            Node node = GetNode(x, y);

            if (node == null || node.Cost != 2)
            {
                AddNode(x, y, 2);
            }
            else
            {
                RemoveNode(x, y);
            }
        }
        public void ToggleSea(int x, int y)
        {
            Node node = GetNode(x, y);

            if (node == null || node.Cost != 3)
            {
                AddNode(x, y, 3);
            }
            else
            {
                RemoveNode(x, y);
            }
        }

        public List<Node> GetPath(int startX, int startY, int endX, int endY)
        {
            List<Node> path = new List<Node>();

            Node start = GetNode(startX, startY);
            Node end = GetNode(endX, endY);

            if (start == null || end == null)
            {
                return path;
            }

            AStar(start, end);

            if (!cameFrom.ContainsKey(end))
            {
                return path;
            }

            Node currNode = end;

            while (currNode != cameFrom[currNode])
            {
                path.Add(currNode);
                currNode = cameFrom[currNode];
            }

            path.Reverse();

            return path;
        }
        
        public void WaveFunctionCollapse()
        {
            Node start;
            
            int[] cards = new int[4];
            bool isEmpty = false;
            int x;
            int y;
            for (int k = 0; k <Nodes.Length; k++)
            {            

                start = Nodes[k];
                //setto le cards
                switch (start.Cost)
                {
                    case 0: //road
                        cards[(int)TILES.ROAD] = 3;
                        cards[(int)TILES.FOREST] = 1;
                        cards[(int)TILES.BLOCK] = 1;
                        cards[(int)TILES.WATER] = 0;
                        break;
                    case 1: //forest
                        cards[(int)TILES.ROAD]  = 2;
                        cards[(int)TILES.FOREST] = 1;
                        cards[(int)TILES.BLOCK] = 1;
                        cards[(int)TILES.WATER] = 1;
                        break;
                    case 2: //block
                        cards[(int)TILES.ROAD]  = 3;
                        cards[(int)TILES.FOREST] = 2;
                        cards[(int)TILES.BLOCK] = 0;                            
                        cards[(int)TILES.WATER] = 1;
                        break;
                    case 3: //water
                        cards[(int)TILES.ROAD]  = 2;
                        cards[(int)TILES.FOREST] = 1;
                        cards[(int)TILES.BLOCK] = 0;                            
                        cards[(int)TILES.WATER] = 3;
                        break;
                    default:
                        isEmpty = true;
                        break;
                }
                if (!isEmpty)
                {
                    foreach (Node neighbour in start.Neighbours)
                    {
                        switch (neighbour.Cost)
                        {
                            case 0: cards[(int)TILES.ROAD] = cards[(int)TILES.ROAD] >= 0 ? cards[(int)TILES.ROAD]-- : 0; break;
                            case 1: cards[(int)TILES.FOREST] = cards[(int)TILES.FOREST] >= 0 ? cards[(int)TILES.FOREST]-- : 0; break;
                            case 2: cards[(int)TILES.BLOCK] = cards[(int)TILES.BLOCK] >= 0 ? cards[(int)TILES.BLOCK]-- : 0; break;
                            case 3: cards[(int)TILES.WATER] = cards[(int)TILES.WATER] >= 0 ? cards[(int)TILES.WATER]-- : 0; break;
                        }
                    }
                    for (int i = 0; i < start.Neighbours.Count; i++)
                    {
                        x = start.Neighbours[i].X;
                        y = start.Neighbours[i].Y;
                        int index = y * width + x;
                        if (start.Neighbours[i].Cost == (int)TILES.NON)
                        {
                            int result = cards[(int)TILES.BLOCK] + cards[(int)TILES.FOREST] + cards[(int)TILES.ROAD] + cards[(int)TILES.FOREST];
                            int probability = rand.Next(result);
                            if (probability < cards[(int)TILES.ROAD])
                            {
                                cells[index] = (int)TILES.ROAD;
                                cards[(int)TILES.ROAD] = cards[(int)TILES.ROAD] >= 0 ? cards[(int)TILES.ROAD]-- : 0;
                            }
                            else if (probability < cards[(int)TILES.ROAD] + cards[(int)TILES.BLOCK])
                            {
                                cells[index] = (int)TILES.BLOCK;
                                cards[(int)TILES.BLOCK] = cards[(int)TILES.BLOCK] >= 0 ? cards[(int)TILES.BLOCK]-- : 0;
                            }
                            else if (probability < cards[(int)TILES.ROAD] + cards[(int)TILES.BLOCK] + cards[(int)TILES.FOREST])
                            {
                                cells[index] = (int)TILES.FOREST;
                                cards[(int)TILES.FOREST] = cards[(int)TILES.FOREST] >= 0 ? cards[(int)TILES.FOREST]-- : 0;
                            }
                            else if (probability < cards[(int)TILES.ROAD] + cards[(int)TILES.BLOCK] + cards[(int)TILES.FOREST] + cards[(int)TILES.WATER])
                            {
                                cells[index] = (int)TILES.WATER;
                                cards[(int)TILES.WATER] = cards[(int)TILES.WATER] >= 0 ? cards[(int)TILES.WATER]-- : 0;
                            }
                        }
                    }
                }
                x = k % width;
                y = k / width;
                if (Nodes[k].Cost == (int)TILES.NON)
                {
                    switch (cells[k])
                    {
                        case 0: ToggleRoad(x, y); break;
                        case 1: ToggleForest(x, y); break;
                        case 2: ToggleBlock(x, y); break;
                        case 3: ToggleSea(x, y); break;
                    }
                }
            }
        }

        public void AStar(Node start, Node end)
        {
            cameFrom = new Dictionary<Node, Node>();
            costSoFar = new Dictionary<Node, int>();
            frontier = new PriorityQueue();

            cameFrom[start] = start;
            costSoFar[start] = 0;
            frontier.Enqueue(start, Heuristic(start, end));

            while (!frontier.IsEmpty)
            {
                Node currNode = frontier.Dequeue();

                if (currNode == end)
                {
                    return;
                }

                foreach (Node nextNode in currNode.Neighbours)
                {
                    int newCost = costSoFar[currNode] + nextNode.Cost;

                    if (!costSoFar.ContainsKey(nextNode) || costSoFar[nextNode] > newCost)
                    {
                        cameFrom[nextNode] = currNode;
                        costSoFar[nextNode] = newCost;
                        int priority = newCost + Heuristic(nextNode, end);
                        frontier.Enqueue(nextNode, priority);
                    }
                }
            }
        }

        private int Heuristic(Node start, Node end)
        {
            return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);       // Manhattan Distance;
        }

        public void Draw()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    sprite.position = new Vector2(x, y);

                    if (GetNode(x, y) == null)
                    {
                    }
                    else if (GetNode(x, y).Cost == 0)
                    {
                        sprite.DrawTexture(GFXMngr.GetTexture("road"));
                    }
                    else if (GetNode(x, y).Cost == 1)
                    {
                        sprite.DrawTexture(GFXMngr.GetTexture("forest"));
                    }
                    else if (GetNode(x, y).Cost == 2)
                    {
                        sprite.DrawTexture(GFXMngr.GetTexture("block"));
                    }
                    else if (GetNode(x, y).Cost == 3)
                    {
                        sprite.DrawTexture(GFXMngr.GetTexture("water"));
                    }
                }
            }
        }
    }
}
