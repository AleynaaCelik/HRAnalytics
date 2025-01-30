using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Core.Constants
{
   
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Employee = "Employee";

        public static readonly IReadOnlyList<string> AllRoles = new[]
        {
            Admin,
            Manager,
            Employee
        };
    }
}



