/*namespace Console.Data;

public class UserRoleFields
{
    public static string Get(int role)
    {
        var fields = typeof(UserRole).GetFields()
            .Where(x => x.FieldType == typeof(UserRole))
            .Select(x => x.Name)
            .ToList();
        return fields[role];
    } 
}*/