using System.Text;

namespace MinotaurLabyrinth
{
    // Container for the command objects and the valid user input to call the corresponding command
    /// <summary>
    /// Represents a collection of game commands, allowing commands to be added or removed dynamically throughout the game.
    /// </summary>
    public class CommandList
    {
        private readonly Dictionary<string, ICommand> _commands = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<(ICommand command, List<string> inputs)> _commandInputs = new();

        public CommandList()
        {
            AddCommand(new List<string>() { "n", "north" }, new MoveNorthCommand());
            AddCommand(new List<string>() { "s", "south" }, new MoveSouthCommand());
            AddCommand(new List<string>() { "e", "east" }, new MoveEastCommand());
            AddCommand(new List<string>() { "w", "west" }, new MoveWestCommand());
            AddCommand(new List<string>() { "g", "grab sword" }, new GetSwordCommand());
            AddCommand(new List<string>() { "q", "quit" }, new QuitCommand());
            AddCommand(new List<string>() { "secret" }, new SecretCommand());
            AddCommand(new List<string>() { "d", "debug" }, new DebugMapCommand());
        }

        /// <summary>
        /// Adds a command to the command list, associated with the specified input strings.
        /// </summary>
        /// <param name="inputs">The strings that represent valid inputs to execute the command.</param>
        /// <param name="command">The command that will be added.</param>
        public void AddCommand(List<string> inputs, ICommand command)
        {
            var commandType = command.GetType();
            if (!_commands.Values.Any(c => c.GetType() == commandType))
            {
                foreach (string input in inputs)
                {
                    _commands[input] = command;
                }
                _commandInputs.Add((command, inputs));
            }
        }

        /// <summary>
        /// Removes a command from the command list based on its type.
        /// </summary>
        /// <param name="commandType">The type of the command that will be removed.</param>
        public void RemoveCommand(Type commandType)
        {
            foreach (var key in _commands.Where(kvp => kvp.Value.GetType() == commandType).Select(kvp => kvp.Key).ToList())
            {
                _commands.Remove(key);
            }
            _commandInputs.RemoveAll(ci => ci.command.GetType() == commandType);
        }

        /// <summary>
        /// Finds the command corresponding to the given string input.
        /// </summary>
        /// <param name="input">A string that corresponds to a valid command in the CommandList.</param>
        /// <returns>Returns the command object corresponding to the input or null if the input does not correspond to a command.</returns>
        public ICommand? GetCommand(string input)
        {
            _commands.TryGetValue(input, out ICommand? command);
            return command;
        }

        /// <summary>
        /// Returns a string representation of the non-secret commands in the CommandList.
        /// </summary>
        /// <returns>A string representation of the non-secret commands.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var (command, inputs) in _commandInputs)
            {
                if (command is not SecretCommand)
                {
                    foreach (string input in inputs)
                    {
                        sb.Append('(').Append(input).Append(") ");
                    }
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }
    }
}
