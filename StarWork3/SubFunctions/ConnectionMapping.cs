using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections;

namespace StarWork3.SubFunctions
{
    public class ConnectionMapping
    {
        public Dictionary<string, string> NickNames = new();
        public int Count { get { return NickNames.Count; } }

        public void Add(string key, string nickName)
        {
            if (!NickNames.ContainsKey(key))
                NickNames.Add(key, nickName);
        }

        public void Remove(string key)
        {
            NickNames.Remove(key);
        }

        public bool CheckConnection(string nickName)
        {
            foreach (var nick in NickNames)
            {
                if (nick.Value.Equals(nickName)) 
                    return true;
            }
            return false;
        }

        public Dictionary<string, bool> CheckRangeConnection(IEnumerable<string> nickNames)
        {
            Dictionary<string, bool> results = new();
            foreach (string nickName in nickNames)
            {
                results.Add(nickName, false);
                foreach (var nick in NickNames)
                {
                    if (nick.Value.Equals(nickName))
                        results[nickName] = true;
                }
            }
            return results;
        }
    }
}
