using Een.Model;

namespace Een.Data;

public static class Users
{
    public static User? Get(Guid id)
    {
        Database db = new();

        return db.Users.First(u => u.Id == id);
    }

    public static User? Get(string username)
    {
        Database db = new();

        return db.Users.First(u => u.Username == username);
    }

    public static User? Get(string username, string password)
    {
        Database db = new();

        return db.Users.First(u => u.Username == username && u.Password == password);
    }

    public static bool Add(User user)
    {
        Database db = new();

        if (db.Users.Any(u => u.Id == user.Id))
        {
            return false;
        }

        db.Users.Add(user);
        db.SaveChanges();

        return true;
    }

    public static bool Update(User user)
    {
        Database db = new();

        User? dbUser = Get(user.Id);

        if (dbUser == null)
        {
            return false;
        }

        // TODO: Uuh, fix this
        dbUser.Username = user.Username;
        dbUser.Password = user.Password;
        dbUser.Wins = user.Wins;
        dbUser.Loses = user.Loses;
        dbUser.ProfileImage = user.ProfileImage;

        db.SaveChangesAsync();

        return true;
    }

    public static bool Remove(Guid id)
    {
        Database db = new();

        User? user = Get(id);

        if (user == null)
        {
            return false;
        }

        db.Users.Remove(user);
        db.SaveChanges();

        return true;
    }
}