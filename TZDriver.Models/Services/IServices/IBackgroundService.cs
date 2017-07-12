using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TZDriver.Models.Services.IServices
{
    public interface IBackgroundService
    {
        Task<bool> CheckApplicationtriggerStatus();
    }
}
