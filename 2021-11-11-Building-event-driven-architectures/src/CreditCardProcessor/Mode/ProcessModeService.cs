using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace CreditCardProcessor
{
    public class ProcessModeService : IProcessModeService
    {
        public const string MODE_KEY = "MODE";

        private readonly ConcurrentDictionary<string, ProcessMode> modeStorage = new ConcurrentDictionary<string, ProcessMode>();

        public ProcessMode Mode => modeStorage[MODE_KEY];

        public ProcessModeService()
        {
            SetMode(ProcessMode.Normal);
        }

        public void SetProcessMode(int mode)
        {
            if (mode <= 0 || mode > 3)
                throw new InvalidOperationException();

            var processMode = (ProcessMode)mode;
            SetMode(processMode);
        }

        private void SetMode(ProcessMode mode)
        {
            modeStorage.AddOrUpdate(MODE_KEY, mode, (key, existing) => mode);
        }
    }
}
