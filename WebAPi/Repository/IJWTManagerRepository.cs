using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPi.Models;

namespace WebAPi.Repository
{
    public interface IJWTManagerRepository
    {
        Tokens Authenticate(User users);
    }
}
