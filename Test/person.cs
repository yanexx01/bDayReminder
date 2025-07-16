using System;

namespace Test
{
    enum Gender
    {
        муж,
        жен,
        none
    }

    class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Second_name { get; set; }
        public DateOnly Date_of_birth { get; set; }
        public Gender Gender { get; set; }

        public string Output(int count, bool ext)
        {
            if (!ext) return $"[{count + 1}] {Second_name} {Name[0]}. - {Date_of_birth}";
            else return $"[{count + 1}] {Second_name} {Name}, {Gender} - {Date_of_birth}";
        }
    }
    
}

