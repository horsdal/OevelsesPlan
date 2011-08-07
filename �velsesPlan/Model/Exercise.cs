using System;

namespace ØvelsesPlan.Model
{
    public class Exercise
    {
        public Exercise(string name, string muscleGroup, string muscle, bool active, string description)
        {
            Name = name;
            MuscleGroup = muscleGroup;
            Muscle = muscle;
            Active = active;
            Description = description;
        }

        public string Name { get; set; }
        public string Id { get; set; }
        public string MuscleGroup { get; set; }
        public string Muscle { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
                return false;

            var rhs = obj as Exercise;
            return this.Id == rhs.Id && this.Name == rhs.Name && this.MuscleGroup == rhs.MuscleGroup &&
                   this.Muscle == rhs.Muscle && this.Active == rhs.Active && this.Description == rhs.Description;
        }

        public string[] Flatten()
        {
            return new[]
                       {
                           Name,
                           MuscleGroup,
                           Muscle,
                           Active ? "Ja" : "Nej",
                           Description
                       };
        }
    }
}