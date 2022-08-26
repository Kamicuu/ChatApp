
using System.Collections.Generic;

namespace ChatApp.Models
{
    internal sealed class Database
    {
        private IEnumerable<ChatRoom> _chatRooms;
        internal IEnumerable<ChatRoom> ChatRooms { get { return _chatRooms; } }

        private static Database instance = null;
        private static readonly object padlock = new object();

        private Database()
        {
            _chatRooms = new List<ChatRoom>();
        }

        public static Database Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Database();
                    }
                    return instance;
                }
            }
        }
    }
}
