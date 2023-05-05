namespace MinotaurLabyrinth
{
    public class Room : IActivatable
    {
        static Room()
        {
            RoomFactory.Instance.Register(RoomType.Room, () => new Room());
        }
        private Monster? _monster;
        public virtual RoomType Type { get; } = RoomType.Room;
        public virtual bool IsActive { get; protected set; }
        public void AddMonster(Monster monster)
        {
            IsActive = true;
            _monster = monster;
        }
        public void RemoveMonster()
        {
            IsActive = false;
            _monster = null;
        }

        public virtual bool DisplaySense(Hero hero, int heroDistance)
        {
            if (_monster != null)
            {
                return _monster.DisplaySense(hero, heroDistance);
            }
            return false;
        }

        public virtual void Activate(Hero hero, Map map)
        {
            _monster?.Activate(hero, map);
        }

        public virtual DisplayDetails Display()
        {
            if (_monster != null)
                return _monster.Display();
            else
                return new DisplayDetails("[ ]", ConsoleColor.Gray);
        }
    }
}
