namespace MyMvcApp.ViewModels
{
    public class UserListViewModel
    {
        public string Id { get; set; } = string.Empty;

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string Role { get; set; } = string.Empty;
    }
}