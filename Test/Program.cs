namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            BirthdayManager bm = new BirthdayManager();
            MainMenu m = new MainMenu(bm);

            m.Run(false);
        }
    }
}
