using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Home.AppLogic.Comparers
{
    public class DistanceAndWwaClassComparer : IDistanceAndWwaClassComparer
    {
        public bool Equals(Class x, Class y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return x.EventStart.Date == y.EventStart.Date
                   && x.EventEnd.Date == y.EventEnd.Date
                   && NormalizedLocation(x.Location) == NormalizedLocation(y.Location)
                   && x.ClassTypeId == y.ClassTypeId;
        }

        public int GetHashCode(Class obj)
        {
            return $"{obj.EventStart.Date} {obj.EventEnd.Date} {NormalizedLocation(obj.Location)} {obj.ClassTypeId}".GetHashCode();
        }

        private string NormalizedLocation(string location)
        {
            var result = location.ToLower();
            string[] unwantedChars = {" ", ",", "-", "(", ")"};
            foreach (var unwanted in unwantedChars)
            {
                result = result.Replace(unwanted, "");
            }

            result = result.Trim();
            return result;
        }
    }
}
