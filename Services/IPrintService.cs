using System;
using System.Collections.Generic;
using System.Text;

namespace MyJournal.Services
{
    public interface IPrintService
    {
        Task Print(string jobName, string htmlContent);
    }
}
