using TvMazeAPI.Core.Services.Interfaces;

namespace TvMazeAPI.Core.Services.Implementations
{
    public class TvShowValidator : ITvShowValidator
    {
        public (bool IsValid, string ErrorMessage) ValidateMonthAndYear(int month, int year)
        {
            if (IsValidMonth(month) && IsValidYear(year))
            {
                return (true, "Validation successfull");
            }

            if (!IsValidMonth(month) && IsValidMonth(year))
            {
                return (false, "Onjuiste maand- en jaardwaarde. De maand moet tussen de 1 en 12 zijn. Het jaar moet tussen 1951 en 2100 zijn.");
            }

            if (!IsValidMonth(month))
            {
                return (false, "Onjuiste maandwaarde. De maand moet tussen de 1 en 12 zijn.");
            }

            return (false, "Onjuiste jaarwaarde. Het jaar moet tussen 1951 en 2100 zijn");
        }
        public bool IsValidMonth(int month)
        {
            return month >= 1 && month <= 12;
        }

        public bool IsValidYear(int year)
        {
            return year >= 1951 && year <= 2100;
        }

    }
}
