namespace AIDMusicApp.Models
{
    public class MusicianSkill
    {
        public int Id { get; set; }

        public int MusicianId { get; set; }

        public int SkillId { get; set; }

        public MusicianSkill Copy()
        {
            return new MusicianSkill
            {
                Id = Id,
                MusicianId = MusicianId,
                SkillId = SkillId
            };
        }
    }
}
