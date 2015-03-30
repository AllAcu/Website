using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Domain.Authentication;
using Microsoft.Its.Domain;
using Newtonsoft.Json;

namespace Domain.Repository
{
    public class UserRepository
    {
        private readonly string userFile;
        private IDictionary<UserId, UserPrincipal> usersById;
        private IDictionary<Username, UserPrincipal> usersByName;

        public UserRepository(string userFile)
        {
            this.userFile = userFile;
            Load();
        }

        public UserRepository(IEnumerable<UserPrincipal> users)
        {
            usersById = users.ToDictionary(u => new UserId(u.Id.ToString()), u => u);
        }

        public IEnumerable<UserPrincipal> GetUsers()
        {
            return usersById.Values;
        }

        public UserPrincipal GetUser(UserId id)
        {
            return usersById.ContainsKey(id) ? usersById[id] : null;
        }

        public UserPrincipal GetUser(Username name)
        {
            return usersByName.ContainsKey(name) ? usersByName[name] : null;
        }

        public void Save(string email, string fullName, string password)
        {
            var contents = File.ReadAllText(userFile);
            var users = JsonConvert.DeserializeObject<List<UserRecord>>(contents);
            if (users.Any(u => string.Compare(email, u.Email, StringComparison.OrdinalIgnoreCase) == 0))
            {
                return;
            }

            users.Add(new UserRecord
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                Name = fullName
            });
            File.WriteAllText(userFile, JsonConvert.SerializeObject(users, Formatting.Indented));

            Load();
        }

        private void Load()
        {
            var contents = File.ReadAllText(userFile);
            usersById = JsonConvert.DeserializeObject<List<UserRecord>>(contents)
                .ToDictionary(i => new UserId(i.Id),
                    i => new UserPrincipal(id: new Guid(i.Id), name: i.Name, email: i.Email, ticket: "abcdefg"));
            usersByName = usersById.Values.ToDictionary(i => new Username(i.Email), i => i, Username.Compare);
        }

        public class Username : String<Username>
        {
            public Username(string value) : base(value)
            {

            }

            public static Comparer Compare = new Comparer();

            public class Comparer : IEqualityComparer<Username>
            {
                public bool Equals(Username x, Username y)
                {
                    return string.Compare(x.Value, y.Value, StringComparison.OrdinalIgnoreCase) == 0;
                }

                public int GetHashCode(Username obj)
                {
                    return obj.Value.GetHashCode();
                }
            }
        }

        private class UserRecord
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }
    }

    public class UserId : String<UserId>
    {
        public UserId(string value) : base(value)
        {
            
        }
    }
}
