using System;
using System.Collections.Generic;
using System.Text;

namespace MyJournal.Services.Interfaces
{
    public interface IPrintService
    {
        Task Print(string jobName, string htmlContent);
    }
}
