using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using CachingExample.Utilities;
using CachingExample.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace CachingExample.Controllers
{
    [Route("api")]
    public class ValuesController : Controller
    {

        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;

        public ValuesController(
            IMemoryCache memoryCache,
            IDistributedCache distributedCache)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        // GET api/users
        // Cache item in memory for specific time interval
        [HttpGet("user/memory")]
        public UserViewModel GetUserMemory()
        {
            var user = _memoryCache.Get<UserViewModel>("user");
            if (user == null)
            {
                user = new UserViewModel
                {
                    Id = 1,
                    Name = "Shahid",
                    Email = "shahidcse@gmail.com",
                    Address = "Dhaka"
                };
                _memoryCache.Set("user", user, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
                });
            }
            return user;
        }

        // GET api/users
        // Cache item in distributed redis server for specific time interval
        [HttpGet("user/distributed")]
        public async Task<UserViewModel> GetUserDistributed()
        {
            var bytes = await _distributedCache.GetAsync("user");

            var user = SerializationUtilities.Deserialize<UserViewModel>(bytes);

            if (user == null)
            {
                user = new UserViewModel
                {
                    Id = 1,
                    Name = "Shahid",
                    Email = "shahidcse@gmail.com",
                    Address = "Dhaka"
                };

                var data = SerializationUtilities.Serialize(user);
                if (data != null)
                {
                    await _distributedCache.SetAsync("user", data, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
                    });
                }
            }
            return user;
        }

    }
}
