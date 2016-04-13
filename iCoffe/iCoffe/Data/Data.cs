using System;
using System.Collections.Generic;
using System.Text;

namespace iCoffe.Shared
{
    public class Data
    {
        public List<CoffeeHouseNet> Nets { get; }

        public Data ()
        {
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
