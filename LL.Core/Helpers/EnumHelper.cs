namespace LL.Core.Helpers;

public static class EnumHelper
{
    public static int GetEnumValue(Type enumType, string value)
    {
        // Return 0 if not an enum
        if (!enumType.IsEnum)
            return 0; 
        
        // Convert the input string to lowercase for case-insensitive comparison
        string lowerValue = value.ToLower();

        // Loop through all names of the enum
        foreach (string enumName in Enum.GetNames(enumType))
        {
            // Convert enum name to lowercase
            if (enumName.ToLower() == lowerValue)
            {
                // Parse and return the matched enum value
                return (int)Enum.Parse(enumType, enumName);
            }
        }

        // If no match is found, return 0
        return 0;
    }
    
    public static string GetEnumValueById<TEnum>(int id) where TEnum : Enum
    {
        // Check if the id exists in the enum
        if (Enum.IsDefined(typeof(TEnum), id))
        {
            return Enum.GetName(typeof(TEnum), id); // Return the name as a string
        }

        return string.Empty; // Return null or handle invalid ID
    }
}