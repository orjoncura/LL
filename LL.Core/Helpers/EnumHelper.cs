namespace LL.Core.Helpers;

public static class EnumHelper
{
    public static int GetEnumValue(Type enumType, string value)
    {
        if (!enumType.IsEnum)
        {
            Console.WriteLine("Provided type is not an enum.");
            return 0; // Return 0 if not an enum
        }

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
}