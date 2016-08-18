namespace iCoffe.Shared
{
    public class User
    {
        public string Email { set; get; }

        public string GRAvatar { set; get; }

        public string Avatar { set; get; }

        public string FirstName { set; get; }

        public string LastName { set; get; }

        public string City { set; get; }
    }
    
    public class UserInfo
    {
        public string FullUserName { set; get; }

        public string Login { set; get; }

        public int Points { set; get; }
    }
}
