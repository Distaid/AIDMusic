namespace AIDMusicApp.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int AccessId { get; set; }

        public byte[] Avatar { get; set; }

        public User Copy()
        {
            var user = new User
            {
                Id = Id,
                Login = Login,
                Password = Password,
                Phone = Phone,
                Email = Email,
                AccessId = AccessId
            };
            Avatar.CopyTo(user.Avatar, 0);

            return user;
        }
    }
}
