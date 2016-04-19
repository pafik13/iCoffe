using System.Collections.Generic;

namespace iCoffe.Shared
{
    public class Data
    {
        public static List<CoffeeHouseNet> Nets { get; }
        public static List<GeolocObj> Objs { get; set; }
        public static List<GeolocComplexObj> ComplexObjs { get; set; }
        public static GeolocPoint center { get; set; }

        public static GeolocComplexObj Get(int id)
        {
            if (ComplexObjs == null)
            {
                return null;
            }
            else
            {
                foreach (var obj in ComplexObjs)
                {
                    if(obj.Obj.Id == id)
                    {
                        return obj;
                    }
                }
                return null;
            }
        }

        static Data ()
        {
            ComplexObjs = new List<GeolocComplexObj>();

            Nets = new List<CoffeeHouseNet>();
            Nets.Add(new CoffeeHouseNet() {
                Name = @"First Net",
            });
            Nets.Add(new CoffeeHouseNet()
            {
                Name = @"Second Net",
            });
            Nets.Add(new CoffeeHouseNet()
            {
                Name = @"Third Net",
            });
            Nets.Add(new CoffeeHouseNet()
            {
                Name = @"Fourth Net",
            });
            Nets.Add(new CoffeeHouseNet()
            {
                Name = @"Fifth Net",
            });
            Nets.Add(new CoffeeHouseNet()
            {
                Name = @"Sixth Net",
            });
            Nets.Add(new CoffeeHouseNet()
            {
                Name = @"Seventh Net",
            });
        }
    }
}
