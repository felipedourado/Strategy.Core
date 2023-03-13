using Strategy.Core.Domain.Base;

namespace Strategy.Core.Domain.Models
{
    public class AccountBase : MongoDocumentBase
    {
        public int Agency { get; set; }
        public int Account { get; set; }
    }
}
