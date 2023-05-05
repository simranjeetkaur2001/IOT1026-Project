namespace MinotaurLabyrinth
{
    /// <summary>
    /// A factory class for creating Room instances based on RoomType.
    /// </summary>
    public sealed class RoomFactory
    {
        private RoomFactory() { }
        private static readonly Lazy<RoomFactory> _lazy = new(() => new RoomFactory());

        /// <summary>
        /// Singleton instance of RoomFactory.
        /// </summary>
        public static RoomFactory Instance => _lazy.Value;

        private readonly Dictionary<RoomType, Func<Room>> _callbacks = new();

        /// <summary>
        /// Registers a RoomType with a function that creates a Room instance.
        /// </summary>
        /// <param name="type">The RoomType to register.</param>
        /// <param name="createFn">The function that creates the Room instance.</param>
        /// <returns>True if the RoomType was registered successfully, false otherwise.</returns>
        public bool Register(RoomType type, Func<Room> createFn)
        {
            if (_callbacks.ContainsKey(type))
            {
                return false;
            }

            _callbacks.Add(type, createFn);
            return true;
        }

        /// <summary>
        /// Creates a Room instance based on the given RoomType.
        /// </summary>
        /// <param name="type">The RoomType of the room to be created.</param>
        /// <returns>A Room instance of the specified RoomType.</returns>
        public Room BuildRoom(RoomType type)
        {
            if (_callbacks.TryGetValue(type, out Func<Room>? function))
            {
                return function();
            }

            var atype = Type.GetType($"MinotaurLabyrinth.{type}") ?? throw new Exception($"Could not find type {type}");
            var room = (Room)Activator.CreateInstance(atype)!;
            Register(type, () => (Room)Activator.CreateInstance(atype)!);
            return room;
        }
    }
}
