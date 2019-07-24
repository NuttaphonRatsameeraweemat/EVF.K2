using EVF.K2.Bll.Models;
using System.Collections.Generic;

namespace EVF.K2.Bll.Interfaces
{
    public interface ISmartObject
    {
        List<Dictionary<string, object>> GetSmartObject(SmartObjectModel model);
    }
}
