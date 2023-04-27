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
        Dictionary<Node, Node> cameFrom;        // parents
        Dictionary<Node, int> costSoFar;        // distances
        PriorityQueue frontier;                 // toVisit

        int width;
        int height;
        int[] cells;

        public int Width { get { return width; } }
        public Node[] Nodes { get; }

        Sprite sprite;

        public Map(int width, int height, int cellsNumber)
        {
            this.width = width;
            this.height = height;
            this.cells = new int[cellsNumber];
            Random rand = new Random();
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = rand.Next(4);
            }


            sprite = new Sprite(1, 1);  // each objects will be 1 cell (1 x 1) - to achieve this we must set the window orthographic size as well
            Nodes = new Node[cells.Length];

            // build Nodes from cells
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i] <= 0) { continue; }

                int x = i % width;
                int y = i / width;
                Nodes[i] = new Node(x, y, cells[i]);
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;

                    if (Nodes[index] == null)
                    {
                        continue;
                    }

                    AddNeighbours(Nodes[index], x, y);
                }
            }
        }

        // Add a Neighbour Node (to the passed one) for each direction if they are valid
        void AddNeighbours(Node node, int x, int y)
        {
            // Checks each direction neighbour for the current Node

            // TOP
            CheckNeighbours(node, x, y - 1);
            // BOTTOM
            CheckNeighbours(node, x, y + 1);
            // RIGHT
            CheckNeighbours(node, x + 1, y);
            // LEFT
            CheckNeighbours(node, x - 1, y);
        }

        // Checks if the Node at the passed X and Y is inside the map and it's valid and
        // if it is, adds it to the current Node's neighbours
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

            foreach (Node adj in node.Neighbours)
            {
                adj.RemoveNeighbour(node);
            }

            Nodes[index] = null;
            cells[index] = 0;
        }

        // Return the relative Node using coords
        Node GetNode(int x, int y)
        {
            if ((x >= width || x < 0) || (y >= height || y < 0)) { return null; }

            return Nodes[y * width + x];
        }

        public void ToggleNode(int x, int y)
        {
            Node node = GetNode(x, y);

            if (node == null)
            {
                AddNode(x, y);
            }
            else
            {
                RemoveNode(x, y);
            }
        }

        public void ToggleObstacle(int x, int y)
        {
            Node node = GetNode(x, y);

            if (node == null || node.Cost == 1)
            {
                if (node != null)
                {
                    RemoveNode(x, y);
                }
                AddNode(x, y, 3);
            }
            else
            {
                RemoveNode(x, y);
                AddNode(x, y);
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
                        sprite.DrawColor(new Vector4(0, 0, 0, 1));
                    }
                    else if (GetNode(x, y).Cost == 1)
                    {
                        sprite.DrawColor(new Vector4(1, 1, 1, 1));
                    }
                    else if (GetNode(x, y).Cost == 2)
                    {
                        sprite.DrawColor(new Vector4(0, 1, 1, 1));
                    }
                    else 
                    {
                        sprite.DrawColor(new Vector4(1, 0, 0, 1));
                    }
                }
            }
        }
    }
}
