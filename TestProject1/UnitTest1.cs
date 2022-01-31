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
            //����� ������������, �������� ��� � ���� ������ �� ������;
            string user1Login = "thisUserDoesNotExistLogin";
            var temp = WpfApp1.AppContext.FindUserByLogin(user1Login);
            Assert.AreEqual(false, temp);
        }
        [Test]
        public void Test2()
        {
            //����� ������������, ������� ���� � ���� ������ �� ������;
            string user2Login = "newTestUser";
            var temp = WpfApp1.AppContext.FindUserByLogin(user2Login);
            Assert.AreEqual(true, temp);
        }
        [Test]
        public void Test3()
        {
            //����� ������������, �������� ��� � ���� ������ �� ������ � ������;
            string user1Login = "thisUserDoesNotExistLogin";
            string user1Password = "thisUserDoesNotExistPassword";
            var temp = WpfApp1.AppContext.FindUserByLoginAndPass(user1Login,user1Password);
            Assert.AreEqual(false, temp);
        }
        [Test]
        public void Test4()
        {
            //����� ������������, ������� ���� � ���� ������ �� ������ � ������, ������ �� ������;
            string user1Login = "newTestUser";
            string user1Password = "thisUserDoesNotExistPassword";
            var temp = WpfApp1.AppContext.FindUserByLoginAndPass(user1Login, user1Password);
            Assert.AreEqual(false, temp);
        }
        [Test]
        public void Test5()
        {
            //����� ������������, ������� ���� � ���� ������ �� ������ � ������, ������ ������;
            string user1Login = "newTestUser";
            string user1Password = "password123";
            var temp = WpfApp1.AppContext.FindUserByLoginAndPass(user1Login, user1Password);
            Assert.AreEqual(true, temp);
        }
    }
}