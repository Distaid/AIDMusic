namespace AIDMusicApp.Models
{
    public class MusicianSkills
    {
        public int Id { get; set; }

        public int MusicianId { get; set; }

        public int SkillId { get; set; }

        public MusicianSkills Copy()
        {
            return new MusicianSkills
            {
                Id = Id,
                MusicianId = MusicianId,
                SkillId = SkillId
            };
        }
    }
}
