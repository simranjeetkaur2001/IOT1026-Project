﻿namespace MinotaurLabyrinth
{
    /// <summary>
    /// A static class that provides methods to create and initialize a labyrinth map with various features, such as an entrance, sword, traps, monsters, and riddle rooms.
    /// </summary>
    public static class LabyrinthCreator
    {
        const int ScalingFactor = 16;
        static readonly Dictionary<Size, (int rows, int cols)> _mapSizeDimensions = new()
        {
            { Size.Small, (4, 4) },
            { Size.Medium, (6, 6) },
            { Size.Large, (8, 8) },
        };

        /// <summary>
        /// Initializes the labyrinth map with the specified size, and creates a new Hero at the entrance location.
        /// </summary>
        /// <param name="mapSize">The Size of the map to be created (Small, Medium, or Large).</param>
        /// <returns>A tuple containing the initialized Map and the Hero placed at the entrance location.</returns>
        public static (Map, Hero) InitializeMap(Size mapSize)
        {
            Map map = CreateMap(mapSize);
            ProceduralGenerator.Initialize(map);
            Location start = RandomizeMap(map);
            return (map, InitializePlayer(start));
        }

        private static Map CreateMap(Size mapSize)
        {
            if (!_mapSizeDimensions.TryGetValue(mapSize, out var dimensions))
            {
                throw new ArgumentException("Unknown map size");
            }

            return new Map(dimensions.rows, dimensions.cols);
        }

        /// <summary>
        /// Creates a labyrinth map with randomly placed and non-overlapping features, such as the entrance, sword, traps, monsters, and riddle rooms.
        /// </summary>
        /// <param name="map">The Map to be randomized with features.</param>
        /// <returns>The Location of the entrance in the randomized map.</returns>
        private static Location RandomizeMap(Map map)
        {
            Location start = PlaceEntrance(map);
            PlaceSword(map, start);
            AddRooms(RoomType.Pit, map);
            InitializeMonsters(map);
            AddEggRoom(map);
            return start;
        }

        /// <summary>
        /// Places the entrance room at a random edge location on the map.
        /// </summary>
        /// <param name="map">The map on which the entrance room will be placed.</param>
        /// <returns>The location of the placed entrance room.</returns>
        private static Location PlaceEntrance(Map map)
        {
            Location start = ProceduralGenerator.GetRandomEdgeLocation(map);
            map.SetRoomAtLocation(start, RoomType.Entrance);
            return start;
        }

        /// <summary>
        /// Places the sword room at a random location on the map that is not adjacent to the specified start location.
        /// </summary>
        /// <param name="map">The map on which the sword room will be placed.</param>
        /// <param name="start">The start location to avoid placing the sword room adjacent to.</param>
        private static void PlaceSword(Map map, Location start)
        {
            Location swordLocation = ProceduralGenerator.GetRandomLocationNotAdjacentTo(start);
            map.SetRoomAtLocation(swordLocation, RoomType.Sword);
        }

        /// <summary>
        /// Adds a specified number of rooms of the given room type to the map.
        /// </summary>
        /// <param name="roomType">The RoomType to be added to the map (e.g., Pit).</param>
        /// <param name="map">The Map to which the rooms should be added.</param>
        /// <param name="multiplier">An optional multiplier to scale the number of rooms to be added (default is 1).</param>
        private static void AddRooms(RoomType roomType, Map map, int multiplier = 1)
        {
            int numRooms = map.Rows * map.Columns * multiplier / ScalingFactor;
            for (int i = 0; i < numRooms; ++i)
            {
                map.SetRoomAtLocation(ProceduralGenerator.GetRandomLocation(), roomType);
            }
        }

        /// <summary>
        /// Initializes a Hero at the specified starting location.
        /// </summary>
        /// <param name="start">The Location where the Hero should be initialized.</param>
        /// <returns>A new Hero instance at the specified starting location.</returns>
        private static Hero InitializePlayer(Location start)
        {
            return new Hero(start);
        }

        /// <summary>
        /// Initializes monsters in the map, ensuring they do not overlap with existing locations.
        /// </summary>
        /// <param name="map">The Map in which monsters should be initialized.</param>
        private static void InitializeMonsters(Map map)
        {
            // Ensure monster locations do not overlap existing locations on the map
            Location minotaurLocation = ProceduralGenerator.GetRandomLocation();
            Room room = map.GetRoomAtLocation(minotaurLocation);
            room.AddMonster(new Minotaur());
        }

        /// <summary>
        /// Adds a riddle room to the map at a random location.
        /// </summary>
        /// <param name="map">The Map to which the riddle room should be added.</param>
        private static void AddEggRoom(Map map)
        {
            Location eggRoomLocation = ProceduralGenerator.GetRandomLocation();
            map.SetRoomAtLocation(eggRoomLocation, RoomType.EggRoom);
        }
    }
}