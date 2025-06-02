using Domain.Enums;

namespace Domain.Extensions
{
    public static class GenderExtensions
    {
        public static string GetDisplayName(this Gender gender)
        {
            return gender switch
            {
                Gender.Male => "Kişi",
                Gender.Female => "Qadın",
                _ => gender.ToString()
            };
        }
    }
}
