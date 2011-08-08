using System;

namespace ØvelsesPlan.Model
{
    public class Exercise
    {
        public enum PropertyNumbers
        {
            Name = 0,
            MuscleGroup,
            Muscle,
            ActiveText,
            Description
        }

        private static int id = 0;

        public Exercise(string name, string muscleGroup, string muscle, bool active, string description)
            : this((++id).ToString(), name,muscleGroup, muscle, active, description)
        {
        }

        public Exercise(string id, string name, string muscleGroup, string muscle, bool active, string description)
        {
            Name = name;
            MuscleGroup = muscleGroup;
            Muscle = muscle;
            Active = active;
            Description = description;
            Id = id;
        }

        public string Name { get; set; }
        public string Id { get; set; }
        public string MuscleGroup { get; set; }
        public string Muscle { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public string ActiveText { get { return Active ? "Ja" : "Nej"; } }
        public string DT_RowId { get { return Id; } }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
                return false;

            var rhs = obj as Exercise;
            return this.Id == rhs.Id && this.Name == rhs.Name && this.MuscleGroup == rhs.MuscleGroup &&
                   this.Muscle == rhs.Muscle && this.Active == rhs.Active && this.Description == rhs.Description;
        }

        public void UpdatePropertyNumber(PropertyNumbers propertyNumber, object newValue)
        {
            switch (propertyNumber)
            {
                case PropertyNumbers.Name:
                    Name = (string) newValue;
                    break;
                case PropertyNumbers.MuscleGroup:
                    MuscleGroup = (string) newValue;
                    break;
                case PropertyNumbers.Muscle:
                    Muscle = (string) newValue;
                    break;
                case PropertyNumbers.ActiveText:
                    Active = (bool) newValue;
                    break;
                case PropertyNumbers.Description:
                    Description = (string) newValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("propertyNumber");
            }
        }
    }
}