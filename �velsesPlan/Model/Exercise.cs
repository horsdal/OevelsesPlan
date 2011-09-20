﻿using System;

namespace ØvelsesPlan.Model
{
    public class Exercise
    {
        public enum PropertyNumbers
        {
            Name = 0,
            MuscleGroup,
            Muscle,
            Description,
            ActiveText
        }

        public Exercise(string name, string muscleGroup, string muscle, bool active, string description)
            : this(Guid.NewGuid().ToString(), name, muscleGroup, muscle, active, description)
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
            var rhs = obj as Exercise;
            return rhs != null && this.Id == rhs.Id && this.Name == rhs.Name && this.MuscleGroup == rhs.MuscleGroup &&
                   this.Muscle == rhs.Muscle && this.Active == rhs.Active && this.Description == rhs.Description;
        }

        public void UpdatePropertyNumber(PropertyNumbers propertyNumber, string newValue)
        {
            switch (propertyNumber)
            {
                case PropertyNumbers.Name:
                    Name = newValue;
                    break;
                case PropertyNumbers.MuscleGroup:
                    MuscleGroup = newValue;
                    break;
                case PropertyNumbers.Muscle:
                    Muscle = newValue;
                    break;
                case PropertyNumbers.ActiveText:
                    Active = newValue == "Ja" ? true : false;
                    break;
                case PropertyNumbers.Description:
                    Description = newValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("propertyNumber");
            }
        }
    }
}