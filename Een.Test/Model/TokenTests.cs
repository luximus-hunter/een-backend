using Een.Model;

namespace Een.Test.Model
{
    [TestFixture]
    public class TokenTests
    {
        [Test]
        public void Constructor_ShouldSetProperties()
        {
            // Arrange
            User user = new("testuser", "testpassword");

            // Act
            Token token = new(user);

            // Assert
            Assert.That(token.User, Is.EqualTo(user));
        }
    }
}