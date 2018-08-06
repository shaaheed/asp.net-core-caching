using System;

namespace CachingExample.ViewModels
{
    [Serializable]
    public class UserViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
