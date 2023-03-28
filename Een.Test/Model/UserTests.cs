using Een.Model;

namespace Een.Test.Model
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void CheckPassword_ShouldReturnTrue()
        {
            // Arrange
            const string username = "testuser";
            const string password = "testpassword";
            User user = new(username, password);

            // Act
            bool result = user.CheckPassword(password);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void CheckPassword_ShouldReturnFalse()
        {
            // Arrange
            const string username = "testuser";
            const string password = "testpassword";
            User user = new(username, password);

            // Act
            bool result = user.CheckPassword("wrongpassword");

            // Assert
            Assert.That(result, Is.False);
        }
    }
}