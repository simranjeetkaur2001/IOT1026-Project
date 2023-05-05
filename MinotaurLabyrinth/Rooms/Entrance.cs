namespace MinotaurLabyrinth
{
    public class Entrance : Room
    {
        static Entrance()
        {
            RoomFactory.Instance.Register(RoomType.Entrance, () => new Entrance());
        }
        public override RoomType Type { get; } = RoomType.Entrance;
        public override bool IsActive { get; protected set; } = true;
        public override void Activate(Hero hero, Map map)
        {
            if (hero.HasSword)
                hero.IsVictorious = true;
        }
        public override DisplayDetails Display()
        {
            return new DisplayDetails($"[{Type.ToString()[0]}]", ConsoleColor.DarkGreen);
        }

        // Displays the appropriate message if the player can see the light from outside the labyrinth
        public override bool DisplaySense(Hero hero, int heroDistance)
        {
            if (heroDistance == 0)
                ConsoleHelper.WriteLine("You see light in this room coming from outside the labyrinth. This is the entrance.", ConsoleColor.Yellow);
            else if (heroDistance == 1)
                ConsoleHelper.WriteLine("You hear birds chirping faintly in the distance. An entrance should be nearby.", ConsoleColor.Yellow);
            else
                return false;
            return true;
        }
    }
}
