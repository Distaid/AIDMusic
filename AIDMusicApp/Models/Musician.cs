using System.Collections.Generic;

namespace AIDMusicApp.Models
{
    public class Musician
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte Age { get; set; }

        public int CountryId { get; set; }

        public bool IsDead { get; set; }

        public List<Skill> Skills { get; set; }

        public Musician Copy()
        {
            var musician = new Musician
            {
                Id = Id,
                Name = Name,
                Age = Age,
                CountryId = CountryId,
                IsDead = IsDead,
                Skills = new List<Skill>()
            };

            Skills.ForEach(item => musician.Skills.Add(item.Copy()));

            return musician;
        }
    }
}
