using System;
using System.Collections.Generic;
using System.Text;

namespace CreditCardProcessor
{
    public interface IProcessModeService
    {
        void SetProcessMode(int mode);
        ProcessMode Mode { get; }
    }
}
