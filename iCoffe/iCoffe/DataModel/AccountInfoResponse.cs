using System.Collections.Generic;

namespace iCoffe.Shared
{
    public class AccountInfoResponse
    {
        public UserInfo User { get; set; }
        public List<Purchase> Purchases { get; set;  }
    }
}
