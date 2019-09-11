using System;
using System.Collections.Generic;
using System.Text;

namespace NoteTaker.Core.Services.Interfaces
{
    public interface ITokenService
    {
        string GetToken(string userId, string username);
    }
}
