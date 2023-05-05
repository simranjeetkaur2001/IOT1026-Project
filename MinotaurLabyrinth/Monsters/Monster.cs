namespace MinotaurLabyrinth
{
    /// <summary>
    /// Represents one of the several monster types in the game.
    /// </summary>
    public abstract class Monster : IActivatable
    {
        // Whether the monster is alive or not.
        public bool IsAlive { get; set; } = true;

        // Called when the monster and the player are both in the same room. Gives
        // the monster a chance to do its thing.
        public abstract void Activate(Hero hero, Map map);
        public abstract bool DisplaySense(Hero hero, int heroDistance);
        public abstract DisplayDetails Display();
    }
}
