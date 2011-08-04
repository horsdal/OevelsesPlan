using System;

namespace ØvelsesPlan.Model
{
    public class Exercise
    {
        public Exercise(string name, string muscleGroup, string muscle, bool active)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}