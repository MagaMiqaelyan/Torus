namespace Torus
{
    public class Node
    {
        public Position Position { get; set; }
        public int Weight { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public Node Up { get; set; }
        public Node Down { get; set; }
        public bool IsEdge { get; set; }

        public Node(Position pos, int weight = 0, bool isEdge = false)
        {
            Position = pos;
            Weight = weight;
            IsEdge = isEdge;
        }
    }
}
