using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;


namespace iCoffe.Shared
{
    public class Data
    {
        public static List<CoffeeHouseNet> Nets { get; }
        public static List<GeolocObj> Objs { get; set; }
        public static List<BonusOffer> Offers { get; set; }
        public static List<GeolocComplexObj> ComplexObjs { get; set; }
        public static GeolocPoint center { get; set; }

        public static string SerializeUser(User user)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(User));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, user);
                return textWriter.ToString();
            }
        }

        public static User DeserializeUser(string user)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(User));

            using (TextReader reader = new StringReader(user))
            {
                return (User)xmlSerializer.Deserialize(reader);
            }
        }



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

		public static GeolocObj GetObj(int id)
		{
			if (Objs == null)
			{
				return null;
			}
			else
			{
				foreach (var obj in Objs)
				{
					if(obj.Id == id)
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

            Objs = new List<GeolocObj>();
			Objs.Add (new GeolocObj () {
				Id = 1, 
				geoloc = new GeolocPoint(){
					x = 54.9773609, y = 73.3880858
				},
				group_Id = 1,

				// Info
				Adress = "ул. Маршала Жукова, 70Ак4",
				Label = "Pic",
				Title = "ТинТо-Кофе",
				Descr = "tinto-coffee.ru 8 (381) 232-50-89"
			});
			Objs.Add (new GeolocObj () {
				Id = 2, 
				geoloc = new GeolocPoint(){
					x = 54.9794035, y = 73.3810118
				},
				group_Id = 2,

				// Info
				Adress = "Карла Маркса просп., 5А",
				Label = @"https://upload.wikimedia.org/wikipedia/ru/2/2a/Travelers_Coffee_Logo.png",
				Title = "Traveler's Coffee - ТОПАЗ",
				Descr = "travelerscoffee.ru 8 (381) 231-78-62",
				ImageURL = @"https://cdn01.travelerscoffee.ru/files/joints/photos/a69af159cca8a3f5c7be7e3556677bd4cf6128c800375.jpg"
			});
			Objs.Add (new GeolocObj () {
				Id = 3, 
				geoloc = new GeolocPoint(){
					x = 54.9794405, y = 73.3771154
				},
				group_Id = 1,

				// Info
				Adress = "Карла Маркса просп., 10",
				Label = @"http://www.tinto-coffee.ru/sites/default/files/199.png",
				Title = "ТинТо-Кофе",
				Descr = "tinto-coffee.ru 8 (381) 237-04-37",
				ImageURL = @"https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcSuGF8r0DYCUYIAznWyFm9B-0H9FyRzwVrmb_0l1o3_gWqvfGDblQ"
			});
			Objs.Add (new GeolocObj () {
				Id = 4, 
				geoloc = new GeolocPoint(){
					x = 54.9794645, y = 73.3770725
				},
				group_Id = 3,

				// Info
				Adress = "ул. Ленина, 31",
				Label = "Pic",
				Title = "Вояж",
				Descr = "8 (381) 237-09-10"
			});

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
