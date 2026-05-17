using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Company_Registration_API.Utils
{
    public class PasswordHelper
    {
       private readonly PasswordHasher _hasher = new PasswordHasher();

    public string Hash(string password)
    {
        return _hasher.HashPassword(password);
    }

    public bool Verify(string hashedPassword, string inputPassword)
    {
            return _hasher.VerifyHashedPassword(
                hashedPassword,
                inputPassword)== PasswordVerificationResult.Success;
    }
    }
}