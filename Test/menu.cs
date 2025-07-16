using System;
using System.Runtime.InteropServices;
using Test;

namespace Test
{
    class MenuItem
    {
        public string Title { get; set; }
        protected Action<BirthdayManager> OnSelect { get; }

        public MenuItem(string title, Action<BirthdayManager> onSelect)
        {
            Title = title;
            OnSelect = onSelect;
        }

        public override string ToString() => Title;

        public void Execute(BirthdayManager people) => OnSelect(people);
    }

    abstract class Menu
    {
        private List<MenuItem> items = new List<MenuItem>();
        public string Title = "Menu";
        public int selectedItemIndex { get; set; } = 0;
        public BirthdayManager birthdayManager;

        public Menu(BirthdayManager birthdayManager)
        {
            this.birthdayManager = birthdayManager;
        }

        public void AddItem(MenuItem item)
        {
            items.Add(item);
        }

        virtual public void UpperOutput()
        {
            Console.WriteLine($"{Title}\n");
        }

        public void Run(bool needToBack)
        {
            // Console.CursorVisible = false;
            if (items.Count == 0)
            {
                Console.WriteLine("Нет пунктов меню!");
                Console.ReadKey();
                return;
            }
            do
            {
                Console.Clear();
                UpperOutput();

                for (int i = 0; i < items.Count; i++)
                {
                    if (i == selectedItemIndex)
                    {
                        Console.WriteLine($"> {items[i]}");
                    }
                    else
                    {
                        Console.WriteLine($"  {items[i]}");
                    }
                }

                ConsoleKey key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedItemIndex = (selectedItemIndex - 1 + items.Count) % items.Count;
                        break;

                    case ConsoleKey.DownArrow:
                        selectedItemIndex = (selectedItemIndex + 1) % items.Count;
                        break;

                    case ConsoleKey.Enter:
                        Console.Clear();
                        items[selectedItemIndex].Execute(birthdayManager);

                        if (needToBack) return;
                        break;

                    case ConsoleKey.Escape:
                        return;
                }

            }
            while (true);
        }
        
    public static string GetDayWord(int n)
        {
            if (n % 10 == 1 && n % 100 != 11) return "день";
            if (n % 10 >= 2 && n % 10 <= 4 && (n % 100 < 10 || n % 100 >= 20)) return "дня";
            return "дней";
        }
    }
    
}

