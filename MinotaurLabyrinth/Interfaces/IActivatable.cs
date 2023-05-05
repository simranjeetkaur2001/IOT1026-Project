namespace MinotaurLabyrinth
{
    public interface IActivatable
    {
        bool DisplaySense(Hero hero, int heroDistance);
        void Activate(Hero hero, Map map);
    }
}