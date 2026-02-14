using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Helpers
{
    public class CodeOfferGenerator
    {
        public static string GenerateRandomCode()
        {
            return "OFFER-" + Guid.NewGuid().ToString().Substring(0, 12).ToUpper();
        }
    }
}
