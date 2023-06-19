using System;
using System.Collections.Generic;

namespace MinotaurLabyrinth
{
 
    public class EggRoom : Room
    {
        static EggRoom()
        {
            RoomFactory.Instance.Register(RoomType.EggRoom, () => new EggRoom());
        }

        /// <inheritdoc/>
        public override RoomType Type { get; } = RoomType.EggRoom;

        /// <inheritdoc/>
        public override bool IsActive { get; protected set; } = true;

        public override void Activate(Hero hero, Map map)
        {
            if (IsActive)
            {
                ConsoleHelper.WriteLine("You Found an egg !", ConsoleColor.Yellow);
                ConsoleHelper.WriteLine("The egg is cracking...........", ConsoleColor.Yellow);


                var random = new Random();
                var list = new List<string>{"Monster", "Chick"};
                int index = random.Next(list.Count);

                if (list[index]=="chick"){
                    IsActive = false;
                    ConsoleHelper.WriteLine("Its a Chick, You are Safe... Yayyyyy", ConsoleColor.Green);
                }else{
                    ConsoleHelper.WriteLine("Its a Dinasour, You are attacked by a Dinasour..., you are dead.", ConsoleColor.Red);
                }

            }
        }

        /// <inheritdoc/>
        public override DisplayDetails Display()
        {
            return IsActive ? new DisplayDetails($"[{Type.ToString()[0]}]", ConsoleColor.Yellow)
                            : base.Display();
        }

        /// <summary>
        /// Displays sensory information about the riddle room, based on the hero's distance from it.
        /// </summary>
        /// <param name="hero">The hero sensing the riddle room.</param>
        /// <param name="heroDistance">The distance between the hero and the riddle room.</param>
        /// <returns>Returns true if a message was displayed; otherwise, false.</returns>
        public override bool DisplaySense(Hero hero, int heroDistance)
        {
            if (!IsActive)
            {
                if (base.DisplaySense(hero, heroDistance))
                {
                    return true;
                }
                if (heroDistance == 0)
                {
                    ConsoleHelper.WriteLine("You recall the riddle you solved in this room, now devoid of its mystical aura.", ConsoleColor.DarkGray);
                    return true;
                }
            }
            else if (heroDistance == 1 || heroDistance == 2)
            {
                ConsoleHelper.WriteLine(heroDistance == 1 ? "You sense a riddle. There is a riddle room nearby!" : "Your intuition tells you that something intriguing is nearby", ConsoleColor.DarkGray);
                return true;
            }
            return false;
        }
    }
}