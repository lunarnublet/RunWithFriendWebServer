using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;

namespace WebService.Services
{
    public static class RouteEx
    {
        public static string PrettyLatLng(string latLng, int maxLength = -1)
        {
            var split = latLng.Split(',');

            if (split.Length == 2)
            {
                if (maxLength > 0)
                {
                    if (split[0].Length > maxLength)
                    {
                        int indexOfDot = split[0].IndexOf('.');
                        indexOfDot = indexOfDot > 0 ? indexOfDot : 0;
                        split[0] = split[0].Substring(0, Math.Min(indexOfDot + maxLength, split[0].Length));
                    }
                    if (split[1].Length > maxLength)
                    {
                        int indexOfDot = split[1].IndexOf('.');
                        indexOfDot = indexOfDot > 0 ? indexOfDot : 0;
                        split[1] = split[1].Substring(0, Math.Min(indexOfDot + maxLength, split[1].Length));
                    }
                }

                return split[0] + ", " + split[1];
            }

            throw new ArgumentException("latLng");
        }
    }
}