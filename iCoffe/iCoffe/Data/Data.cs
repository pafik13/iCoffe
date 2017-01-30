using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;

namespace iCoffe.Shared
{
    public class Data
    {
        public static List<Offer> Offers { get; set; }
        public static List<Place> Places { get; set; }
        public static List<PlaceInfo> PlaceInfos { get; set; }
        public static UserInfo UserInfo { get; set; }
        public static List<Offer> UserPurchasedOffers { get; set; }

        //public static string SerializeUser(User user)
        //{
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(User));

        //    using (StringWriter textWriter = new StringWriter())
        //    {
        //        xmlSerializer.Serialize(textWriter, user);
        //        return textWriter.ToString();
        //    }
        //}

        public static Place GetPlace(int placeId)
        {
            return Places.FirstOrDefault(place => place.Id == placeId);
        }

        public static Offer GetOffer(int offerId)
        {
            return Offers.FirstOrDefault(offer => offer.Id == offerId);
        }

        //internal static BonusOffer GetBonusOffer(Guid guid)
        //{
        //    return BonusOffers.FirstOrDefault(offer => offer.Id == guid);
        //}

        //public static User DeserializeUser(string user)
        //{
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(User));

        //    using (TextReader reader = new StringReader(user))
        //    {
        //        return (User)xmlSerializer.Deserialize(reader);
        //    }
        //}

        static Data ()
        {
            System.Diagnostics.Debug.WriteLine("Data class constroctor called.", "Data");
        }
    }
}
