using NUnit.Framework;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void Test1()
        {
            //поиск пользователя, которого нет в базе данных по логину;
            string user1Login = "thisUserDoesNotExistLogin";
            var temp = WpfApp1.AppContext.FindUserByLogin(user1Login);
            Assert.AreEqual(false, temp);
        }
        [Test]
        public void Test2()
        {
            //поиск пользователя, который есть в базе данных по логину;
            string user2Login = "newTestUser";
            var temp = WpfApp1.AppContext.FindUserByLogin(user2Login);
            Assert.AreEqual(true, temp);
        }
        [Test]
        public void Test3()
        {
            //поиск пользователя, которого нет в базе данных по логину и паролю;
            string user1Login = "thisUserDoesNotExistLogin";
            string user1Password = "thisUserDoesNotExistPassword";
            var temp = WpfApp1.AppContext.FindUserByLoginAndPass(user1Login,user1Password);
            Assert.AreEqual(false, temp);
        }
        [Test]
        public void Test4()
        {
            //поиск пользователя, который есть в базе данных по логину и паролю, пароль не верный;
            string user1Login = "newTestUser";
            string user1Password = "thisUserDoesNotExistPassword";
            var temp = WpfApp1.AppContext.FindUserByLoginAndPass(user1Login, user1Password);
            Assert.AreEqual(false, temp);
        }
        [Test]
        public void Test5()
        {
            //поиск пользователя, который есть в базе данных по логину и паролю, пароль верный;
            string user1Login = "newTestUser";
            string user1Password = "password123";
            var temp = WpfApp1.AppContext.FindUserByLoginAndPass(user1Login, user1Password);
            Assert.AreEqual(true, temp);
        }
    }
}