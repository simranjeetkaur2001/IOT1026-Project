﻿using System;
using System.Collections.Generic;

namespace MinotaurLabyrinth
{
    // Represents the map and what each room is made out of.
    public class Map
    {
        
        // The rooms are stored in a 2D array. A 3D array would allow for multiple levels in the dungeon. 
        private readonly Room[,] _rooms;

        // The total number of rows in this specific game world.
        public int Rows { get; }

        // The total number of columns in this specific game world.
        public int Columns { get; }

        // Debugging mode display all the rooms and monsters on the map
        public bool DebugMode { get; set; }

        // Creates a new map with a specific size.
        public Map(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _rooms = new Room[rows, columns];
            // Initialize all rooms to the basic type
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < columns; ++j)
                {
                    _rooms[i, j] = new Room();
                }
            }
        }

        // Returns the type of room at a specific location -> returns a Wall RoomType if the location is off the map
        public RoomType GetRoomTypeAtLocation(Location location) => GetRoomAtLocation(location).Type;

        // Returns a reference to the room object at a specific location. If the location is off the map, a Wall will be returned
        public Room GetRoomAtLocation(Location location) => IsOnMap(location) ? _rooms[location.Row, location.Column] : new Wall();

        /// <summary>
        /// Checks if a given location has a neighboring location with the specified room type.
        /// Neighboring locations are those that are adjacent vertically, horizontally, or diagonally.
        /// </summary>
        /// <param name="location">The location to check for neighbors.</param>
        /// <param name="roomType">The room type to check for in neighboring locations.</param>
        /// <returns>True if a neighboring location with the specified room type is found; otherwise, false.</returns>
        public bool HasNeighborWithType(Location location, RoomType roomType)
        {
            // Define the possible neighboring location offsets
            int[] rowOffsets = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] colOffsets = { -1, 0, 1, -1, 1, -1, 0, 1 };

            // Loop through the neighboring locations
            for (int i = 0; i < rowOffsets.Length; i++)
            {
                Location neighborLocation = new(location.Row + rowOffsets[i], location.Column + colOffsets[i]);
                if (GetRoomTypeAtLocation(neighborLocation) == roomType)
                {
                    return true;
                }
            }
            return false;
        }

        // Returns a list of Locations that are within the player's sense range
        public List<Location> GetSensableLocations(Hero player)
        {
            List<Location> sensables = new();
            int distance = player.SenseRange;
            if (distance == 0)
            {
                sensables.Add(player.Location);
            }
            else
            {
                (int x, int y) = player.Location;
                for (int i = -distance; i <= distance; ++i)
                {
                    for (int j = -distance; j <= distance; ++j)
                    {
                        if (Math.Abs(i) + Math.Abs(j) <= distance)
                        {
                            Location loc = new(x + j, y + i);
                            if (IsOnMap(loc))
                                sensables.Add(loc);
                        }
                    }
                }
            }
            return sensables;
        }

        // Indicates whether a specific location is actually on the map or not.
        public bool IsOnMap(Location location) =>
            location.Row >= 0 &&
            location.Row < _rooms.GetLength(0) &&
            location.Column >= 0 &&
            location.Column < _rooms.GetLength(1);

        // Changes the type of room at a specific spot in the world to a new type.
        public void SetRoomAtLocation(Location location, RoomType roomType) => _rooms[location.Row, location.Column] = RoomFactory.Instance.BuildRoom(roomType);


        // Adds a riddle room at the specified location on the map
        public void AddEggRoom(Location location)
        {
            if (IsOnMap(location))
            {
                _rooms[location.Row, location.Column] = RoomFactory.Instance.BuildRoom(RoomType.EggRoom);
                }
        }

        // Moves the player to the specified room in the map.
        public void MovePlayerToRoom(Hero player, Room room)
        {
            Location roomLocation = FindRoomLocation(room);
            if (roomLocation != null)
            {
                player.Location = roomLocation;
            }
        }

        // Finds the location of a specific room in the map.
        private Location FindRoomLocation(Room room)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (_rooms[i, j] == room)
                    {
                        return new Location(i, j);
                    }
                }
            }
            return null;
        }

        



        

        // Displays the map on the console. If the player does not have a map, only their current location is displayed.
        public void Display(Location playerLocation, bool playerHasMap = true, bool debugMode = false)
        {
            for (int i = 0; i < Rows; ++i)
            {
                for (int j = 0; j < Columns; ++j)
                {
                    if (playerLocation.Row == i && playerLocation.Column == j)
                    {
                        ConsoleHelper.Write("(X)", ConsoleColor.Yellow);
                    }
                    else
                    {
                        Location loc = new(i, j);
                        var room = GetRoomAtLocation(loc);

                        if (debugMode)
                        {
                            ConsoleHelper.Write(room.Display());
                        }
                        else if (playerHasMap)
                        {
                            if (room.Type == RoomType.Entrance)
                                ConsoleHelper.Write(room.Display());
                            else if (room.Type == RoomType.EggRoom && room.IsActive)
                                ConsoleHelper.Write("[()]", ConsoleColor.Green);
                            else
                                ConsoleHelper.Write("[ ]", ConsoleColor.Gray);
                        }
                        else
                        {
                            ConsoleHelper.Write("[ ]", ConsoleColor.Black);
                        }
                    }
                }


                

                Console.WriteLine();
            }
        }
    }
}