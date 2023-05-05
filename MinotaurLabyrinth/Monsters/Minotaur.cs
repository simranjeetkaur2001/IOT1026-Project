namespace MinotaurLabyrinth
{
    // Represents a minotaur in the game.
    public class Minotaur : Monster
    {
        // When activated, this moves the player two spaces east (+2 columns) and one space north (-1 row)
        // and the minotaur moves two spaces west (-2 columns) and one space south (+1 row). However,
        // it ensures both player and minotaur stay within the boundaries of the map and the minotaur is in a valid room.
        // If the player has found the sword, the minotaur takes it and places it back in the original room.
        public override void Activate(Hero hero, Map map)
        {
            const int RowMove = 1;
            const int ColMove = 2;
            ConsoleHelper.WriteLine("You have encountered the minotaur! He charges at you and knocks you into another room.", ConsoleColor.Magenta);
            if (hero.HasSword)
            {
                hero.HasSword = false;
                ConsoleHelper.WriteLine("After recovering your senses, you notice you are no longer in possession of the magic sword!", ConsoleColor.Magenta);
            }

            Location currentLocation = hero.Location;
            Room currentRoom = map.GetRoomAtLocation(currentLocation);

            // Clamp the player to a new location
            hero.Location = Clamp(new Location(hero.Location.Row - RowMove, hero.Location.Column + ColMove), map.Rows, map.Columns);

            // Clamp the minotaur to a valid location starting at the maximum clamp distance and working inwards
            for (int i = RowMove; i >= 0; --i)
            {
                for (int j = ColMove; j >= 0; --j)
                {
                    Location newLocation = Clamp(new Location(currentLocation.Row + i, currentLocation.Column - j), map.Rows, map.Columns);
                    Room room = map.GetRoomAtLocation(newLocation);
                    if (room.Type == RoomType.Room && !room.IsActive)
                    {
                        room.AddMonster(this);
                        currentRoom.RemoveMonster();
                        return;
                    }
                }
            }
        }

        // Takes a location and a map size, and produces a new location that is as much the same
        // as possible, but guarantees it is on the map.
        private static Location Clamp(Location location, int totalRows, int totalColumns)
        {
            int row = location.Row;
            if (row < 0) row = 0;
            if (row >= totalRows) row = totalRows - 1;

            int column = location.Column;
            if (column < 0) column = 0;
            if (column >= totalColumns) column = totalColumns - 1;

            return new Location(row, column);
        }

        public override bool DisplaySense(Hero hero, int heroDistance)
        {
            if (heroDistance == 1)
            {
                ConsoleHelper.WriteLine("You hear growling and stomping. The minotaur is nearby!", ConsoleColor.Red);
                return true;
            }
            return false;
        }

        public override DisplayDetails Display()
        {
            return new DisplayDetails("[M]", ConsoleColor.Red);
        }
    }
}
