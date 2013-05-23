using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IBCSApp.Services.API;
using IBCSApp.Models;
using IBCS.BF.Key;

namespace IBCSAppTest
{
    [TestClass]
    public class APIServicesTest
    {
        [TestMethod, TestCategory("API")]
        public void LoginServiceTest()
        {
            ILoginService loginService = new LoginService();
        }
    }
}
