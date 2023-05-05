namespace MinotaurLabyrinth
{
    public class Wall : Room
    {
        static Wall()
        {
            RoomFactory.Instance.Register(RoomType.Wall, () => new Wall());
        }
        public override RoomType Type { get; } = RoomType.Wall;

        // No need to override the Activate method here
        public override DisplayDetails Display()
        {
            return new DisplayDetails("[ ]", ConsoleColor.Black);
        }
    }
}
