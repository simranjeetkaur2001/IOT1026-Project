namespace MinotaurLabyrinth
{
    /// <summary>
    /// A static class that provides methods to generate random locations and other procedural generation-related functionality.
    /// </summary>
    public static class ProceduralGenerator
    {
        private static HashSet<Location> _locations = null!;
        private static HashSet<Location> _usedLocations = null!;

        /// <summary>
        /// Initializes the ProceduralGenerator by creating a set of all possible locations on the given map.
        /// This method must be called before using any other methods in this class.
        /// </summary>
        /// <param name="map">The Map for which the ProceduralGenerator should be initialized.</param>
        public static void Initialize(Map map)
        {
            var numRows = map.Rows;
            var numCols = map.Columns;
            // Create 2 lists representing the row/col positions.
            // This allows everything to work even if our grid is not a square.
            var rowPositions = Enumerable.Range(0, numRows).ToList();
            var colPositions = Enumerable.Range(0, numCols).ToList();

            // Will hold all the possible locations
            _locations = new HashSet<Location>(numRows * numCols);
            // Will hold only the used locations
            _usedLocations = new HashSet<Location>(numRows * numCols);

            foreach (int row in rowPositions)
            {
                foreach (int col in colPositions)
                    _locations.Add(new Location(row, col));
            }
        }

        /// <summary>
        /// Get a random edge location on the map. Edge locations are those on the outer
        /// perimeter of the map, excluding the corners.
        /// </summary>
        /// <param name="map">The map for which an edge location is needed.</param>
        /// <returns>A random location on the edge of the map.</returns>
        public static Location GetRandomEdgeLocation(Map map)
        {
            int rows = map.Rows;
            int cols = map.Columns;
            // Use a set to store viable locations so that duplicate locations will automatically be discarded
            var locations = new HashSet<Location>();

            // Iterate over top and bottom rows
            for (int col = 0; col < cols; ++col)
            {
                AddEdgeLocationIfValid(map, locations, 0, col);
                AddEdgeLocationIfValid(map, locations, rows - 1, col);
            }

            // Iterate over left and right columns (skip corners, as they were already covered)
            for (int row = 1; row < rows - 1; ++row)
            {
                AddEdgeLocationIfValid(map, locations, row, 0);
                AddEdgeLocationIfValid(map, locations, row, cols - 1);
            }

            Location start = locations.ElementAt(RandomNumberGenerator.Next(locations.Count));
            // Only consider the location used if the location is valid
            _usedLocations.Add(start);
            return start;
        }

        /// <summary>
        /// Adds an edge location to the given HashSet if the location is valid (i.e., not a wall).
        /// </summary>
        /// <param name="map">The Map in which the edge location should be checked for validity.</param>
        /// <param name="locations">The HashSet to which the edge location should be added if valid.</param>
        /// <param name="row">The row index of the edge location.</param>
        /// <param name="col">The column index of the edge location.</param>
        private static void AddEdgeLocationIfValid(Map map, HashSet<Location> locations, int row, int col)
        {
            Location loc = new(row, col);
            // Verify the location not off map in the event 'walls' are added
            if (map.GetRoomTypeAtLocation(loc) != RoomType.Wall)
            {
                locations.Add(loc);
            }
        }

        /// <summary>
        /// Returns a random location on the map that is not adjacent to the given location.
        /// </summary>
        /// <param name="location">The Location to which the returned location should not be adjacent.</param>
        /// <returns>A random location on the map that is not adjacent to the given location.</returns>
        public static Location GetRandomLocationNotAdjacentTo(Location location)
        {
            EnsureInitialized();
            Location newLocation = GetRandomLocation(false);
            while (IsAdjacent(location, newLocation))
            {
                newLocation = GetRandomLocation(false);
            }
            // Only consider the location used if the location is valid
            _usedLocations.Add(newLocation);
            return newLocation;
        }

        /// <summary>
        /// Checks if two locations are adjacent to each other.
        /// </summary>
        /// <param name="loc1">The first Location to be compared.</param>
        /// <param name="loc2">The second Location to be compared.</param>
        /// <returns>True if the locations are adjacent, otherwise false.</returns>
        private static bool IsAdjacent(Location loc1, Location loc2)
        {
            int rowDiff = Math.Abs(loc1.Row - loc2.Row);
            int colDiff = Math.Abs(loc1.Column - loc2.Column);

            return rowDiff <= 1 && colDiff <= 1 && !(rowDiff == 0 && colDiff == 0);
        }

        /// <summary>
        /// Get a random map location. The parameters allow the user to specify whether the location should be considered
        /// used for future method calls and whether the location is allowed to have an interactive event already present or not.
        /// </summary>
        /// <param name="removeLocation">Whether the location should be considered used for future function calls</param>
        /// <param name="unused">Whether the location is required to be unused and have no interactive event already present</param>
        /// <returns>A random location on the map</returns>
        public static Location GetRandomLocation(bool removeLocation = true, bool unused = true)
        {
            EnsureInitialized();
            if (_usedLocations.Count == _locations.Count)
                throw new CreatorException(_locations.Count, _usedLocations.Count);
            Location location = unused ? _locations.Except(_usedLocations).ElementAt(RandomNumberGenerator.Next(_locations.Count - _usedLocations.Count))
                                       : _locations.ElementAt(RandomNumberGenerator.Next(_locations.Count));
            if (removeLocation)
            {
                _usedLocations.Add(location);
            }
            return location;
        }

        private static void EnsureInitialized()
        {
            if (_locations == null || _usedLocations == null)
            {
                throw new InvalidOperationException("ProceduralGenerator has not been initialized. Call Initialize() method before using it.");
            }
        }
    }

    /// <summary>
    /// An exception class that represents errors that occur during the procedural generation process.
    /// </summary>
    public class CreatorException : Exception
    {
        public CreatorException() { }

        public CreatorException(string message)
            : base(message)
        { }

        public CreatorException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public CreatorException(int totalRooms, int usedRooms)
            : base("Not enough rooms remaining in the labyrinth!\n" +
                   $"Total rooms: {totalRooms}, Used rooms: {usedRooms}")
        { }
    }
}
