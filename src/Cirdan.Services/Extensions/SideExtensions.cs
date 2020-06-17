using System.ComponentModel;
using Cirdan.Config;
using Cirdan.Models;

namespace Cirdan.Services.Extensions
{
    public static class SideExtensions
    {
        public static string ToString(this Side side)
        {
            switch (side)
            {
                case Side.Bid:
                    return "b";
                case Side.Ask:
                    return "s";
            }

            throw new InvalidEnumArgumentException(nameof(side));
        }

        public static Side ToSideEnum(this string side)
        {
            switch (side)
            {
                case "b":
                    return Side.Bid;
                case "s":
                    return Side.Ask;
                default:
                    throw new ValidationException(Validation.InvalidOrderSideValue);
            }
        }
    }
}
